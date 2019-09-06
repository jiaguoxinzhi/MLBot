using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLBot.Extentions;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 签名检测工具
    /// </summary>
    [Author("Linyee", "2019-04-17")]
    public static class CheckSignature
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Author("Linyee", "2019-04-17")]
        public const string Token = "weixin";

        /// <summary>
        /// 签名检测
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static bool Check(string signature, string timestamp, string nonce, string token = null)
        {
            return signature?.Equals(GetSignature(timestamp, nonce, token),StringComparison.OrdinalIgnoreCase)==true;
        }

        /// <summary>
        /// 签名检测
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string GetSignature(string timestamp, string nonce, string token = null)
        {
            token = token ?? "weixin";
            string[] strArray = new string[] { token, timestamp, nonce };
            string[] array = strArray.OrderBy(p=>p).ToArray();
            string str = string.Join("", array);
            string sign = str.ToSHA1();
            return sign;
        }

        /// <summary>
        /// 签名检测
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public static bool Check(string signature, PostModel postModel)
        {
            return Check(signature, postModel.Timestamp, postModel.Nonce, postModel.Token);
        }
    }
}
