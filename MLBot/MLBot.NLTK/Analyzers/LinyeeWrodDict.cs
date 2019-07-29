using MLBot.Extentions;
using MLBot.NLTK.Enums;
using MLBot.NLTK.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLBot.NLTK.Analyzers
{
    /// <summary>
    /// 词典
    /// </summary>
    [Author("Linyee", "2019-07-22")]
    internal class LinyeeWrodDict : Dictionary<string, WordInfoOnce>, IDisposable
    {
        #region 重写词典

        /// <summary>
        /// 添加元素
        /// 如果是新增 增加保存次数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [Author("Linyee", "2019-07-27")]
        public WordAnalyResult AddNew(string key, WordInfoOnce value)
        {
            WordAnalyResult result = new WordAnalyResult();
            if (ContainsKey(key))
            {
                return result.SetFail("已经存在") ;//已有
            }
            else
            {
                base.Add(key, value);
                savetimes++;
                return result.SetOk();
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [Author("Linyee", "2019-07-26")]
        public new void Add(string key, WordInfoOnce value)
        {
            if (ContainsKey(key))
            {
                //$"{name}已存在键：{key}。".WriteWarningLine();
                return;//已有
            }

            if (name == "默认")
            {
                var w = value;
                if (w.p?.StartsWith("i", StringComparison.OrdinalIgnoreCase) == true) Idioms.Add(w.w, w);//成语词典
                else if (w.p?.StartsWith("nr", StringComparison.OrdinalIgnoreCase) == true) PersonNames.Add(w.w, w);//人名词典
                else if (w.p?.StartsWith("ns", StringComparison.OrdinalIgnoreCase) == true) PlaceNames.Add(w.w, w);//地 
                else
                {
                    TempWords.Add(key,value);
                }
            }
            else
            {
                base.Add(key, value);
            }
        }

        /// <summary>
        /// 索引
        /// 获取时不存在时 null
        /// 设置时不存在时 加到临时库
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-26")]
        public new WordInfoOnce this[string key]
        {
            get {
                if (base.ContainsKey(key)) return base[key];
                foreach (var dict in subdicts)
                {
                    if (dict?.ContainsKey(key) == true) return dict[key];
                }
                return null;
            }
            set {
                if (base.ContainsKey(key))
                {
                    base[key] = value;
                    return;
                }
                foreach (var dict in subdicts)
                {
                    if (dict?.ContainsKey(key) == true)
                    {
                        dict[key] = value;
                        return;
                    }
                }

                //不存在则加到临时库
                if (TempWords.ContainsKey(key)) TempWords[key] = value;
                else TempWords.Add(key, value);
            }
        }

        /// <summary>
        /// 添加子字典
        /// </summary>
        /// <param name="dict">子词典</param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-26")]
        public bool Add(LinyeeWrodDict dict)
        {
            if (!subdicts.Contains(dict))
            {
                subdicts.Add(dict);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-26")]
        public new bool ContainsKey(string key)
        {
            if (base.ContainsKey(key)) return true;
            foreach (var dict in subdicts)
            {
                if (dict?.ContainsKey(key) == true) return true;
            }
            return false;
        }

        #endregion

        #region 常用词典
        //不要加 static构造 会卡死。

        /// <summary>
        /// 初始化
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        private static void init(string dictname = "默认")
        {
            //Console.WriteLine($"{dictname}开始初始化");

            Task.WaitAll(Task.Run(() =>
            {
                if (Idioms == null) Idioms = new LinyeeWrodDict("成语", "idioms.db");
            }), Task.Run(() =>
            {
                if (PersonNames == null) PersonNames = new LinyeeWrodDict("人名", "nr.db");
            }), Task.Run(() =>
            {
                if (PlaceNames == null) PlaceNames = new LinyeeWrodDict("地名", "ns.db");
            })
            );

            Task.WaitAll(Task.Run(() =>
            {
                if (TempWords == null) TempWords = new LinyeeWrodDict("临时", "temp.db");
            }), Task.Run(() =>
            {
                if (WordsDict == null) WordsDict = new LinyeeWrodDict("词典", "words.db");
            }));

            //Console.WriteLine($"{dictname}初始化完成");
        }

        /// <summary>
        /// 成语词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal static LinyeeWrodDict Idioms { get; private set; }

        /// <summary>
        /// 人名词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal static LinyeeWrodDict PersonNames { get; private set; }

        /// <summary>
        /// 地名词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal static LinyeeWrodDict PlaceNames { get; private set; }

        /// <summary>
        /// 临时词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal static LinyeeWrodDict TempWords { get; private set; }

        /// <summary>
        /// 成语词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal static LinyeeWrodDict WordsDict { get; private set; }


        /// <summary>
        /// 默认词典
        /// </summary>
        [Author("Linyee", "2019-07-22")]
        internal static LinyeeWrodDict Default {
            get {
                if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrames().Take(5).Select(f =>
                {
                    var m = f.GetMethod();
                    return $"{m.Module}.{m.ToString()}";
                }).ToJsonString());

                if (_Default == null) _Default = new LinyeeWrodDict();
                return _Default;
            }
        }
        private static LinyeeWrodDict _Default;
        #endregion

        #region 构造 初始化 析构
        /// <summary>
        /// 释放
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public virtual void Dispose()
        {
            OpenSave();
            timer.Dispose();
            timer = null;
            this.Clear();
        }

        /// <summary>
        /// 词典目录
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        protected static string jsondir;

        /// <summary>
        /// 子词典
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        protected List<LinyeeWrodDict> subdicts = new List<LinyeeWrodDict>();

        /// <summary>
        /// 词典目录
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        protected string dbfile;


        [Author("Linyee", "2019-07-25")]
        protected Timer timer;

        /// <summary>
        /// 名称
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        protected string name = "默认";

        /// <summary>
        /// 指定词典
        /// 不包含默认词典
        /// </summary>
        /// <param name="dictname">指定词典名称</param>
        /// <param name="dbnmae">指定加载的文件名</param>
        [Author("Linyee", "2019-07-26")]
        public LinyeeWrodDict(string dictname, string dbnmae):base()
        {
            //Console.WriteLine($"{dictname}开始构造");
            //LogService.Debug($"{dictname}开始构造");

            //名称设置
            if (!string.IsNullOrEmpty(dictname)) name = dictname;

            //目录检测
            if (string.IsNullOrEmpty(jsondir))
            {
                jsondir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Dictionarys");
                if (!Directory.Exists(jsondir)) Directory.CreateDirectory(jsondir);
            }

            //词典文件名
            dbfile = Path.Combine(jsondir, dbnmae);
            //Console.WriteLine($"{name}文件名：{dbfile}");

            //加载词典
            try
            {
                ReadThread(dbfile);
            }
            catch(Exception ex)
            {
                ex.ToString().WriteErrorLine();
            }

            //每隔五分钟开关一次
            timer = new Timer((obj) =>
            {
                OpenSaveCloseSave();
            }, null, 300000, 300000);
            //Console.WriteLine($"{dictname}构造结束");
            //LogService.Debug($"{dictname}构造结束");
        }

        /// <summary>
        /// 默认词典
        /// 包含几个常用的子词典 成语、人名、地名、临时、词典
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        public LinyeeWrodDict()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            this.name = "默认";

            //目录检测
            if (string.IsNullOrEmpty(jsondir))
            {
                jsondir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Dictionarys");
                if (!Directory.Exists(jsondir)) Directory.CreateDirectory(jsondir);
            }
            dbfile = Path.Combine(jsondir, "default.db");

            init(name);
            //子词典
            subdicts.AddRange(new LinyeeWrodDict[] { WordsDict, Idioms, PersonNames, PlaceNames, TempWords });
            //LinyeeCharDict.Default.OpenSaveCloseSave();

        }

        #endregion

        #region 读取

        /// <summary>
        /// 线程读取
        /// </summary>
        [Author("Linyee", "2019-07-22")]
        internal virtual void ReadThread(string file)
        {
            lock (file)
            {
                //读取现有词典
                using (var f = File.Open(file, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                {
                    using (var sr = new StreamReader(f, Encoding.UTF8))
                    {
                        StreamReaderAll(sr);
                    }
                }
            }

            OpenSaveCloseSave();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        [Author("Linyee", "2019-07-22")]
        private void StreamReaderAll(StreamReader sr)
        {
            //$"{name}数据量比较大{sr.BaseStream.Length}BS，初始化数据需要一些时间，请耐心等待".WriteInfoLine(ConsoleColor.Cyan);
            long lineCount = 0;
            var dt0 = DateTime.Now;
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line == "") continue;//空行
                lineCount++;

                WordInfoOnce w = null;
                string[] keys = new string[0];

                if (line.StartsWith("{"))
                {
                    w = JsonConvert.DeserializeObject<WordInfoOnce>(line);
                }
                else
                {
                    if (line.IndexOf('\t') > 0) keys = line.Split('\t');
                    else keys = line.Split(' ');

                    if (keys.Length >= 8)
                    {
                        //sw.WriteLine($"{w.w}\t{w.f}\t{w.p}\t{w.l}\t{w.o}\t{w.py}\t{w.isp}\t{w.pt}");
                        w = new WordInfoOnce()
                        {
                            w = keys[0],
                            f = string.IsNullOrEmpty(keys[1]) ? 0 : double.Parse(keys[1]),
                            p = keys[2],
                            l = keys[3],
                            o = string.IsNullOrEmpty(keys[4]) ? null : (byte?)byte.Parse(keys[4]),
                            py = keys[5],
                            isp = string.IsNullOrEmpty(keys[4]) ? null : (bool?)bool.Parse(keys[6]),
                            pt = keys[7],
                        };
                    }
                    else if (keys.Length > 3)
                    {
                        w = new WordInfoOnce()
                        {
                            w = string.Join(" ", keys.Take(keys.Length - 2)),
                            p = keys[keys.Length - 1],
                        };
                        double d = 0;
                        var b = double.TryParse(keys[keys.Length - 2], out d);
                        if (!b) b = double.TryParse(keys[1], out d);
                        w.f = d;
                    }
                    else if (keys.Length == 3)
                    {
                        w = new WordInfoOnce()
                        {
                            w = keys[0],
                            p = keys[2],
                        };
                        double d = 0;
                        var b = double.TryParse(keys[1], out d);
                        w.f = d;
                    }
                    else if (keys.Length == 2)
                    {
                        w = new WordInfoOnce()
                        {
                            w = keys[0],
                        };
                        double d = 0;
                        var b = double.TryParse(keys[1], out d);
                        w.f = d;
                    }
                    else
                    {
                        w = new WordInfoOnce()
                        {
                            w = keys[0],
                        };
                    }
                }

                //分文件
                if (name == "词典" || name == "临时")
                {
                    //Console.Write($"{name}.");
                    if (name != ("成语") && w.p?.StartsWith("i", StringComparison.OrdinalIgnoreCase) == true) Idioms.AddNew(w.w, w);//成语词典
                    else if (name != ("人名") && w.p?.StartsWith("nr", StringComparison.OrdinalIgnoreCase) == true) PersonNames.AddNew(w.w, w);//人名词典
                    else if (name != ("地名") && w.p?.StartsWith("ns", StringComparison.OrdinalIgnoreCase) == true) PlaceNames.AddNew(w.w, w);//地名词典
                    else//词典或临时
                    {
                        if (!base.ContainsKey(w.w)) base.Add(w.w, w);
                        else if (!base.ContainsKey(w.Key))
                        {
                            var item = base[w.w];
                            if (item.p != w.p) base.Add(w.Key, w.SetKeyType(KeyType.Key));
                        }
                    }
                }
                else
                {
                    if (!base.ContainsKey(w.w)) base.Add(w.w, w);
                    else if (!base.ContainsKey(w.Key))
                    {
                        var item = base[w.w];
                        if (item.p != w.p) base.Add(w.Key, w.SetKeyType(KeyType.Key));
                    }
                }

                //3秒提醒
                if (lineCount % 1000 == 0 && DateTime.Now.AddSeconds(-3) > dt0)
                {
                    dt0 = DateTime.Now;
                    $"{name}已完成{lineCount}".WriteInfoLine(ConsoleColor.Cyan);
                }
            }

            if (this.Count != lineCount) savetimes++;//保存
        }

        #endregion

        #region 保存

        bool closesave = false;
        /// <summary>
        /// 保存次数
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        long savetimes = 0;
        /// <summary>
        /// 关闭保存
        /// 打开时如果有未存操作则自动保存
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        internal void CloseSave()
        {
            closesave = true;
            savetimes = 0;
        }
        /// <summary>
        /// 打开时如果有未存操作则自动保存
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal void OpenSave()
        {
            closesave = false;
            if (savetimes > 0)
            {
                SaveThread();
            }
            savetimes = 0;
        }

        /// <summary>
        /// 先打开，再关闭
        /// </summary>
        [Author("Linyee", "2019-07-26")]
        internal void OpenSaveCloseSave()
        {
            OpenSave();
            CloseSave();

            //保存子词典
            foreach (var sub in subdicts)
            {
                sub.SaveThread();
                sub.OpenSaveCloseSave();
            }

            //LinyeeCharDict.Default.OpenSaveCloseSave();
        }

        private bool isinsave = false;
        /// <summary>
        /// 线程保存 全新写入
        /// 如果有子词典会自动保存子词典
        /// </summary>
        [Author("Linyee", "2019-07-22")]
        internal void SaveThread()
        {
            //禁用保存时
            if (closesave)
            {
                savetimes++;
                return;
            }

            //有记录才保存
            if (this.Count > 0)
            {
                if (isinsave)
                {
                    $"{name} 已经在保存了".WriteInfoLine(ConsoleColor.Yellow);
                    return;//已经在保存了
                }
                new Thread(()=> {
                    lock (dbfile)
                    {
                        isinsave = true;
                        //var jss = new JsonSerializerSettings();
                        //jss.NullValueHandling = NullValueHandling.Ignore;//忽略空值 好像很慢
                        var linecount = 0;
                        var dt0 = DateTime.Now;
                        using (var f = File.Open(dbfile + ".new", FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            using (var sw = new StreamWriter(f, Encoding.UTF8, 1048576 * 16))//65536
                            {
                                foreach (var w in this.Values.OrderBy(v => v.w).ThenBy(v => v.p))
                                {
                                    sw.WriteLine($"{w.w}\t{w.f}\t{w.p}\t{w.l}\t{w.o}\t{w.py}\t{w.isp}\t{w.pt}");
                                    //sw.WriteLine(w.ToJsonString(NullValueHandling.Ignore));
                                    linecount++;
                                    if (linecount % 100000 == 0 && DateTime.Now.AddSeconds(-1) > dt0)
                                    {
                                        $"{name}已保存{linecount}".WriteInfoLine(ConsoleColor.Cyan);
                                        dt0 = DateTime.Now;
                                    }
                                }
                            }
                        }

                        //复制模式
                        File.Delete(dbfile);
                        Thread.Sleep(100);
                        File.Copy(dbfile + ".new", dbfile);

                        $"{name} 更新完成".WriteInfoLine(ConsoleColor.Green);
                        isinsave = false;
                    }
                }).Start();
            }

            //保存子词典
            foreach (var sub in subdicts)
            {
                sub.SaveThread();
            }
        }

        #endregion

        #region 分词

        /// <summary>
        /// 析词 多词
        /// </summary>
        /// <param name="charDict"></param>
        /// <param name="raw"></param>
        /// <param name="fi"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        internal virtual List<WordInfoOnce> WordAnaly( ReadOnlySpan<char> raw, int fi)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            List<WordInfoOnce> list = new List<WordInfoOnce>();
            var maxlen = raw.Length - fi;
            if (maxlen > 1)
            {
                #region 词典分词
                var arr = new WordInfoOnce[maxlen];
                for(var fj=0;fj< maxlen; fj++)
                {
                    var ch = raw[fi + fj];
                    //charDict.CharAnaly(ch);//补充词典

                    var word = WordsAnaly(raw, fi+fj);

                    //语法调整

                    //词频调整
                    if (fj > 0 && word != null)
                    {
                        if (word.w.Length > arr[fj - 1].w.Length)
                        {
                            arr[fj - 1] = null;
                        }
                        else if (word.w.Length == arr[fj - 1].w.Length && word.f > arr[fj - 1].f)
                        {
                            arr[fj - 1] = null;
                        }
                    }
                    arr[fj] = word;
                }

                //结果处理
                string tword = "";
                for (var fj = 0; fj < maxlen;)
                {
                    var cword = arr[fj];
                    var nword = (fj + 1 < maxlen)?arr[fj + 1]:null;
                    if (cword == null)
                    {
                        tword += raw[fi + fj];
                        fj++;
                        continue;
                    }
                    else
                    {
                        if (tword.Length == 1)//单字成词
                        {
                            list.Add(new WordInfoOnce(tword[0]));
                            //list.Add(charDict.CharAnaly(tword[0]));
                            tword = "";
                        }
                        else if (tword.Length > 1)//多字新词
                        {
                            var maxkey = tword;
                            var info = new WordInfoOnce(maxkey);
                            list.Add(info);
                            tword = "";
                            //SaveThread();
                        }

                            list.Add(cword);
                            tword = "";
                            fj += cword.w.Length;
                            continue;
                    }
                }

                //最后不成词的字
                if (!string.IsNullOrEmpty(tword))
                {
                    list.Add(new WordInfoOnce(tword));
                    tword = "";
                }

                return list;
                #endregion
            }
            else
            {
                #region 词典分词
                //list.Add( charDict.CharAnaly(raw[fi]));
                list.Add(new WordInfoOnce(raw[fi]));
                return list;
                #endregion
            }

        }


        /// <summary>
        /// 析词 只解析出词不要字
        /// 不存在则为null
        /// </summary>
        /// <param name="charDict"></param>
        /// <param name="raw"></param>
        /// <param name="fi"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        internal virtual WordInfoOnce WordsAnaly( ReadOnlySpan<char> raw, int fi)
        {
            #region 词典分词
            var maxlen = raw.Length - fi;
            if (maxlen > 1)
            {
                var maxkey = raw.Slice(fi).ToString();
                //快速响应 精确大小写
                for (var fl = maxlen; fl > 0; fl--)
                {
                    var word = raw.Slice(fi, fl).ToString();
                    if (this.ContainsKey(word)) return this[word];
                }
                //快速响应 忽略大小写
                for (var fl = maxlen; fl > 0; fl--)
                {
                    var word = raw.Slice(fi, fl).ToString();
                    var key = this.Keys.FirstOrDefault(k => k.Equals(word, StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(key)) return this[key];
                }
                //慢速响应
                for (var fl = maxlen; fl > 0; fl--)
                {
                    var word = raw.Slice(fi, fl).ToString();
                    if(ContainsWord(word)) return this.Values.FirstOrDefault(w => w.IndexKeyType == KeyType.Key && w.w.Equals(word, StringComparison.OrdinalIgnoreCase));
                }
            }
            return null;
            #endregion
        }

        /// <summary>
        /// 不区分大小写 判断是否包含指定词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-25")]
        private bool ContainsWord(string word)
        {
            return this.Values.Any(w =>w.w.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
