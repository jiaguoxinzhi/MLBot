
using MLBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MLBot
{
    /// <summary>
    /// 智能Ai
    /// </summary>
    [Author("Linyee","2019-07-01")]
    public class MLAiBot
    {
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



        [Author("Linyee", "2019-06-20")]
        private static Regex setAlaim1 = new Regex(regHeaderString + "[请]?(?<time>\\d+)(?<unit>((秒)|(分钟)|(小时)))[后]?提醒[我]?(?<do>.*)[,.!，。！]*" + regEnderString, RegexOptions.Compiled);
        [Author("Linyee", "2019-06-20")]
        private static Regex setAlaim2 = new Regex(regHeaderString + "[请]?(?<time>\\d+)(?<unit>((秒)|(分钟)|(小时)))[后]?给[我个]?闹钟(叫我(?<do>.*))?[,.!，。！]*" + regEnderString, RegexOptions.Compiled);

        /// <summary>
        /// 默认实例
        /// </summary>
        [Author("Linyee", "2019-07-01")]
        public static readonly MLAiBot Default=new MLAiBot();

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="buffer"></param>
        [Author("Linyee", "2019-07-01")]
        public byte[] Processing(byte[] buffer)
        {
            var b0 = buffer[0];
            if (b0 >= 0x20)
            {
                var text = Encoding.UTF8.GetString(buffer);
                return Encoding.UTF8.GetBytes("正在开发中。。敬请期待！");
            }
            else
            {
                throw new MLBotNoSuperMessageFormat();
            }
        }

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="text"></param>
        [Author("Linyee", "2019-10-20")]
        public ExecuteResult<string> Processing(string text)
        {
            ExecuteResult<string> result = new ExecuteResult<string>();
            var PostContent = text;

            ////问时间
            //if (new Regex(regHeaderString + "(现在)?几点[了]?[\\?]?" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent) || new Regex(regHeaderString + "(给我)?报[个]?时[.,。，！!]?" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
            //{
            //    _ = GetTextResponse(
            //        times[RandomNumber.GetRndInt(0, times.Count - 1)]
            //        .Replace("$date$", dtnow.ToString("yyyy-MM-dd"))
            //        .Replace("$time$", dtnow.ToString("HH:mm")));

            //    if (rcr != null)
            //    {
            //        rcr.Records.Add(new ChatRecordInfo()
            //        {
            //            Q = PostContent,
            //            A = Content,
            //            T = "被问时间",
            //            P = 1,
            //            RT = dtday_1,
            //            DT = DateTime.Now,
            //            IM = 0.01,
            //        });
            //    }
            //    return this;
            //}

            ////问时间
            //if (new Regex(regHeaderString + "(今天[是]?)?几号[了]?[\\?]*" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent) || new Regex(regHeaderString + "(今天[是]?)?的日期[是]?[\\?]*" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
            //{
            //    _ = GetTextResponse(
            //        dates[RandomNumber.GetRndInt(0, dates.Count - 1)]
            //        .Replace("$date$", dtnow.ToString("yyyy-MM-dd"))
            //        .Replace("$time$", dtnow.ToString("HH:mm")));

            //    if (rcr != null)
            //    {
            //        rcr.Records.Add(new ChatRecordInfo()
            //        {
            //            Q = PostContent,
            //            A = Content,
            //            T = "被问时间",
            //            P = 1,
            //            RT = dtday_1,
            //            DT = DateTime.Now,
            //            IM = 0.01,
            //        });
            //    }
            //    return this;
            //}

            ////问时间
            //if (new Regex(regHeaderString + "(今天[是]?)?(星期|周)[几]?[了]?[\\?]*" + regEnderString, RegexOptions.Compiled).IsMatch(PostContent))
            //{
            //    _ = GetTextResponse(
            //        wdays[RandomNumber.GetRndInt(0, wdays.Count - 1)]
            //        .Replace("$date$", dtnow.ToString("yyyy-MM-dd"))
            //        .Replace("$weekday$", "星期" + dtnow.DayOfWeek.ToString("d"))
            //        );

            //    if (rcr != null)
            //    {
            //        rcr.Records.Add(new ChatRecordInfo()
            //        {
            //            Q = PostContent,
            //            A = Content,
            //            T = "被问时间",
            //            P = 1,
            //            RT = dtday_1,
            //            DT = DateTime.Now,
            //            IM = 0.01,
            //        });
            //    }
            //    return this;
            //}

            return result.SetFail("正在开发中。。敬请期待！");
        }
    }
}
