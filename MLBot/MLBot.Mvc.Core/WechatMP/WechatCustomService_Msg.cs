using System;
using System.Collections.Generic;
using System.Text;
using MLBot.Enums;
using MLBot.Extentions;
using Tencent;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信客服消息主发
    /// </summary>
    public partial class WechatCustomService
    {
        private static string SendUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=$access_token$";

        /// <summary>
        /// 加密方式
        /// </summary>
        public string Encrypt_type { get; set; }
        /// <summary>
        /// 加解密类
        /// </summary>
        public WXBizMsgCrypt WxCrypt { get; set; }
        /// <summary>
        /// 是否有发送客服消息的权限
        /// </summary>

        public bool AccessSendMsg { get; set; } = true;

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
            string upmsg = msg;

            ////加密信息
            //if (Encrypt_type == "aes")
            //{
            //    var rTimeStamp = DateTime.Now.GetTimestamp10();
            //    var rNonce = RandomString.BuildRndString(16);
            //    string restr = null;
            //    var rb = WxCrypt.EncryptMsg(msg, rTimeStamp + "", rNonce, ref restr);
            //    if (rb == 0)
            //    {
            //        upmsg = restr;
            //    }
            //    else
            //    {
            //        $"{rb} EncryptMsg".WriteErrorLine();
            //    }
            //}
            //LogService.AnyLog("WeChatWebHook", "客服密数据", $"{upmsg}");

            var res = wc.UploadString(url, upmsg);
            SentSponseInfo sent = Newtonsoft.Json.JsonConvert.DeserializeObject<SentSponseInfo>(res);
            if (sent.errcode == 0)
            {
                return result.SetData(true).SetOk();
            }
            else
            {
                if (sent.errcode == 48001)
                {
                    AccessSendMsg = false;
                }
                return result.SetData(false).SetFail(sent.errmsg);
            }
        }
    }

    public class SentSponseInfo
    {
        //{"errcode":48001,"errmsg":"api unauthorized hints: [PHCFLXHHRa-nwOTXa!]"}
        public int errcode{get;set;}
        public string errmsg { get; set; }

    }
}
