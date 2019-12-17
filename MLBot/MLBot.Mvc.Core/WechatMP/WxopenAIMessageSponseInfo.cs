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
        /// <summary>
        /// 意图节点
        /// </summary>
        public int ans_node_id { get; set; }
        /// <summary>
        /// 意图名称
        /// </summary>
        public string ans_node_name { get; set; }
        public string answer { get; set; }
        public string answer_type { get; set; }
        public object bid_stat { get; set; }
        public List<MsgInfo> msg { get; set; }
        public string from_user_name { get; set; }
        public string status { get; set; }
        public string to_user_name { get; set; }
    }

    public class MsgInfo
    {
        public string music_url { get; set; }
        public string pic_url { get; set; }
        public string singer_name { get; set; }
        public string song_name { get; set; }

        public List<ArticleInfo> articles { get; set; }
    }

    public class ArticleInfo
    {
        public string description { get; set; }
        public string pic_url { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
