using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    [Author("Linyee", "2019-08-16")]
    public class RebotChatRecord
    {
        /// <summary>
        /// 聊天记录
        /// </summary>
        [Author("Linyee", "2019-08-16")]
        public List<ChatRecordInfo> Records { get; set; } = new List<ChatRecordInfo>();
    }
}
