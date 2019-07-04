
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Exceptions
{
    /// <summary>
    /// 异常基类
    /// </summary>

    [Author("Linyee", "2019-07-01")]
    public class MLBotException : Exception
    {
        /// <summary>
        /// 基础异常 未细分
        /// </summary>
        [Author("Linyee", "2019-07-01")]
        public MLBotException(string msg):base(msg)
        {
        }
    }
}
