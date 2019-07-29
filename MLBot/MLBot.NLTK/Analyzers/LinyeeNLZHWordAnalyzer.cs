using MLBot.NLTK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MLBot.NLTK.Analyzers
{
    /// <summary>
    /// 中文分词器
    /// </summary>
    [Author("Linyee", "2019-07-24")]
    internal ref struct LinyeeNLZHWordAnalyzer
    {
        #region "中文分词"

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
        /// 词典
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        public LinyeeWrodDict WordDict { get; private set; }
        /// <summary>
        /// 输出 词集
        /// </summary>

        [Author("Linyee", "2019-07-19")]
        private List<WordInfoOnce> words;

        /// <summary>
        /// 中文分词
        /// 针对整个子句
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        internal WordAnalyResult<List<WordInfoOnce>> WordAnaly()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            WordAnalyResult<List<WordInfoOnce>> result = new WordAnalyResult<List<WordInfoOnce>>();

            var winfos = WordDict.WordAnaly(Raw, 0);
            words.AddRange(winfos);
            //
            return result.SetData(words).SetOk();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中文分词器
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        internal LinyeeNLZHWordAnalyzer(ReadOnlySpan<char> span, Encoding enc, LinyeeWrodDict wdict)
        {
            this.Raw = span;
            this.Enc = enc;
            this.WordDict = wdict;

            words = new List<WordInfoOnce>();
        }
        #endregion

    }
}
