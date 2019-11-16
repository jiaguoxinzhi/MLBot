

using LinyeeSeq2Seq.Models;
using MLBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{
    /// <summary>
    /// Sequence To Sequence
    /// 序列到序列
    /// </summary>
    public class Seq2Seq
    {
        #region 参数或配置
        /// <summary>
        /// 训练开始事件
        /// </summary>

        public event EventHandler TrainStart;
        /// <summary>
        /// 训练完成事件
        /// </summary>

        public event EventHandler TrainDone;
        /// <summary>
        /// 世纪完成事件
        /// </summary>

        public event EventHandler EpochDone;
        /// <summary>
        /// 迭代完成事件
        /// </summary>

        public event EventHandler IterationDone;

        public int max_word = 300; // max length of generated sentences 
        /// <summary>
        /// 词转索引
        /// </summary>
        public Dictionary<string, WordInfoV1_1> wordToIndex ; 
        /// <summary>
        /// 索引转词
        /// </summary>
        public Dictionary<int, WordInfoV1_1> indexToWord ;
        /// <summary>
        /// 
        /// </summary>
        public List<string> vocab ;
        /// <summary>
        /// 输入语句组
        /// </summary>
        public List<List<string>> InputSequences;
        /// <summary>
        /// 输出语句组
        /// </summary>
        public List<List<string>> OutputSequences;
        /// <summary>
        /// 隐式大小 hdim 维
        /// </summary>
        public int hidden_size;
        /// <summary>
        /// 输入大小 列数 dim 维
        /// </summary>
        public int word_size;

        // optimization  hyperparameters
        public double regc = 0.000001; // L2 regularization strength
        public double learning_rate = 0.001; // learning rate
        public double clipval = 5.0; // clip gradients at this value

        /// <summary>
        /// 
        /// </summary>
        public Optimizer solver;
        /// <summary>
        /// 词嵌入向量
        /// 嵌入 权重矩阵
        /// 行vocab.count+2 列dim
        /// 相当于Word2Vec 语义
        /// </summary>
        public WeightMatrix Embedding;

        /// <summary>
        /// 隐式 权重矩阵
        /// </summary>
        //Output Layer Weights
        public WeightMatrix Whd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WeightMatrix bd { get; set; }
        /// <summary>
        /// 编码器
        /// </summary>
        public Encoder encoder;
        /// <summary>
        /// 
        /// </summary>
        public Encoder ReversEncoder;
        /// <summary>
        /// 解码器
        /// </summary>
        public LinyeeDecoder decoder; 

        /// <summary>
        /// 
        /// </summary>
        public bool UseDropout { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 模型名
        /// </summary>
        internal string ModelName { get; set; }
        #endregion

        #region ..ctor

        /// <summary>
        /// Sequence To Sequence
        /// 序列到序列
        /// </summary>
        public Seq2Seq(string ModelName = "Model.lys2s")
        {
            this.ModelName = ModelName;
        }

        /// <summary>
        /// Sequence To Sequence
        /// 序列到序列
        /// </summary>
        /// <param name="inputSize">输入大小 列数 dim</param>
        /// <param name="hiddenSize">隐式大小 hdim</param>
        /// <param name="depth">深度</param>
        /// <param name="input">输入语句组</param>
        /// <param name="output">输出句组</param>
        /// <param name="useDropout"></param>
        public Seq2Seq(int inputSize, int hiddenSize, int depth, List<List<string>> input, List<List<string>> output, bool useDropout,string ModelName= "Model.lys2s"):this(ModelName)
        {
            this.InputSequences = input;
            this.OutputSequences = output;
            this.Depth=depth;
            // size of word embeddings.
            word_size = inputSize; 
            // list of sizes of hidden layers
            this.hidden_size = hiddenSize;
            solver = new Optimizer();
            
            OneHotEncoding(input, output);

            this.Whd = new WeightMatrix(hidden_size , vocab.Count + 2,  true);
            this.Embedding = new WeightMatrix(vocab.Count + 2, word_size,   true);
            this.bd = new WeightMatrix(1, vocab.Count + 2, 0);             
          
            encoder = new Encoder(hidden_size, word_size, depth);
            ReversEncoder = new Encoder(hidden_size, word_size, depth);
            decoder = new LinyeeDecoder(hidden_size, word_size, depth);          
        }
        #endregion
        
        #region train 训练
        /// <summary>
        /// 键-热度 编码
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="_output"></param>
        internal void OneHotEncoding(List<List<string>> _input, List<List<string>> _output)
        {
            // count up all words
            //词频统计
            Dictionary<string, int> d = new Dictionary<string, int>();

            if (wordToIndex == null) wordToIndex = new Dictionary<string, WordInfoV1_1>();
            if (indexToWord == null) indexToWord = new Dictionary<int, WordInfoV1_1>();
            if (vocab == null) vocab = new List<string>();

            for (int j = 0, n2 = _input.Count; j < n2; j++)
            {
                var item = _input[j];
                for (int i = 0, n = item.Count; i < n; i++)
                {
                    var txti = item[i];
                    //if (string.IsNullOrEmpty(txti)) continue;
                    if (d.Keys.Contains(txti)) { d[txti] += 1; }
                    else { d.Add(txti, 1); }
                }

                var item2 = _output[j];
                for (int i = 0, n = item2.Count; i < n; i++)
                {
                    var txti = item2[i];
                    //if (string.IsNullOrEmpty(txti)) continue;
                    if (d.Keys.Contains(txti)) { d[txti] += 1; }
                    else { d.Add(txti, 1); }
                }
            }

            // NOTE: start at one because we will have START and END tokens!
            // that is, START token will be index 0 in model word vectors
            // and END token will be index 0 in the next word softmax
            // 赋值索引表 词表
            var q = 2;
            foreach (var ch in d)
            {
                if (ch.Value >= 1)
                {
                    // add word to vocab
                    var w = new WordInfoV1_1()
                    {
                        i = q,
                        f=ch.Value,
                        w=ch.Key,
                    };
                    wordToIndex.Add(w.w, w);
                    indexToWord.Add(w.i, w);
                    //wordToIndex[ch.Key].i = q;
                    //indexToWord[q].w = ch.Key;
                    vocab.Add(ch.Key);
                    q++;
                }
            }

            //Console.WriteLine($"keys>{string.Join(",",wordToIndex.Keys.ToArray())}");
        }

        private bool training = false;
        public string newType="new";

        /// <summary>
        /// 训练
        /// </summary>
        /// <param name="trainingEpoch">训练代数</param>
        public void Train(int trainingEpoch)
        {
            if (training)
            {
                Console.WriteLine("已有训练任务，正在进行。。。");
            }
            training = true;
            //调用事件
            if (TrainStart != null)
            {
                new Thread(()=> {
                    TrainStart(this, new CostEventArg()
                    {
                        Iteration = trainingEpoch
                    });
                }).Start();
            } 
            
            for (int ep = 0; ep < trainingEpoch; ep++)
            {
                Random r = new Random();
                for (int itr = 0; itr < InputSequences.Count; itr++)
                {
                    // sample sentence from data 数据中的样句
                    List<string> OutputSentence;
                    ComputeGraph g;
                    double cost;
                    List<WeightMatrix> encoded = new List<WeightMatrix>();
                    int sentIndex = itr;
                    if (ep == 1)
                    {
                        sentIndex = InputSequences.Count - 1 - itr;
                    }
                    else if (ep > 1)
                    {
                        r.Next(0, InputSequences.Count);
                    }
                    Encode(sentIndex, out OutputSentence, out g, out cost,ref encoded);
                    cost = DecodeOutput(OutputSentence, g, cost, encoded);

                    g.backward();
                    UpdateParameters();
                    Reset();

                    //调用事件
                    if (IterationDone != null)
                    {
                        new Thread(()=> {
                            IterationDone(this, new CostEventArg()
                            {
                                Cost = cost / OutputSentence.Count
                                ,
                                Iteration = ep
                            });
                        }).Start();
                    }                     
                }

                //调用事件
                if (EpochDone != null)
                {
                    new Thread(() => {
                        EpochDone(this, new CostEventArg()
                        {
                            Iteration = ep
                        });
                    }).Start();
                }
            }

            //调用事件
            if (TrainDone != null)
            {
                new Thread(()=> {
                    TrainDone(this, null);
                }).Start();
            }
            training = false;
        }

        /// <summary>
        /// 续训
        /// </summary>
        /// <param name="trainingEpoch">训练代数</param>
        public void ReTrain(int trainingEpoch, List<List<string>> input, List<List<string>> output)
        {
            if (training)
            {
                Console.WriteLine("已有训练任务，正在进行。。。");
            }
            training = true;

            this.InputSequences = input;
            this.OutputSequences = output;
            if(solver==null) solver = new Optimizer();
            OneHotEncoding(input, output);

            //调用事件
            if (TrainStart != null)
            {
                new Thread(() => {
                    TrainStart(this, new CostEventArg()
                    {
                        Iteration = trainingEpoch
                    });
                }).Start();
            }

            for (int ep = 0; ep < trainingEpoch; ep++)
            {
                Random r = new Random();
                for (int itr = 0; itr < InputSequences.Count; itr++)
                {
                    // sample sentence from data 数据中的样句
                    List<string> OutputSentence;
                    ComputeGraph g;
                    double cost;
                    List<WeightMatrix> encoded = new List<WeightMatrix>();
                    int sentIndex = itr;
                    if (ep == 1)
                    {
                        sentIndex = InputSequences.Count - 1 - itr;
                    }
                    else if (ep > 1)
                    {
                        r.Next(0, InputSequences.Count);
                    }
                    Encode(sentIndex, out OutputSentence, out g, out cost, ref encoded);
                    cost = DecodeOutput(OutputSentence, g, cost, encoded);

                    g.backward();
                    UpdateParameters();
                    Reset();

                    //调用事件
                    if (IterationDone != null)
                    {
                        new Thread(() => {
                            IterationDone(this, new CostEventArg()
                            {
                                Cost = cost / OutputSentence.Count
                                ,
                                Iteration = ep
                            });
                        }).Start();
                    }
                }

                //调用事件
                if (EpochDone != null)
                {
                    new Thread(() => {
                        EpochDone(this, new CostEventArg()
                        {
                            Iteration = ep
                        });
                    }).Start();
                }
            }

            //调用事件
            if (TrainDone != null)
            {
                new Thread(() => {
                    TrainDone(this, null);
                }).Start();
            }
            training = false;
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="sentIndex">对话索引</param>
        /// <param name="OutputSentence">输出的样句</param>
        /// <param name="g"></param>
        /// <param name="cost"></param>
        /// <param name="encoded"></param>
        private void Encode(int sentIndex, out List<string> OutputSentence, out ComputeGraph g, out double cost, ref  List<WeightMatrix> encoded)
        {
            //var sentIndex = r.Next(0, InputSequences.Count);
            var inputSentence = InputSequences[sentIndex];
            var reversSentence = InputSequences[sentIndex].ToList();
            reversSentence.Reverse();
            OutputSentence = OutputSequences[sentIndex];
            g = new ComputeGraph();

            cost = 0.0;             
            for (int i = 0; i < inputSentence.Count; i++)
            {
                int ix_source = wordToIndex[inputSentence[i]].i;//顺
                int ix_source2 = wordToIndex[reversSentence[i]].i;//逆
                var x = g.PeekRow(Embedding, ix_source);//查询指定行数据
                var eOutput = encoder.Encode(x, g);
                var x2 = g.PeekRow(Embedding, ix_source2);
                var eOutput2 = ReversEncoder.Encode(x2, g);
                encoded.Add( g.concatColumns(eOutput, eOutput2));
            }


            //if (UseDropout)
            //{
            //    encoded = g.Dropout(encoded, 0.2);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OutputSentence"></param>
        /// <param name="g"></param>
        /// <param name="cost"></param>
        /// <param name="encoded"></param>
        /// <returns></returns>
        private double DecodeOutput(List<string> OutputSentence, ComputeGraph g, double cost, List<WeightMatrix> encoded)
        {
            int ix_input = 1; 
            for (int i = 0; i < OutputSentence.Count + 1; i++)
            {
                int ix_target = 0;
                if (i == OutputSentence.Count) 
                {
                    ix_target = 0; 
                }
                else
                {
                    ix_target = wordToIndex[OutputSentence[i]].i;
                }

                var x = g.PeekRow(Embedding, ix_input);
                var eOutput = decoder.Decode(x,encoded, g);
                if (UseDropout)
                {
                    eOutput = g.Dropout(eOutput, 0.2);

                }
                var o = g.add(
                       g.mul(eOutput, this.Whd), this.bd);
                if (UseDropout)
                {
                    o = g.Dropout(o, 0.2);

                }

                var probs = g.SoftmaxWithCrossEntropy(o);
                cost += -Math.Log(probs.Weight[ix_target]);
                 
                o.Gradient = probs.Weight;
                o.Gradient[ix_target] -= 1;
                ix_input = ix_target;
            }
            return cost;
        }

        private void UpdateParameters()
        {
            var model = encoder.getParams();
            model.AddRange(decoder.getParams());
            model.AddRange(ReversEncoder.getParams());
            model.Add(Embedding);
            model.Add(Whd);
            model.Add(bd);
            solver.setp(model, learning_rate, regc, clipval);
        }

        #endregion

        #region 预测

        /// <summary>
        /// 重置码器
        /// </summary>
        private void Reset()
        {
            encoder.Reset();
            ReversEncoder.Reset();
            decoder.Reset();
        }

        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="inputSeq">输入分词过的语句</param>
        /// <returns></returns>
        public ExecuteResult< List<string>> Predict(List<string> inputSeq)
        {
            ExecuteResult<List<string>> eresult = new ExecuteResult<List<string>>();
            Reset();
            List<string> result = new List<string>();
            var G2 = new ComputeGraph(false);
            //反序组
            List<string> revseq = inputSeq.ToList();
            revseq.Reverse();
            List<WeightMatrix>  encoded = new List<WeightMatrix>();
            //
            //Console.WriteLine($"keys>{string.Join(",", wordToIndex.Keys.ToArray())}");
            for (int i = 0; i < inputSeq.Count; i++)
            {
                //索引
                if (!wordToIndex.ContainsKey(inputSeq[i]) ) {
                    return eresult.SetFail($"抱歉，未能理解 \"{inputSeq[i]}\"  的含义, 请重新训练我吧!");
                    //return $"抱歉，未能理解 \"{inputSeq[i]}\"  的含义, 请重新训练我吧!".Split(' ').ToList();
                    //return $"I'm sorry, I can't understand \"{inputSeq[i]}\"  the meaning of the word, please you to retrain me!".Split(' ').ToList();
                }

                if ( !wordToIndex.ContainsKey(revseq[i]))
                {
                    return eresult.SetFail($"抱歉，未能理解 \"{revseq[i]}\"  的含义, 请重新训练我吧!");
                    //return $"抱歉，未能理解 \"{inputSeq[i]}\"  的含义, 请重新训练我吧!".Split(' ').ToList();
                    //return $"I'm sorry, I can't understand \"{revseq[i]}\"  the meaning of the word, please you to retrain me!".Split(' ').ToList();
                }

                int ix = wordToIndex[inputSeq[i]].i;
                int ix2 = wordToIndex[revseq[i]].i; 

                var x2 = G2.PeekRow(Embedding, ix);
                var o = encoder.Encode(x2, G2);
                var x3 = G2.PeekRow(Embedding, ix2);
                var eOutput2 = ReversEncoder.Encode(x3, G2);               
                var d = G2.concatColumns(o, eOutput2);                   
                encoded.Add(d);                 
            }
             
            //if (UseDropout)
            //{
            //    for (int i = 0; i < encoded.Weight.Length; i++)
            //    {
            //        encoded.Weight[i] *= 0.2;
            //    }
            //}
            var ix_input = 1;
            while(true)
            {
                var x = G2.PeekRow(Embedding, ix_input);
                var eOutput = decoder.Decode(x,encoded, G2);
                if (UseDropout)
                {
                    for (int i = 0; i < eOutput.Weight.Length; i++)
                    {
                        eOutput.Weight[i] *= 0.2;
                    }
                } 
                var o = G2.add(
                       G2.mul(eOutput, this.Whd), this.bd);
                if (UseDropout)
                {
                    for (int i = 0; i < o.Weight.Length; i++)
                    {
                        o.Weight[i] *= 0.2;
                    }
                }
                var probs = G2.SoftmaxWithCrossEntropy(o);
                var maxv = probs.Weight[0];
                var maxi = 0;
                for (int i = 1; i < probs.Weight.Length; i++)
                {
                    if (probs.Weight[i] > maxv)
                    {
                        maxv = probs.Weight[i];
                        maxi = i;
                    }
                }
                var pred = maxi;

                if (pred == 0) break; // END token predicted, break out
                
                if (result.Count > max_word) { break; } // something is wrong 
                var letter2 = indexToWord[pred].w;
                result.Add(letter2);
                ix_input = pred;
            }

            return eresult.SetData( result).SetOk();
        }
        #endregion

        #region 加载

        /// <summary>
        /// 加载已有模型
        /// </summary>
        public static Seq2Seq Load(string modelname= "Model.lys2s")
        {
            //var tosave = new ModelData();
            BinaryFormatter bf = new BinaryFormatter();//用二进制保存和加载，必须是相同的程序集才可以加载
            FileStream fs = new FileStream(modelname, FileMode.Open, FileAccess.Read);
            var mdl = bf.Deserialize(fs) as ModelData;
            //var mdl = bf.Deserialize(fs) as ModelAttentionData;
            fs.Close();
            fs.Dispose();

            var s2s = new Seq2Seq();
            //s2s.bd =new WeightMatrix( mdl.bd);
            //s2s.Whd =new WeightMatrix( mdl.Whd);
            //s2s.Embedding = new WeightMatrix( mdl.Wil);

            //s2s.decoder =new LinyeeDecoder( mdl.decoder);
            //s2s.encoder =new Encoder( mdl.encoder);
            //s2s.ReversEncoder =new Encoder( mdl.ReversEncoder);

            s2s.bd = (mdl.bd);
            s2s.Whd = (mdl.Whd);
            s2s.Embedding =(mdl.Wil);

            s2s.decoder = (mdl.decoder);
            s2s.encoder = (mdl.encoder);
            s2s.ReversEncoder = (mdl.ReversEncoder);

            s2s.hidden_size = mdl.hidden_sizes;
            s2s.word_size = mdl.letter_size;
            s2s.Depth = mdl.Depth;

            s2s.clipval = mdl.clipval;
            s2s.learning_rate = mdl.learning_rate;
            s2s.max_word = 300;
            s2s.regc = mdl.regc;
            s2s.UseDropout = mdl.UseDropout;

            //s2s.wordToIndex = mdl.wordToIndex;
            s2s.indexToWord = mdl.indexToWord;
            Dictionary<string, WordInfoV1_1> w2i = new Dictionary<string, WordInfoV1_1>();
            foreach(var kv in s2s.indexToWord)
            {
                w2i.Add(kv.Value.w,kv.Value);
            }
            s2s.wordToIndex = w2i;

            s2s.vocab = new List<string>();
            s2s.vocab.AddRange(w2i.Keys);

            s2s.newType = "retrain";

            //s2s.Preprocess();
            //s2s.Save("Model.lys2s");
            return s2s;
        }
        #endregion
    }

    /// <summary>
    /// seq2seq 扩展
    /// </summary>
    public static class Seq2Seq_Ex
    {
        internal static void Preprocess(this Seq2Seq s2s)
        {
            /// <summary>
            /// 输入语句
            /// </summary>
            List<List<string>> input = new List<List<string>>();
            /// <summary>
            /// 输出语句
            /// </summary>
            List<List<string>> output = new List<List<string>>();

            var HumanTextRaw = File.ReadAllLines("human_text.txt");
            var RobotTextRaw = File.ReadAllLines("robot_text.txt");

            for (int i = 0; i < HumanTextRaw.Length; i++)
            {
                HumanTextRaw[i] = RemoveAccentMark(HumanTextRaw[i]);
                RobotTextRaw[i] = RemoveAccentMark(RobotTextRaw[i]);
            }

            var HumanText = new List<string>();
            var RobotText = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                HumanText.Add(HumanTextRaw[i]);
                RobotText.Add(RobotTextRaw[i]);
            }

            for (int i = 0; i < HumanText.Count; i++)
            {
                input.Add(HumanText[i].ToLower().Trim().Split(' ').ToList());
                output.Add(RobotText[i].ToLower().Trim().Split(' ').ToList());
            }

            s2s.OneHotEncoding(input, output);

        }
        /// <summary>
        /// 移除口音标记
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static string RemoveAccentMark(string i)
        {
            return i.Replace('á', 'a')
                    .Replace('é', 'e')
                    .Replace('í', 'i')
                    .Replace('ó', 'o')
                    .Replace('ú', 'u')
                    .Replace('Á', 'A')
                    .Replace('É', 'E')
                    .Replace('Í', 'I')
                    .Replace('Ó', 'O')
                    .Replace('Ú', 'U');
        }

        private static object lockSave = new object();
        private static bool inSave = false;

        /// <summary>
        /// 保存模型
        /// </summary>
        public static void Save(this Seq2Seq s2s, bool hadinback = true, string ModelName=null)
        {
            if (inSave && hadinback) {
                return;
            }

            lock (lockSave)
            {
                inSave = true;
                var mname = ModelName;
                if (string.IsNullOrEmpty(mname)) mname = s2s.ModelName;

                ModelData tosave = new ModelData();
                //tosave.wordToIndex = s2s.wordToIndex;
                tosave.indexToWord = s2s.indexToWord;

                tosave.bd = s2s.bd;
                tosave.clipval = s2s.clipval;
                tosave.decoder = s2s.decoder;
                tosave.Depth = s2s.Depth;
                tosave.encoder = s2s.encoder;
                tosave.hidden_sizes = s2s.hidden_size;
                tosave.learning_rate = s2s.learning_rate;
                tosave.letter_size = s2s.word_size;
                tosave.max_chars_gen = s2s.max_word;
                tosave.regc = s2s.regc;
                tosave.ReversEncoder = s2s.ReversEncoder;
                tosave.UseDropout = s2s.UseDropout;
                tosave.Whd = s2s.Whd;
                tosave.Wil = s2s.Embedding;

                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(mname, FileMode.OpenOrCreate, FileAccess.Write);
                bf.Serialize(fs, tosave);
                fs.Close();
                fs.Dispose();

                inSave = false;
            }
        }

    }

    #region 模型
    /// <summary>
    /// 模型数据
    /// </summary>
    [Serializable]
    public class ModelData
    {
        /// <summary>
        /// 
        /// </summary>
        public int max_chars_gen = 100; // max length of generated sentences 
        /// <summary>
        /// 
        /// </summary>
        public int hidden_sizes;
        /// <summary>
        /// 
        /// </summary>
        public int letter_size;

        /// <summary>
        /// 
        /// </summary>
        // optimization  
        public double regc = 0.000001; // L2 regularization strength
        /// <summary>
        /// 
        /// </summary>
        public double learning_rate = 0.01; // learning rate
        /// <summary>
        /// 
        /// </summary>
        public double clipval = 5.0; // clip gradients at this value
        /// <summary>
        /// 
        /// </summary>
        public bool UseDropout { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Encoder encoder;
        /// <summary>
        /// 
        /// </summary>
        public Encoder ReversEncoder;
        /// <summary>
        /// 
        /// </summary>
        public LinyeeDecoder decoder; 

         /// <summary>
         /// 
         /// </summary>
        public WeightMatrix Wil;
        /// <summary>
        /// 
        /// </summary>
        //Output Layer Weights
        public WeightMatrix Whd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WeightMatrix bd { get; set; }
        public Dictionary<string, WordInfoV1_1> wordToIndex { get; set; }
        public Dictionary<int, WordInfoV1_1> indexToWord { get; set; }
    }
    #endregion
}
