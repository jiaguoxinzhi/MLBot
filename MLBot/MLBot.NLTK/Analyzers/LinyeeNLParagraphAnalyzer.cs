using MLBot.NLTK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MLBot.NLTK.Analyzers
{
    /// <summary>
    /// 分段器
    /// </summary>
    [Author("Linyee", "2019-07-24")]
    internal ref struct LinyeeNLParagraphAnalyzer
    {
        #region "具体段落"

        /// <summary>
        /// 数据
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal ReadOnlySpan<char> Raw { get; }

        /// <summary>
        /// 编码
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal Encoding Enc { get; }

        /// <summary>
        /// 词语停止的字符集，通常是标点符号
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal readonly List<char> WordStopChars;


        /// <summary>
        /// 语句停止的字符集，通常是句断符号
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal readonly List<char> SentenceStopChars;

        /// <summary>
        /// 段落停止的字符集，通常是换行符号
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal readonly List<char> ParagraphStopChars;

        /// <summary>
        /// 结束位
        /// </summary>

        [Author("Linyee", "2019-07-24")]
        private List<int> stops;


        /// <summary>
        /// 段落清单
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        private List<Paragraph> paragraphs;// = new List<Paragraph>();

        /// <summary>
        /// 分段
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        internal WordAnalyResult ParagraphAnaly()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());

            WordAnalyResult result = new WordAnalyResult();
            stops.Clear();
            stops.Add(0);//开始值
            //计算停止处 
            for (var fi = 0; fi < Raw.Length; fi++)
            {
                //段落符号停止
                if (ParagraphStopChars.Contains(Raw[fi]))
                {
                    stops.Add(fi);
                }
            }
            stops.Add(Raw.Length);//结束值
            //截段
            for (var fi = 1; fi < stops.Count; fi++)
            {
                var spi = stops[fi - 1];
                if (spi > 0)
                {
                    var splitword = Raw[spi];
                    //words.Add(splitword.ToString());
                    spi++;
                }
                var epi = stops[fi];
                //if (epi >= Raw.Length && paragraphs.Count > 0)
                //{
                //    break;//退出
                //}
                //else if(epi< Raw.Length)
                //{
                //    epi++;
                //}
                var slen = epi - spi;
                if (slen < 1) break;
                var span = Raw.Slice(spi, slen);
                paragraphs.Add(new Paragraph(new LinyeeNLSentenceAnalyzer(span, Enc, SentenceStopChars, WordStopChars).SentenceAnaly()));
            }

            //
            return result.SetData(paragraphs).SetOk();
        }

        /// <summary>
        /// 分段器
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal LinyeeNLParagraphAnalyzer(ReadOnlySpan<char> span, Encoding enc, List<char> paragraphstopchars, List<char> sentencestopchars, List<char> wordstopchars)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());

            this.Raw = span;
            this.Enc = enc;
            this.ParagraphStopChars = paragraphstopchars;
            this.SentenceStopChars = sentencestopchars;
            this.WordStopChars = wordstopchars;

            paragraphs = new List<Paragraph>();
            stops = new List<int>();
        }
        #endregion

    }
}
