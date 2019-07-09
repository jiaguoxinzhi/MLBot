using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MLBot
{
    /// <summary>
    /// 定时任务服务
    /// 只需添加对应的事件
    /// Linyee 2018-09-20
    /// </summary>
    public class TimerService :  IDisposable, ITimerService
    {
        /// <summary>
        /// 默认服务
        /// </summary>
        public static TimerService Default = new TimerService(true);

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            MastExit = true;
        }

        public TimerService()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 是否直接启动服务
        /// </summary>
        /// <param name="IsStart"></param>
        public TimerService(bool IsStart) : this()
        {
            if (IsStart) this.Start();
        }


        #region "事件"


        /// <summary>
        /// 服务启动事件
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> ServiceStarted;

        /// <summary>
        /// 服务结束事件
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> ServiceOvered;

        /// <summary>
        /// 分钟事件1
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute1;

        /// <summary>
        /// 分钟事件5
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute5;

        /// <summary>
        /// 分钟事件10
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute10;

        /// <summary>
        /// 分钟事件30
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute30;

        /// <summary>
        /// 分钟事件60
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute60;

        /// <summary>
        /// 分钟事件100
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Minute100;

        /// <summary>
        /// 整点事件
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> HourTop;

        /// <summary>
        /// 半点事件
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> HourHalf;


        /// <summary>
        /// 秒钟事件1
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second1;

        /// <summary>
        /// 秒钟事件5
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second5;

        /// <summary>
        /// 秒钟事件5 单次
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second5Once;

        /// <summary>
        /// 秒钟事件10
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second10;

        /// <summary>
        /// 秒钟事件30
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second30;

        /// <summary>
        /// 秒钟事件30 单次
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second30Once;

        /// <summary>
        /// 秒钟事件60
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second60;

        /// <summary>
        /// 秒钟事件100
        /// </summary>
        public event EventHandler<LinyeeTimerEventArgs> Second100;
        #endregion

        #region "服务"
        private Timer timer_minu, timer_sec;
        private bool MastExit = false;
        private AutoResetEvent autoReset = new AutoResetEvent(false);
        private AutoResetEvent autoResetSec = new AutoResetEvent(false);


        public void Start()
        {
            //限开
            bool runone = false;
            string name = "TimerService01";
            string currentName = "";
            currentName = name;
            try
            {
                Mutex run = new Mutex(true, name, out runone);//System.Threading.
            }
            catch (Exception ex)
            {
                runone = false;
            }

            if (!runone)
            {
                if (ConfigBase.Default.IsRuntime) LogService.Runtime("===定时服务，已经启动，且已达最大数量===");
                return;
            }


            if (ConfigBase.Default.IsRuntime) LogService.Runtime("==============启动定时服务");

            var th = new System.Threading.Thread(service);
            th.Start();
            timer_minu = new Timer(timer_minu_ticks, null, 60000, 60000);

            var thsec = new System.Threading.Thread(service_sec);
            thsec.Start();
            timer_sec = new Timer(timer_sec_ticks, null, 1000, 1000);
        }

        /// <summary>
        /// 停止计时服务
        /// </summary>
        public void Stop()
        {
            this.MastExit = true;
        }

        /// <summary>
        /// 计时任务
        /// 每分钟执行一次
        /// </summary>
        /// <param name="state"></param>
        private void timer_minu_ticks(object state)
        {
            autoReset.Set();
        }

        /// <summary>
        /// 计时任务
        /// 每分钟执行一次
        /// </summary>
        /// <param name="state"></param>
        private void timer_sec_ticks(object state)
        {
            autoResetSec.Set();
        }

        /// <summary>
        /// 触发事件的服务
        /// </summary>
        private void service()
        {
            long count = 0;
            LinyeeTimerEventArgs eargs = new LinyeeTimerEventArgs()
            {
                timerid = count,
            };
            NoException(() => { ServiceStarted?.Invoke(this, eargs); });
            var dtstart = DateTime.Now;
            while (!MastExit)
            {
                autoReset.WaitOne();

                var dtnow = DateTime.Now;
                count++;
                eargs = new LinyeeTimerEventArgs()
                {
                    timerid = count,
                };
                NoException(() => { Minute1?.Invoke(this, new LinyeeTimerEventArgs()); });
                //if (count % 2 == 0) Minute2?.Invoke(this, new LinyeeTimerEventArgs());
                if (count % 5 == 0) NoException(() => { Minute5?.Invoke(this, eargs); });
                if (count % 10 == 0) NoException(() => { Minute10?.Invoke(this, eargs); });
                if (count % 30 == 0) NoException(() => { Minute30?.Invoke(this, eargs); });
                if (count % 60 == 0) NoException(() => { Minute60?.Invoke(this, eargs); });
                if (count % 100 == 0) NoException(() => { Minute100?.Invoke(this, eargs); });

                if (dtnow.Minute == 0) NoException(() => { HourTop?.Invoke(this, eargs); }); //HourTop?.Invoke(this, new LinyeeTimerEventArgs());
                if (dtnow.Minute == 30) NoException(() => { HourHalf?.Invoke(this, eargs); }); //HourHalf?.Invoke(this, new LinyeeTimerEventArgs());

            }

            var dtover = DateTime.Now;
            NoException(() => { ServiceOvered?.Invoke(this, eargs); });
        }

        /// <summary>
        /// 触发事件的服务
        /// </summary>
        private void service_sec()
        {
            long countsec = 0;
            LinyeeTimerEventArgs eargs = new LinyeeTimerEventArgs()
            {
                timerid = countsec,
                timerType = TimerType.Seconds,
            };
            NoException(() => { ServiceStarted?.Invoke(this, eargs); });
            var dtstart = DateTime.Now;
            while (!MastExit)
            {
                autoResetSec.WaitOne();

                var dtnow = DateTime.Now;
                countsec++;
                eargs = new LinyeeTimerEventArgs()
                {
                    timerid = countsec,
                    timerType = TimerType.Seconds,
                };
                NoException(() => { Second1?.Invoke(this, eargs); });
                //if (countsec % 2 == 0) Second2?.Invoke(this, new LinyeeTimerEventArgs());
                if (countsec % 5 == 0) NoException(() => { Second5?.Invoke(this, eargs); });
                if (countsec % 5 == 0) NoException(() => {
                    Second5Once?.Invoke(this, eargs);
                    RemoveEvent(this, "Second5Once");
                });
                if (countsec % 10 == 0) NoException(() => { Second10?.Invoke(this, eargs); });
                if (countsec % 30 == 0) NoException(() => { Second30?.Invoke(this, eargs); });
                if (countsec % 30 == 0) NoException(() => {
                    Second30Once?.Invoke(this, eargs);
                    RemoveEvent(this, "Second30Once");
                });
                if (countsec % 60 == 0) NoException(() => { Second60?.Invoke(this, eargs); });
                if (countsec % 100 == 0) NoException(() => { Second100?.Invoke(this, eargs); });
            }

            var dtover = DateTime.Now;
            NoException(() => { ServiceOvered?.Invoke(this, eargs); });
        }


        /// <summary>
        /// 移除一个对象指定事件的所有注册的方法
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">当前对象</param>
        // <param name="eventName">事件名</param>
        public static void RemoveEvent<T>(T obj, string eventName)
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
            EventInfo[] eventInfo = obj.GetType().GetEvents(bindingFlags);
            if (eventInfo == null)
            {
                return;
            }


            foreach (EventInfo info in eventInfo)
            {
                if (string.Compare(info.Name, eventName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    FieldInfo fieldInfo = info.DeclaringType.GetField(info.Name, bindingFlags);
                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(obj, null);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 移除一个对象指定事件的所有注册的方法
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">当前对象</param>
        // <param name="eventName">事件名</param>
        public static void RemoveFirstEvent<T>(T obj, string eventName)
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
            EventInfo[] eventInfo = obj.GetType().GetEvents(bindingFlags);
            if (eventInfo == null)
            {
                return;
            }


            foreach (EventInfo info in eventInfo)
            {
                if (string.Compare(info.Name, eventName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    FieldInfo fieldInfo = info.DeclaringType.GetField(info.Name, bindingFlags);
                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(obj, null);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 无错 调用
        /// </summary>
        /// <param name="action"></param>
        public static void NoException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (ConfigBase.Default.IsException) LogService.Exception(ex);
            }
        }
        #endregion

        #region "内部类"
        /// <summary>
        /// 计时服务事件包
        /// Linyee 2018-09-20
        /// </summary>
        public class LinyeeTimerEventArgs : EventArgs
        {
            /// <summary>
            /// 当前计时ID
            /// 即计时服务运行分钟数
            /// </summary>
            public long timerid { get; set; } = 0;
            public TimerType timerType { get; set; } = TimerType.Minute;
        }

        /// <summary>
        /// 计时类别
        /// Linyee 2018-09-22
        /// </summary>
        public enum TimerType
        {
            /// <summary>
            /// 分钟
            /// </summary>
            Minute = 1,
            /// <summary>
            /// 秒钟
            /// </summary>
            Seconds,
        }
        #endregion
    }
}
