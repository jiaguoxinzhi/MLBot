using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 信息
    /// </summary>
    [Author("Linyee", "2019-06-21")]
    public class WxSponseMsg
    {

        /// <summary>
        /// 错误代码 0无错
        /// </summary>
        [Author("Linyee", "2019-06-21")]
        public int errcode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Author("Linyee", "2019-06-21")]
        public string errmsg { get; set; }
        /// <summary>
        /// 信息Id
        /// </summary>
        [Author("Linyee", "2019-06-21")]
        public long msgid { get; set; }
    }


    /// <summary>
    /// 微信授权响应信息
    /// </summary>
    [Author("Linyee", "2019-06-20")]
    public class WxAccessToken : WxSponseMsg
    {
        /// <summary>
        /// 令牌
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public string access_token { get; set; }
        /// <summary>
        /// 有效时长
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public int expires_in { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public DateTimeOffset token_time { get; set; } = DateTimeOffset.Now;
        /// <summary>
        /// 过期时间
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public DateTimeOffset expires_time => token_time.AddSeconds(expires_in);

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public string refresh_token { get; set; }

        /// <summary>
        /// 开放id
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public string openid { get; set; }

        /// <summary>
        /// 开放作用域
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public string scope { get; set; }

        /// <summary>
        /// 令牌缓存
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static Dictionary<string, WxAccessToken> token = new Dictionary<string, WxAccessToken>();

        /// <summary>
        /// web令牌缓存
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static Dictionary<string, WxAccessToken> webtoken = new Dictionary<string, WxAccessToken>();

        /// <summary>
        /// 获取令牌 有效时间内进行缓存
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public static WxAccessToken CreateToken(string appId, string appSecret)
        {
            "使用自有微信令牌获取功能".WriteWarningLine();
            if (token.ContainsKey(appId))
            {
                var tres = token[appId];
                if (tres.expires_time > DateTimeOffset.Now) return tres;
            }

            var wc = new WebClientLy();
            var url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
            $"访问{url}".WriteWarningLine();
            var accessTokenJson = wc.DownloadString(url);
            $"结果{accessTokenJson}".WriteWarningLine();
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<WxAccessToken>(accessTokenJson);
            lock (token)
            {
                if (token.ContainsKey(appId)) token.Remove(appId);
                token.Add(appId, res);
            }
            return res;
        }


        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public static WxAccessToken GetToken(string appid, string appsecret)
        {
            var ckey = $"wx_access_token_{appid}";
            var atoken = WxAccessToken.CreateToken(appid, appsecret);
            return atoken;
        }


        public enum SnsapiType
        {
            snsapi_base,
            snsapi_userinfo,
        }

        /// <summary>
        /// 授权登录
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="reurl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Uri GetWxWebLogin(SnsapiType snsapi, string appid, string reurl, string state)
        {
            return new Uri($"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appid}&redirect_uri={HttpUtility.UrlEncode(reurl)}&response_type=code&scope={snsapi.ToString()}&state={state}#wechat_redirect");
        }

        /// <summary>
        /// 获取网站授权令牌
        /// </summary>
        /// <param name="webcode"></param>
        /// <param name="appid"></param>
        /// <param name="appsecrete"></param>
        /// <returns></returns>
        public static WxAccessToken GetWxWebToken(string webcode, string appid, string appsecret)
        {
            var apiurl = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appsecret}&code={webcode}&grant_type=authorization_code";
            WebClientLy wc = new WebClientLy();
            var rejson = wc.DownloadString(apiurl);
            LogService.AnyLog("WxWebToken", rejson);
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<WxAccessToken>(rejson);
            if (res != null)
            {
                lock (webtoken)
                {
                    if (webtoken.ContainsKey(res.openid)) webtoken.Remove(res.openid);
                    webtoken.Add(res.openid, res);
                }
            }
            return res;
        }
    }
}
