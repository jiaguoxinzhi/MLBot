using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Exceptions
{
    /// <summary>
    /// 消息格式暂不支持
    /// </summary>
    [Author("Linyee","2019-07-01")]
    public class MLBotNoSuperMessageFormat : MLBotException
    {
        /// <summary>
        /// 消息格式暂不支持
        /// </summary>
        [Author("Linyee", "2019-07-01")]
        public MLBotNoSuperMessageFormat():base("消息格式暂不支持")
        {
        }
    }
}
