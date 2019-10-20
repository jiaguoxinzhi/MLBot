using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MLBot.Mvc.Areas.MLBot.Controllers;
using MLBot.Mvc.Extentions;
using MLBot.Mvc.WechatMP;
using Tencent;
using MLBot.Extentions;

namespace MLBot.Mvc.Areas.WxWebHook.Controllers
{
    /// <summary>
    /// 微信Api
    /// </summary>
    [ApiController]
    [Author("Linyee", "2019-06-05")]
    public class WeChatController : BaseApiController
    {
        private static string UrlRoot = "";
        private static WXBizMsgCrypt wxcrypt = null;

        /// <summary>
        /// 微信后台验证地址（使用Get）
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机信息</param>
        /// <param name="echostr">回显信息</param>
        /// <returns></returns>
        [Author("Linyee", "2019-06-05")]
        [HttpGet]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            //获取请求域名
            UrlRoot = Request.GetUrlRoot();
            if (CheckSignature.Check(signature, timestamp, nonce, WechatMpSettings.Default.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, WechatMpSettings.Default.Token) + "。如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
            }
        }

        /// <summary>
        /// 微信公众号服务接口
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-06-05")]
        [Modifier("Linyee", "2019-06-20", "完善并支持加密")]
        [HttpPost]
        public ActionResult Post(string signature, string timestamp, string nonce, string openid, string encrypt_type, string msg_signature)
        {
            //StringBuilder log = new StringBuilder();
            LogService.AnyLog("WeChatWebHook", $"WxApi\tPost In");//{DateTime.Now.ToString("HH:mm:ss.fffffff")}\t

            PostModel postModel = new PostModel()
            {
                Token = WechatMpSettings.Default.Token,  //根据自己后台的设置保持一致
                Signature = signature,
                Timestamp = timestamp,
                Nonce = nonce,
                AppId = WechatMpSettings.Default.AppId,
                EncodingAESKey = WechatMpSettings.Default.EncodingAESKey,
                Msg_Signature = msg_signature,
            };

            try
            {
                //加密异常
                if (encrypt_type == "aes" && string.IsNullOrEmpty(postModel.EncodingAESKey))
                {
                    return Content("公从号接口有异常，请通过其它途径解决您的问题，并反馈此消息");
                }
                if (encrypt_type == "aes" && wxcrypt == null)
                {
                    wxcrypt = new WXBizMsgCrypt(postModel.Token, postModel.EncodingAESKey, postModel.AppId);
                }

                if (!CheckSignature.Check(signature, postModel))
                {
                    return Content("参数错误！");
                }

                String PostXml = Request.GetBodyString();
                //解密信息
                if (encrypt_type == "aes")
                {
                    var aeskey = WechatMpSettings.Default.EncodingAESKey;
                    string detext = null;
                    var res = wxcrypt.DecryptMsg(postModel.Msg_Signature, postModel.Timestamp, postModel.Nonce, PostXml, ref detext);
                    //Console.WriteLine($"{res} detext({postModel.Token},{postModel.EncodingAESKey},{postModel.AppId},{postModel.Msg_Signature}, {postModel.Timestamp}, {postModel.Nonce},{PostXml})>{detext}");
                    PostXml = detext;
                }

                WechatResponse wxsponse = new WechatResponse(Request, PostXml);
                var botsponse= wxsponse.Rebot();
                LogService.AnyLog("WeChatWebHook", "响应源数据", $"{botsponse.Content}");
                //加密信息
                if (encrypt_type == "aes")
                {
                    var rTimeStamp = DateTime.Now.GetTimestamp10();
                    var rNonce = RandomString.BuildRndString(16);
                    string restr = null;
                    var rb = wxcrypt.EncryptMsg(botsponse.Content, rTimeStamp + "", rNonce, ref restr);
                    if (rb == 0)
                    {
                        botsponse.Content = restr;
                    }
                    else
                    {
                        $"{rb} EncryptMsg".WriteErrorLine();
                    }
                }
                LogService.AnyLog("WeChatWebHook","响应密数据", $"{botsponse.Content}");
                return botsponse;
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogService.AnyLog("WeChatWebHook", $"{ex.Message}");
                LogService.Exception(ex);
                return Content("");
                #endregion
            }
            finally
            {
                LogService.AnyLog("WeChatWebHook", $"WxApi\tPost Out");
            }
        }
    }
}