using MLBot.Extentions;
using MLBot.NLTK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MLBot.NLTK.Analyzers
{
    /// <summary>
    /// 分词器
    /// </summary>
    [Author("Linyee", "2019-07-17")]
    internal ref struct LinyeeNLWordAnalyzer
    {
        #region "具体语句"

        /// <summary>
        /// 数据
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal ReadOnlySpan<char> Raw { get; }

        /// <summary>
        /// 编码
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal Encoding Enc { get; }

        /// <summary>
        /// 词语停止的字符集，通常是标点符号
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal readonly List<char> StopChars;

        /// <summary>
        /// 结束位
        /// </summary>

        [Author("Linyee", "2019-07-17")]
        private List<int> stops;

        /// <summary>
        /// 输出 词集
        /// </summary>

        [Author("Linyee", "2019-07-19")]
        private List<string> words;

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-17")]
        internal WordAnalyResult<List<string>> WordAnaly()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            WordAnalyResult<List<string>> result = new WordAnalyResult<List<string>>();

            #region 标点分词
            stops.Clear();
            stops.Add(0);//开始值
            //计算停止处 
            for (var fi = 0; fi < Raw.Length; fi++)
            {
                //标点符号停止
                if (StopChars.Contains(Raw[fi]))
                {
                    stops.Add(fi);
                }
            }
            stops.Add(Raw.Length);//结束值
            //截词
            for (var fi = 1; fi < stops.Count; fi++)
            {
                var spi = stops[fi - 1];
                if (spi > 0)
                {
                    var splitword = Raw[spi];
                    words.Add(splitword.ToString());
                    spi++;
                }
                var epi = stops[fi];
                //if (epi >= Raw.Length && words.Count>0)
                //{
                //    break;//退出
                //}
                var wlen = epi - spi;
                if (wlen < 1) break;
                var word = Raw.Slice(spi, wlen);
                var wordstr = word.ToString();

                //词中是否含有中文
                if (wordstr.ContainChinese())
                {
                    try
                    {
                        var winfos = new LinyeeNLZHWordAnalyzer(word, Enc, LinyeeWrodDict.Default).WordAnaly();
                        if (winfos.IsOK)
                        {
                            words.AddRange(winfos.Data.Select(p => p.w));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString().WriteErrorLine();
                    }
                }
                else
                {
                    words.Add(wordstr);
                    var info = new WordInfoOnce(wordstr);
                }
            }
            #endregion

            //
            return result.SetData(words).SetOk();
        }

        /// <summary>
        /// 分词器
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal LinyeeNLWordAnalyzer(ReadOnlySpan<char> span, Encoding enc, List<char> stopchars)
        {
            this.Raw = span;
            this.Enc = enc;
            this.StopChars = stopchars;

            stops = new List<int>();
            words = new List<string>();
        }
        #endregion

    }
}
