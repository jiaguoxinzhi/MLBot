
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MLBot
{
    /// <summary>
    /// 分时更新脚本
    /// 只执行最后一次的脚
    /// </summary>
    [Author("Linyee", "2019-04-11")]
    [Modifier("Linyee", "2019-05-27", "移到MLBot/RootSpace")]
    public class UpdateLastActionService
    {
        /// <summary>
        /// 添加后的启动时间
        /// 最后一次添加延时毫秒数
        /// </summary>
        public long dueTime { get; set; } = 3000;

        /// <summary>
        /// 分时更新脚本
        /// </summary>
        public UpdateLastActionService() { }

        /// <summary>
        /// 最后脚本
        /// </summary>
        internal Action LastActions = null;

        /// <summary>
        /// 最后定时
        /// </summary>
        internal Timer LastTimer = null;

        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <param name="ac"></param>
        public void Add(Action ac)
        {
            LastActions = ac;

            //定时脚本三秒后执行
            if (LastTimer != null)
            {
                LastTimer.Dispose();
                LastTimer = null;
            }

            LastTimer = new Timer((obj) => {
                LastActions?.Invoke();
                LastActions = null;//重置
                LastTimer = null;//重置
            }, null, dueTime, -1);
        }
    }

}

