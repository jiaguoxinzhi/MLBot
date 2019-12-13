using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信对话开放平台配置
    /// </summary>
    [Author("Linyee", "2019-12-13")]
    public class WxopenAISettings
    {
        /// <summary>
        /// 微信对话配置
        /// </summary>
        [Author("Linyee", "2019-12-13")]
        public static WxopenAISettings Default { get;set; } = new WxopenAISettings();

        /// <summary>
        /// 应用id
        /// </summary>
        [Author("Linyee", "2019-12-13")]
        public string APPID { get; set; }
        /// <summary>
        /// 识别码
        /// </summary>
        [Author("Linyee", "2019-12-13")]
        public string TOKEN { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        [Author("Linyee", "2019-12-13")]
        public string EncodingAESKey { get; set; }

    }

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
