using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace LinyeeSeq2Seq
{
    /// <summary>
    /// 日志类
    /// </summary>
    //[Author("Linyee", "2018-05-30")]
    public class LogService
    {
        #region IsOSPlatform
        /// <summary>
        /// 是否Linux系统
        /// </summary>
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        /// <summary>
        /// 是否Windows系统
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// 是否OSX系统
        /// </summary>
        public static bool OSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        #endregion

        #region 实时写入 2019-06-21

        /// <summary>
        /// socket10分钟日志
        /// 实时写入
        /// </summary>
        /// <param name="logstr"></param>
        //[Modifier("Linyee", "2019-07-09", "实时写入")]
        public static string Socket10Minute(params string[] logstr)
        {
            return AnyLog("Socket" + DateTime.Now.ToString("HH") + (DateTime.Now.Minute / 10).ToString("D2"), string.Join("\t", logstr));
        }

        /// <summary>
        /// socket10分钟日志
        /// 实时写入
        /// </summary>
        /// <param name="logstr"></param>
        //[Modifier("Linyee", "2019-07-09", "实时写入")]
        public static string WebSocket10Minute(params string[] logstr)
        {
            return AnyLog("WebSocket" + DateTime.Now.ToString("HH") + (DateTime.Now.Minute / 10).ToString("D2"), string.Join("\t", logstr));
        }

        /// <summary>
        /// socket10分钟日志
        /// 实时写入
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="logstr"></param>
        //[Modifier("Linyee", "2019-07-09", "实时写入")]
        public static string WebSocket10MinuteFormat(string fmt, params object[] logstr)
        {
            return AnyLog("WebSocket" + DateTime.Now.ToString("HH") + (DateTime.Now.Minute / 10).ToString("D2"), string.Format(fmt, logstr));
        }

        /// <summary>
        /// socket10分钟日志
        /// 实时写入
        /// </summary>
        /// <param name="logstr"></param>
        //[Modifier("Linyee", "2019-07-09", "实时写入")]
        public static string WebSocketClient10Minute(params string[] logstr)
        {
            return AnyLog("WebSocketClient" + DateTime.Now.ToString("HH") + (DateTime.Now.Minute / 10).ToString("D2"), string.Join("\t", logstr));
        }



        /// <summary>
        /// 签名运行时
        /// 实时写入
        /// </summary>
        /// <param name="messages"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string SignRuntime(params string[] messages)
        {
            return AnyLog("SignRuntime", messages);
        }

        /// <summary>
        /// 请求运行时
        /// 实时写入
        /// </summary>
        /// <param name="messages"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Request(params string[] messages)
        {
            return AnyLog("Request", messages);
        }

        /// <summary>
        /// 运行日志
        /// 实时写入
        /// </summary>
        /// <param name="message"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Runtime(params string[] message)
        {
            return AnyLog("Runtime", message);
        }

        /// <summary>
        /// 调试日志
        /// 实时写入
        /// </summary>
        /// <param name="strs"></param>
        //[Author("Linyee", "2019-05-15")]
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Debug(params string[] strs)
        {
            return AnyLog("Debug", strs);
        }

        /// <summary>
        /// 错误日志
        /// 实时写入
        /// </summary>
        /// <param name="ex"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Error(Exception ex)
        {
            AnyLog("Error", ex.ToString());
            return AnyLog("Runtime", ex.Message);
        }

        /// <summary>
        /// 异常日志
        /// 实时写入
        /// </summary>
        /// <param name="ex"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Exception(string ex)
        {
            return AnyLog("Exception", ex);
        }

        /// <summary>
        /// 异常日志
        /// 实时写入
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exname"></param>
        //[Modifier("Linyee", "2019-06-21", "实时写入")]
        public static string Exception(Exception ex, string exname = "异常")
        {
            Exception(ex.ToString());
            return AnyLog("Runtime", "Runtime", exname + "：" + ex.Message + "（详见Exception记录）");
        }

        /// <summary>
        /// 任意类别日志
        /// 实时写入
        /// </summary>
        /// <param name="logtype"></param>
        /// <param name="logstr"></param>
        //[Author("Linyee", "2019-05-15")]
        //[Modifier("Linyee", "2019-06-21", "放入线程")]
        public static string AnyLog(string logtype, params string[] logstr)
        {
            var logline = DateTime.Now.ToString("HH:mm:ss.fffffff") + "\t" + string.Join("\t", logstr) + "\r\n";
            ThreadLog(logtype, logline);
            return logline;
        }

        /// <summary>
        /// 多线程写入日志
        /// </summary>
        /// <param name="logtype"></param>
        /// <param name="logline"></param>
        //[Modifier("Linyee", "2019-06-28")]
        private static void ThreadLog(string logtype, string logline)
        {
            new Thread(() => {
                //try
                //{
                    var LogPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
                    if (IsLinux) LogPath = LogPath.Replace("\\", "/");//Linux环境处理
                    if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);

                    var logobj = new object();
                    lock (logTypeLock)
                    {
                        if (logTypeLock.ContainsKey(logtype)) logobj = logTypeLock[logtype];
                        else logTypeLock.Add(logtype, logobj);
                    }

                    lock (logobj)
                    {
                        using (var dbfile = File.Open($"{LogPath}{logtype}.txt", FileMode.Append, FileAccess.Write, FileShare.Write))
                        {
                            using (var sw = new StreamWriter(dbfile))
                            {
                                sw.Write(logline);////[Modifier("Linyee","2019-06-28")]
                            }
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    Thread.Sleep(30);
                //    ThreadLog(logtype, logline);
                //}
            }).Start();
        }

        #endregion

        #region 延时写入

        #region "写入方法"

        /// <summary>
        /// 以id分组记录日志
        /// </summary>
        static Dictionary<long, Queue<string>> LogQueDict = new Dictionary<long, Queue<string>>();
        /// <summary>
        /// id与输出文件名的对应关系
        /// </summary>
        static Dictionary<long, string> LogTypeDict = new Dictionary<long, string>();
        static object tmplogdictlock = new object();
        /// <summary>
        /// 分组的运行日志
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="message"></param>
        private static string Runtime(long tid, params string[] message)
        {
            string msg = DateTime.Now.ToString("HH:mm:ss.fffffff") + "\t" + string.Join("\t", message);
            lock (tmplogdictlock)
            {
                if (LogQueDict.ContainsKey(tid))
                {
                    LogQueDict[tid].Enqueue(msg);
                }
                else
                {
                    var que = new Queue<string>();
                    que.Enqueue(msg);
                    LogQueDict.Add(tid, que);
                }
            }
            return msg;
        }

        /// <summary>
        /// 运行日志 结束分组并写入到文件
        /// 此前通过调用AnyLog(tid,... 的方式来使用任意类别日志
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="message"></param>
        /// <param name="isend"></param>
        public static void Runtime(long tid, string message, bool isend)
        {
            Runtime(tid, message);
            if (isend)
            {
                lock (tmplogdictlock)
                {
                    if (LogQueDict.ContainsKey(tid))
                    {
                        var que = LogQueDict[tid];
                        var msg = string.Join("\r\n", que);
                        LogQueDict.Remove(tid);
                        var logfile = "Runtime.txt";
                        //判断文件名
                        if (LogTypeDict.ContainsKey(tid))
                        {
                            logfile = LogTypeDict[tid];
                            LogTypeDict.Remove(tid);
                        }
                        Runtime(msg);
                    }
                }
            }
        }



        /// <summary>
        /// 分组的任意类别日志
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="logtype"></param>
        /// <param name="logstr"></param>
        public static string AnyLog(long tid, string logtype, params string[] logstr)
        {
            var line = Runtime(tid, logstr);
            if (!LogTypeDict.ContainsKey(tid))
            {
                lock (tmplogdictlock)
                {
                    LogTypeDict.Add(tid, logtype + ".txt");
                }
            }
            return line;
        }

        /// <summary>
        /// 锁定对象 不区分大小写
        /// </summary>
        private static Dictionary<string, object> logTypeLock = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region "写入主体"
        private static object RequestFileLock = new object();
        private static object QueueLock = new object();

        /// <summary>
        /// 多线程日志
        /// </summary>
        /// <param name="state"></param>
        public static void WriteLog(object state)
        {
            var log = state?.ToString();
            if (!string.IsNullOrWhiteSpace(log))
            {
                WriteLog("Log.txt", log);
            }
        }

        /// <summary>
        /// 多线程日志
        /// </summary>
        /// <param name="state"></param>
        public static void WriteLogRequest(object state)
        {
            var log = state?.ToString();
            if (!string.IsNullOrWhiteSpace(log))
            {
                WriteLog("Request.txt", log);
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logtype"></param>
        /// <param name="logs"></param>
        /// <param name="lockobj"></param>
        private static void WriteLog(string logtype, List<string> logs, object lockobj)
        {
            var logstr = string.Join("\r\n", logs) + "\r\n";
            logs.Clear();
            WriteLog(logtype + ".txt", logstr, lockobj);
        }

        private static object TimerListLock = new object();
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        private static List<timerInfo> TimerList = new List<timerInfo>();
        private static timerInfo LastTimer = null;
        private class timerInfo : IDisposable
        {
            public timerInfo(DateTime now, Timer timer)
            {
                this.addTime = now;
                this.timer = timer;
            }

            public DateTime addTime { get; set; }
            public Timer timer { get; set; }

            public void Dispose()
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
            }
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logfile"></param>
        /// <param name="log"></param>
        /// <param name="lockobj"></param>
        private static void WriteLog(string logfile, string log, object lockobj = null)
        {
            var LogPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Log\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
            if (IsLinux) LogPath = LogPath.Replace("\\", "/");//Linux环境处理
            if (lockobj == null) lockobj = RequestFileLock;
            if (!Directory.Exists(LogPath))
            {
                //throw new Exception("目录不存在"+ LogPath);
                Directory.CreateDirectory(LogPath);
            }

            try
            {
                lock (QueueLock)
                {
                    lock (lockobj)
                    {
                        //压入缓存的日志
                        GetValue(logs, logfile).Enqueue(log);
                    }
                }
                var dt = DateTime.Now;
                if (LastTimer != null) LastTimer.Dispose();

                if (IsSoonWrite)
                    LastTimer = new timerInfo(dt, new Timer(new TimerCallback(WriteLogTimer), dt, 1000, -1));
                else
                    WriteLogTimer(dt);
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }

        /// <summary>
        /// 定时写入日志
        /// </summary>
        /// <param name="state"></param>
        private static void WriteLogTimer(object state)
        {
            var LogPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
            if (IsLinux) LogPath = LogPath.Replace("\\", "/");//Linux环境处理
            if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);

            var dt = (DateTime)state;
            try
            {
                lock (TimerListLock)
                {
                    var rlist = TimerList.Where(p => p.addTime <= dt).ToList();
                    foreach (var item in rlist)
                    {
                        TimerList.Remove(item);
                    }
                }
            }
            catch //(Exception ex)
            {
            }

            try
            {
                LogWriteLock.EnterWriteLock();
                string[] keys = new string[0];
                lock (QueueLock)
                {
                    keys = logs.Keys.ToArray();
                }
                foreach (var logfile in keys)
                {
                    var qu = GetValue(logs, logfile);
                    if (qu.Count < 1) continue;
                    string log = "";
                    lock (QueueLock)
                    {
                        log = string.Join("\r\n", qu.ToList());
                        qu.Clear();
                        File.AppendAllText(LogPath + DateTime.Now.ToString("yyyyMMdd_HH") + logfile, log + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        private static Dictionary<string, Queue<string>> logs = new Dictionary<string, Queue<string>>();
        /// <summary>
        /// 一会儿才输出
        /// </summary>
        public static bool IsSoonWrite = true;

        /// <summary>
        /// 获取指定日志队列
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Queue<string> GetValue(Dictionary<string, Queue<string>> logs, string key)
        {
            var kkey = key.ToLower();
            if (!logs.ContainsKey(kkey))
            {
                logs.Add(kkey, new Queue<string>(100000));
            }

            return logs[kkey];
        }
        #endregion

        #endregion
    }
}
