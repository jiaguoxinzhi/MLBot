using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using LinyeeSeq2Seq;
using MLBot.NLTK.Analyzers;

namespace LinyeeSeq2SeqTest
{
    class Program
    {
        static int times = 300;
        static int dim = 32;
        static int hdim = 32;
        static int dep = 1;
        static Seq2Seq S2S;
        static Thread MainThread;
        static Thread ReadThread;
        /// <summary>
        /// 输入语句
        /// </summary>
        static List<List<string>> input = new List<List<string>>();
        /// <summary>
        /// 输出语句
        /// </summary>
        static List<List<string>> output = new List<List<string>>();


        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                int.TryParse(args[0], out times);
            }
            if (args.Length > 1)
            {
                int.TryParse(args[1], out dim);
            }
            if (args.Length > 2)
            {
                int.TryParse(args[2], out hdim);
            }

            //Random r = new Random(5);
            try
            {
                S2S= Seq2Seq.Load();
            }
            catch (Exception ex)
            {
                var msg=LogService.Exception(ex);
                Console.WriteLine(msg);
                S2S = null;
            }

            if (S2S == null)
            {
                Preprocess();
                S2S = new Seq2Seq(dim, hdim, dep, input, output, true);
            }

            //添加事件
            int c = 0;
            var lastdone = DateTime.Now;
            S2S.IterationDone += (a1, a2) =>
            {
                CostEventArg ep = a2 as CostEventArg;
                var dtnow = DateTime.Now;
                if ((dtnow - lastdone).TotalSeconds >= 10)//c % 300 == 0 || 
                {
                    lastdone = dtnow;
                    Console.WriteLine($"训练次数 {ep.Iteration + 1}/{times} 完成行数 {c}");
                    S2S.Save();
                }
                c++;
            };
            S2S.EpochDone += (o, e) => {
                CostEventArg ep = e as CostEventArg;
                Console.WriteLine($"训练批次 {ep.Iteration + 1}/{times} 完成");
            };
            S2S.TrainDone += (o, e) => {
                CostEventArg ep = e as CostEventArg;
                Console.WriteLine($"训练任务 {times}次 完成");
                S2S.Save(false);
            };
            S2S.TrainStart += (o, e) => {
                CostEventArg ep = e as CostEventArg;
                Console.WriteLine($"训练开始 将训{times}次");
            };

            //创建模式 不是加载模式
            if (S2S.newType == "new")
            {
                MainThread = new Thread(new ThreadStart(Train));
                MainThread.Start();
            }

            ReadThread = new Thread(new ThreadStart(ReadingConsole));
            ReadThread.Start();

            //System.Threading.AutoResetEvent resetEvent = new AutoResetEvent(false);
            //resetEvent.WaitOne();
            //Console.ReadKey();
        }

        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="manfile"></param>
        /// <param name="botfile"></param>
        static void Preprocess(string manfile= "human_cn_ws.txt",string botfile= "robot_cn_ws.txt")
        {
            Console.WriteLine($"文件预处理：{manfile} {botfile}");

            input.Clear();
            output.Clear();

            var HumanTextRaw = File.ReadAllLines(manfile);
            var RobotTextRaw = File.ReadAllLines(botfile);

            //if (HumanTextRaw.Length != RobotTextRaw.Length) throw new Exception("数据异常，问答数据个数不一致！");

            for (int i = 0; i < HumanTextRaw.Length; i++)
            {
                HumanTextRaw[i] = RemoveAccentMark(HumanTextRaw[i]);
                RobotTextRaw[i] = RemoveAccentMark(RobotTextRaw[i]);
            }

            var HumanText = new List<string>();
            var RobotText = new List<string>();
            for (int i = 0; i < HumanTextRaw.Length; i++)
            {
                HumanText.Add(HumanTextRaw[i]);
                RobotText.Add(RobotTextRaw[i]);
            }

            for (int i = 0; i < HumanText.Count; i++)
            {
                input.Add(HumanText[i].ToLower().Trim().Split(' ').ToList());
                output.Add(RobotText[i].ToLower().Trim().Split(' ').ToList());
            }
            Console.WriteLine($"文件预处理完成：{input.Count} {input.Count}");
        }

        /// <summary>
        /// 训练
        /// </summary>
        static void Train()
        {
            S2S.Train(times);
        }

        /// <summary>
        /// 读控制台
        /// </summary>
        static void ReadingConsole()
        {

            FileStream fs = null;
            StreamReader sr = null;
            bool readanykey = false;
            string exitmsg = null;
            bool echo = true;

            while (true)
            {
                Console.Write("Q:>");
                var Line = RemoveAccentMark(Console.ReadLine().ToLower());
                if (Line == "") continue;
                if (Line == null) break;


                //人机对话
                try
                {
                    var inwords = LinyeeNLAnalyzer.Default.WordAnalyJieba(Line).Data;
                    Console.WriteLine($"分词结果>{string.Join(" ", inwords)}");
                    var pred = S2S.Predict(inwords).Data;
                    //Console.WriteLine($"<< {Line}");

                    Console.Write("A:>");
                    for (int i = 0; i < pred.Count; i++)
                    {
                        Console.Write(pred[i] + " ");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    LogService.Exception(ex);
                    Console.WriteLine("I'm Sorry, unable to process your request!");
                    Console.WriteLine("我错了，未能理解您的请求!");
                }
            }
        }

        /// <summary>
        /// 移除口音标记
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        static string RemoveAccentMark(string i)
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
    }
}
