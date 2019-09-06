using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信 配置信息
    /// </summary>
    [Author("Linyee", "2019-06-25")]
    public class WechatMpSettings
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public static WechatMpSettings Default;

        /// <summary>
        /// 应用id
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string Token { get; set; }

        /// <summary>
        /// 加密key
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string EncodingAESKey { get; set; }


        /// <summary>
        /// 原始Id
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string MpId { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        [Author("Linyee", "2019-06-25")]
        public string NotifyUrl { get; set; }

    }
}
