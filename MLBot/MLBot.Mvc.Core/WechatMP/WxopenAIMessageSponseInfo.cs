using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信对话信息
    /// </summary>
    [Author("Linyee","2019-12-13")]
    public class WxopenAIMessageSponseInfo
    {
        /// <summary>
        /// 技能节点
        /// </summary>
        public int ans_node_id { get; set; }
        /// <summary>
        /// 技能名称
        /// </summary>
        public string ans_node_name { get; set; }
        /// <summary>
        /// 意图名称
        /// </summary>
        public string title { get; set; }

        public string answer { get; set; }
        public string answer_type { get; set; }
        public object bid_stat { get; set; }
        public List<MsgInfo> msg { get; set; }
        public string from_user_name { get; set; }
        public string status { get; set; }
        public string to_user_name { get; set; }
    }

    /// <summary>
    /// 消息详情
    /// </summary>
    [Author("Linyee", "2019-12-13")]
    public class MsgInfo
    {
        public string music_url { get; set; }
        public string pic_url { get; set; }
        public string singer_name { get; set; }
        public string song_name { get; set; }

        public List<ArticleInfo> articles { get; set; }
    }

    /// <summary>
    /// 图文信息
    /// </summary>
    [Author("Linyee", "2019-12-13")]
    public class ArticleInfo
    {
        public string description { get; set; }
        public string pic_url { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
