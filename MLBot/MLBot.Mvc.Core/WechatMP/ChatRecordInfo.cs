using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 聊天记录 信息
    /// </summary>
    [Author("Linyee", "2019-08-16")]
    public class ChatRecordInfo
    {
        /// <summary>
        /// 问题
        /// </summary>
        public string Q { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string A { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// 回答性质
        /// 1正面回答 -1负面反面回答 0不确定回答
        /// </summary>
        public int P { get; set; }


        /// <summary>
        /// 有效期时
        /// 默认永不过期
        /// </summary>
        public DateTime RT { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DT { get; set; }

        /// <summary>
        /// 产生的亲密度值
        /// intimacy
        /// </summary>
        public double IM { get; set; }
    }
}
