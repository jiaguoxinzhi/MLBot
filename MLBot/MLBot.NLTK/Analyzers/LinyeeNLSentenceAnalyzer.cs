using MLBot.NLTK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MLBot.NLTK.Analyzers
{
    /// <summary>
    /// 分句器
    /// </summary>
    [Author("Linyee", "2019-07-24")]
    internal ref struct LinyeeNLSentenceAnalyzer
    {
        #region "具体语句"

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
        /// 结束位
        /// </summary>

        [Author("Linyee", "2019-07-24")]
        private List<int> stops;

        /// <summary>
        /// 段落清单
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        private List<Sentence> sentences;// = new List<Paragraph>();

        /// <summary>
        /// 分句
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        internal WordAnalyResult SentenceAnaly()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            WordAnalyResult result = new WordAnalyResult();
            stops.Clear();
            stops.Add(0);//开始值
            //计算停止处 
            for (var fi = 0; fi < Raw.Length; fi++)
            {
                //句断符号停止
                if (SentenceStopChars.Contains(Raw[fi]))
                {
                    if (Raw[fi] != '.' || Raw[fi] == '.' && (fi == Raw.Length - 1 || Raw[fi++] == ' '))
                        stops.Add(fi);
                }
            }
            stops.Add(Raw.Length);//结束值
            //截句
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
                var sch = '\x0';
                if (epi < Raw.Length)
                {
                    sch = Raw[epi];
                    //epi++;//包含符号//注释后不包含符号
                }
                var slen = epi - spi;
                if (slen < 1) break;
                var span = Raw.Slice(spi, slen);
                //$"P>{span.ToString()}".WriteInfoLine();
                var words = new LinyeeNLWordAnalyzer(span, Enc, WordStopChars).WordAnaly();
                var res = new Sentence(words, sch);
                sentences.Add(res); ;
            }

            //
            return result.SetData(sentences).SetOk();
        }

        /// <summary>
        /// 分句器
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal LinyeeNLSentenceAnalyzer(ReadOnlySpan<char> span, Encoding enc, List<char> sentencestopchars, List<char> wordstopchars)
        {
            this.Raw = span;
            this.Enc = enc;
            this.SentenceStopChars = sentencestopchars;
            this.WordStopChars = wordstopchars;

            stops = new List<int>();
            sentences = new List<Sentence>();
        }
        #endregion

    }
}
