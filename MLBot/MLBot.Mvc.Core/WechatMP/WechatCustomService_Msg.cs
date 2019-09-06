using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信客服消息主发
    /// </summary>
    public partial class WechatCustomService
    {
        private static string SendUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=$access_token$";

        /// <summary>
        /// 客服主动发送消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="text"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public ExecuteResult<bool> SendTextMsg(string openid, string text)
        {
            ExecuteResult<bool> result = new ExecuteResult<bool>();
            //Console.WriteLine($"参数>{AppSettings.AppId} {AppSettings.AppSecret}");
            var wxtoken = WxAccessToken.GetToken(WechatMpSettings.Default.AppId, WechatMpSettings.Default.AppSecret);
            if (wxtoken.errcode != 0) return result.SetFail(wxtoken.errmsg);
            var url = SendUrl.Replace("$access_token$", wxtoken.access_token);
            var wc = new WebClientLy();
            var msg = $@"{{
    ""touser"":""{openid}"",
    ""msgtype"":""text"",
    ""text"":
    {{
                ""content"":""{text}""
    }}
}}";
            var res = wc.UploadString(url, msg);
            return result.SetOk(res);
        }
    }
}
