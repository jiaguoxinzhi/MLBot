using MLBot.Attributes;
using MLBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 智能Ai
    /// </summary>
    [Author("Linyee","2019-07-01")]
    public class MLAiBot
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        [Author("Linyee", "2019-07-01")]
        public static readonly MLAiBot Default=new MLAiBot();

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="buffer"></param>
        [Author("Linyee", "2019-07-01")]
        public byte[] Processing(byte[] buffer)
        {
            var b0 = buffer[0];
            if (b0 >= 0x20)
            {
                var text = Encoding.UTF8.GetString(buffer);
                return Encoding.UTF8.GetBytes("正在开发中。。敬请期待！");
            }
            else
            {
                throw new MLBotNoSuperMessageFormat();
            }
        }
    }
}
