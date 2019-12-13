using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 
    /// </summary>
    public class WxopenAIMessageSponseInfo
    {
        public string answer { get; set; }
        public string answer_type { get; set; }
        public object bid_stat { get; set; }
        public List<object> msg { get; set; }
        public string from_user_name { get; set; }
        public string status { get; set; }
        public string to_user_name { get; set; }
    }
}
