using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLBot.Extentions;
using MLBot.Mvc.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Tencent;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信事件
    /// </summary>
    [Author("Linyee", "2019-06-20")]
    public enum WxEventType
    {
        NONE,
        SUBSCRIBE,
        UNSUBSCRIBE,
        SCAN,
        LOCATION,
        CLICK,
        /// <summary>
        /// 跳转事件
        /// </summary>
        VIEW,
        /// <summary>
        /// 模板消息事件
        /// </summary>
        TEMPLATESENDJOBFINISH,
    }

    /// <summary>
    /// 微信消息类别
    /// </summary>
    [Author("Linyee", "2019-06-20")]
    public enum WxMsgType
    {
        NONE,
        TEXT,
        IMAGE,
        VOICE,
        VIDEO,
        SHORTVIDEO,
        LOCATION,
        LINK,
        EVENT,
    }

    /// <summary>
    /// 微信显示消息体
    /// </summary>
    [Author("Linyee", "2019-06-20")]
    public class WechatResponse : ContentResult
    {
        /// <summary>
        /// 内容
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public new string Content
        {
            get { return base.Content; }
            set
            {
                base.Content = value;
                //LogService.AnyLog("WeChatWebHook",$"返回消息:{value}");
            }
        }

        /// <summary>
        /// 时间的回答
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static List<string> times = new List<string>()
        {
            "您的手机上会显示的哦！",
            "现在是$time$",
            "现在是$date$ $time$"
        };
        /// <summary>
        /// 时间的回答
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static List<string> dates = new List<string>()
        {
            "您的手机上会显示的哦！",
            "今天是$date$",
            "现在是$date$ $time$"
        };
        /// <summary>
        /// 时间的回答
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static List<string> wdays = new List<string>()
        {
            "您的手机上会显示的哦！",
            "今天是$weekday$",
            "现在是$date$ $weekday$"
        };

        /// <summary>
        /// 问得太多了
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        private static List<string> tomores = new List<string>()
        {
            "你刚刚才问过我哦！",
            "您好啰嗦哦！",
            "同一个事，不要一直重复哦！",
            "您可以看上面的聊天的记录！",
            "$A$",
        };


        [Author("Linyee", "2019-06-20")]
        private static Regex setAlaim1 = new Regex(regHeaderString + "[请]?(?<time>\\d+)(?<unit>((秒)|(分钟)|(小时)))[后]?提醒[我]?(?<do>.*)[,.!，。！]*" + regEnderString, RegexOptions.Compiled);
        [Author("Linyee", "2019-06-20")]
        private static Regex setAlaim2 = new Regex(regHeaderString + "[请]?(?<time>\\d+)(?<unit>((秒)|(分钟)|(小时)))[后]?给[我个]?闹钟(叫我(?<do>.*))?[,.!，。！]*" + regEnderString, RegexOptions.Compiled);

        /// <summary>
        /// 参数字典
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        Dictionary<string, string> xdict = new Dictionary<string, string>();


        WxMsgType PostWxMsgType = WxMsgType.NONE;
        WxEventType PostWxEvent = WxEventType.NONE;
        string PostEventKey = null;
        string PostXml = null;
        string PostContent = null;
        /// <summary>
        /// 状态信息 success成功
        /// </summary>
        string PostStatus = null;
        /// <summary>
        /// 
        /// </summary>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse(HttpRequest Request, string PostXml) : this()
        {
            //对消息进行处理
            var xdoc = XDocument.Parse(PostXml);
            xdict = xdoc.Root.Elements().ToDictionary(d => d.Name.LocalName, d => d.Value);
            //LogService.AnyLog("WeChatWebHook",$"微信公众解析后数据：{xdict.ToJsonString()}");
            if (xdict.ContainsKey("MsgType")) Enum.TryParse<WxMsgType>(xdict["MsgType"]?.ToUpper(), out PostWxMsgType);
            if (xdict.ContainsKey("Event")) Enum.TryParse<WxEventType>(xdict["Event"]?.ToUpper(), out PostWxEvent);
            if (xdict.ContainsKey("EventKey")) PostEventKey = xdict["EventKey"];
            if (xdict.ContainsKey("Content")) PostContent = xdict["Content"];
            if (xdict.ContainsKey("Status")) PostStatus = xdict["Status"];
        }


        /// <summary>
        /// 
        /// </summary>
        [Author("Linyee", "2019-06-21")]
        public WechatResponse()
        {
            ContentType = "application/xml";
            StatusCode = 200;
        }


        /// <summary>
        /// 开始
        /// </summary>
        [Author("Linyee", "2019-04-01")]
        private static string regHeaderString = "^";
        /// <summary>
        /// 结束
        /// </summary>
        [Author("Linyee", "2019-04-01")]
        private static string regEnderString = "$";
        /// <summary>
        /// 小数
        /// </summary>
        [Author("Linyee", "2019-04-01")]
        private static string dblBodyString = "\\d+(\\.\\d+)?[DdFf]?";
        /// <summary>
        /// 十进制 小数 不含正负符号
        /// </summary>
        [Author("Linyee", "2019-04-01")]
        internal static Regex dblBodyRegex = new Regex(regHeaderString + dblBodyString + regEnderString, RegexOptions.Compiled);

        /// <summary>
        /// 算术数值
        /// </summary>
        internal static string ArithmeticValue = "(?<int>([\\+\\-]?\\s*\\d+[L]?))|(?<dbl>(\\d+(\\.\\d+)?([DdFfMm]?|([Ee][\\+\\-]\\d+))))";

        /// <summary>
        /// 算术表达式
        /// </summary>
        [Author("Linyee", "2019-04-01")]
        internal static Regex ArithmeticRegex = new Regex($"{regHeaderString}((帮我)?计算)?(\\s*{ArithmeticValue}\\s*[\\+\\-\\*/^&|]\\s*)+{ArithmeticValue}(=\\??)?{regEnderString}", RegexOptions.Compiled);


        /// <summary>
        /// 微信公众号机器人
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse Rebot()
        {
            var dtnow = DateTime.Now;
            var dtday_1 = dtnow.AddDays(-1);
            try
            {
                switch (PostWxMsgType)
                {
                    //文本
                    case WxMsgType.TEXT:
                        {
                            RebotContext rc = null;
                            string rcrkey = null;
                            RebotChatRecord rcr = null;
                            var botres= MLAiBot.Default.Processing(PostContent);
                            if (botres.IsOk)
                            {
                                _=GetTextResponse(botres.Msg);
                                return this;
                            }

                            //数字
                            if (dblBodyRegex.IsMatch(PostContent))
                            {
                                var val = 0d;
                                double.TryParse(PostContent, out val);
                                string res = null;
                                switch (val)
                                {
                                    //不处理
                                    case 1:
                                        //return GetEmptyResponse();
                                        break;
                                    //原文返回
                                    case 88:
                                    case 886:
                                    case 3166:
                                        res = PostContent;
                                        //return GetTextResponse(PostContent);
                                        break;
                                    default:
                                        res = $@"亲，小优暂时不能理解{PostContent}的意思。请使用更具体的文字来表述或通过其它途径获得解答。";
                                        //return GetTextResponse($@"亲，小优暂时不能理解{PostContent}的意思。请使用更具体的文字来表述或通过其它途径获得解答。");
                                        break;


                                }

                                if (string.IsNullOrEmpty(res))
                                {
                                    _ = GetEmptyResponse();

                                }
                                else
                                {
                                    _ = GetTextResponse(res);
                                }


                                if (rcr != null)
                                {
                                    rcr.Records.Add(new ChatRecordInfo()
                                    {
                                        Q = PostContent,
                                        A = Content,
                                        T = "数字",
                                        P = 1,
                                        //RT = -1,
                                        DT = DateTime.Now,
                                    });
                                }

                                return this;

                            }


                            //闹钟服务
                            var alaim = setAlaim1.Match(PostContent);
                            if (alaim == null) alaim = setAlaim2.Match(PostContent);
                            if (alaim != null && rc != null) rc.ServiceType = ServiceTypeCode.闹钟服务;
                            if (alaim != null && rc != null && rc.ServiceType == ServiceTypeCode.闹钟服务)
                            {
                                var time = -1;
                                int.TryParse(alaim.Groups["time"]?.Value, out time);
                                if (time > 0)
                                {
                                    var unit = alaim.Groups["unit"].Value;
                                    var dothin = alaim.Groups["do"].Value;
                                    DateTime dt = DateTime.MinValue;
                                    if (unit == "秒")
                                    {
                                        dt = DateTime.Now.AddSeconds(time);
                                    }
                                    else if (unit == "分钟")
                                    {
                                        dt = DateTime.Now.AddMinutes(time);
                                    }
                                    else if (unit == "小时")
                                    {
                                        dt = DateTime.Now.AddHours(time);
                                    }

                                    //添加 任务
                                    WechatCustomService.Default.Enqueue(() =>
                                    {
                                        if (DateTime.Now >= dt)
                                        {
                                            WechatCustomService.Default.SendTextMsg(xdict["FromUserName"], $"您让我{time}{unit}提醒您{dothin}，现在时间已经到了！");
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    });
                                    _ = GetTextResponse($"{time}{unit}后提醒您{dothin}，闹钟设置已完成");

                                    if (rcr != null)
                                    {
                                        rcr.Records.Add(new ChatRecordInfo()
                                        {
                                            Q = PostContent,
                                            A = Content,
                                            T = "设定闹钟",
                                            P = 1,
                                            RT = dtday_1,
                                            DT = DateTime.Now,
                                            IM = 0.01,
                                        });
                                    }
                                    return this;
                                }
                            }

                            //招呼
                            if (new Regex(regHeaderString + "[你您]好[!.。！,，]" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
                            {
                                _ = GetTextResponse("您好，我是小优，正在为您服务！");
                                if (rcr != null)
                                {
                                    rcr.Records.Add(new ChatRecordInfo()
                                    {
                                        Q = PostContent,
                                        A = Content,
                                        T = "被招呼",
                                        P = 1,
                                        //RT = dtday_1,
                                        DT = DateTime.Now,
                                        IM = 0.01,
                                    });
                                }
                                return this;
                            }

                            //回应感谢
                            if (new Regex(regHeaderString + "谢谢[您你]?[啦了]?[!.。！,，]" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
                            {
                                _ = GetTextResponse("能为您服务，是小优的荣幸！");
                                if (rcr != null)
                                {
                                    rcr.Records.Add(new ChatRecordInfo()
                                    {
                                        Q = PostContent,
                                        A = Content,
                                        T = "被感谢",
                                        P = 1,
                                        //RT = -1,
                                        DT = DateTime.Now,
                                        IM = 0.01,
                                    });
                                }
                                return this;
                            }

                            //再见
                            if (new Regex(regHeaderString + "再见[啦了]?[!.。！,，]" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
                            {
                                _ = GetTextResponse("能为您服务，是小优的荣幸，期待您的再次光临！");
                                if (rcr != null)
                                {
                                    rcr.Records.Add(new ChatRecordInfo()
                                    {
                                        Q = PostContent,
                                        A = Content,
                                        T = "被再见",
                                        P = 1,
                                        //RT = -1,
                                        DT = DateTime.Now,
                                        IM = 0.01,
                                    });
                                }
                                return this;
                            }

                            //不能处理
                            LogService.AnyLog("WxRebotNoSuper", $@"小优未能处理信息：{PostXml}");
                            _ = GetTextResponse($@"亲，小优暂时不能理解您的信息，已收妥保存信息。请切换服务方式或通过其它途径获得解答。");

                            if (rcr != null)
                            {
                                rcr.Records.Add(new ChatRecordInfo()
                                {
                                    Q = PostContent,
                                    A = Content,
                                    T = "未处理",
                                    P = 1,
                                    //RT = -1,
                                    DT = DateTime.Now,
                                    IM = 0.01,
                                });
                            }
                            return this;
                        }
                    //事件
                    case WxMsgType.EVENT:
                        {
                            switch (PostWxEvent)
                            {
                                //关注
                                case WxEventType.SUBSCRIBE:
                                    return GetTextResponse($@"欢迎回家！！我是小优，可以为您提供很多服务哦！");
                                //取消关注
                                case WxEventType.UNSUBSCRIBE:
                                    return GetTextResponse($@"亲，小优和收呗优选都很舍不得您！！记得常回来看看哦！");
                                //菜单处理
                                case WxEventType.CLICK:
                                    return GetResponseFromMenu(PostEventKey);
                                //不处理
                                case WxEventType.TEMPLATESENDJOBFINISH:
                                    return GetEmptyResponse();
                                //url跳转
                                case WxEventType.VIEW:
                                    return GetEmptyResponse();
                                //不能处理
                                case WxEventType.SCAN:
                                case WxEventType.LOCATION:
                                case WxEventType.NONE:
                                default:
                                    LogService.AnyLog("WxRebotNoSuper", $@"小优未能处理信息：{PostXml}");
                                    return GetTextResponse($@"亲，抱歉！小优暂时不能理解您的{PostWxEvent.ToString()}事件信息，已收妥保存信息。请通过其它途径获得解答。");
                            }
                        }

                    //不能处理
                    case WxMsgType.IMAGE:
                    case WxMsgType.LINK:
                    case WxMsgType.LOCATION:
                    case WxMsgType.SHORTVIDEO:
                    case WxMsgType.VIDEO:
                    case WxMsgType.VOICE:
                    case WxMsgType.NONE:
                    default:
                        LogService.AnyLog("WxRebotNoSuper", $@"小优未能处理信息：{PostXml}");
                        return GetTextResponse($@"亲，抱歉！小优暂时不能理解您的{PostWxMsgType.ToString()}信息，已收妥保存信息。请通过其它途径获得解答。");
                }
            }
            catch (Exception ex)
            {
                LogService.Exception(ex);
                return GetTextResponse($@"亲，小优的软体出问题了{ex.Message}，请通过其它途径获得解答。");
            }
        }

        //private static Dictionary<string, RebotContext> rdict = new Dictionary<string, RebotContext>();

        /// <summary>
        /// 菜单响应
        /// </summary>
        /// <param name="postEventKey"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-06-24")]
        private WechatResponse GetResponseFromMenu(string postEventKey)
        {
            LogService.AnyLog("WxRebotNoSuper", $@"小优未能正确识别菜单信息：{postEventKey} {PostXml}");
            return GetEmptyResponse();
        }

        /// <summary>
        /// 获取空响应
        /// 无须回发消息时
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetEmptyResponse()
        {
            Content = "";//success 或空字串
            return this;
        }


        /// <summary>
        /// 获取文本响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetTextResponse(string rmsg)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[text]]></MsgType>
  <Content><![CDATA[{rmsg}]]></Content>
</xml>";
            return this;
        }

        /// <summary>
        /// 获取图片响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetImageResponse(string media_id)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[image]]></MsgType>
  <Image>
    <MediaId><![CDATA[{media_id}]]></MediaId>
  </Image>
</xml>";
            return this;
        }

        /// <summary>
        /// 获取语音响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetVoiceResponse(string media_id)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[voice]]></MsgType>
  <Voice>
    <MediaId><![CDATA[{media_id}]]></MediaId>
  </Voice>
</xml>";
            return this;
        }

        /// <summary>
        /// 获取视频响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetVideoResponse(string title, string description, string media_id)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[video]]></MsgType>
  <Video>
    <Title><![CDATA[{title}]]></Title>
    <Description><![CDATA[{description}]]></Description>
    <MediaId><![CDATA[{media_id}]]></MediaId>
  </Video>
</xml>";
            return this;
        }

        /// <summary>
        /// 获取音乐响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetMusicResponse(string title, string description, string MUSIC_Url, string HQ_MUSIC_Url, string media_id)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[music]]></MsgType>
  <Music>
    <Title><![CDATA[{title}]]></Title>
    <Description><![CDATA[{description}]]></Description>
    <ThumbMediaId><![CDATA[{media_id}]]></ThumbMediaId>
    <MusicUrl><![CDATA[{MUSIC_Url}]]></MusicUrl>
    <HQMusicUrl><![CDATA[{HQ_MUSIC_Url}]]></HQMusicUrl>
  </Music>
</xml>";
            return this;
        }

        /// <summary>
        /// 获取图文响应
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public WechatResponse GetArticlesResponse(string title, string description, string picurl, string url)
        {
            Content = $@"<xml>
  <ToUserName><![CDATA[{xdict["FromUserName"]}]]></ToUserName>
  <FromUserName><![CDATA[{xdict["ToUserName"]}]]></FromUserName>
  <CreateTime>{DateTime.Now.GetTimestamp10()}</CreateTime>
  <MsgType><![CDATA[news]]></MsgType>
  <ArticleCount>1</ArticleCount>
  <Articles>
    <item>
      <Title><![CDATA[{title}]]></Title>
      <Description><![CDATA[{description}]]></Description>
      <PicUrl><![CDATA[{picurl}]]></PicUrl>
      <Url><![CDATA[{url}]]></Url>
    </item>
  </Articles>
</xml>";
            return this;
        }
    }
}
