
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MLBot.Extentions
{
    /// <summary>
    /// 编码扩展
    /// </summary>
    [Author("Linyee","2019-01-30")]
    public static class Encode_Extentions
    {
        /// <summary>
        /// 转Url编码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isencode">是否需要编码，便于可选使用，默认进行编码</param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string ToUrlEncode(this string text,bool isencode=true)
        {
            if (!isencode) return text;
            if (string.IsNullOrEmpty(text)) return "";
            return HttpUtility.UrlEncode(text);
        }
        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string ToUrlDecode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return HttpUtility.UrlDecode(text);
        }
        /// <summary>
        /// 转Html编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string ToHtmlEncode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return HttpUtility.HtmlEncode(text);
        }
        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string ToHtmlDecode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return HttpUtility.HtmlDecode(text);
        }
        /// <summary>
        /// 转Base64编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string ToBase64String(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-01-30")]
        public static string FromBase64String(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return Encoding.UTF8.GetString( Convert.FromBase64String(text));
        }
    }
}
