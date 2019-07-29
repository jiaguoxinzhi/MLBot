using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.NLTK.Models
{
    /// <summary>
    /// 语句信息
    /// </summary>
    [Author("Linyee", "2019-07-24")]
    internal class Sentence
    {
        [Author("Linyee", "2019-07-24")]
        public Sentence(WordAnalyResult<List<string>> wordAnalyResult)
        {
            this.Words = wordAnalyResult.Data;
        }
        [Author("Linyee", "2019-07-24")]
        public Sentence(WordAnalyResult<List<string>> wordAnalyResult, char fh) : this(wordAnalyResult)
        {
            Words?.Add(fh.ToString());
        }
        [Author("Linyee", "2019-07-24")]
        public List<string> Words { get; private set; } = new List<string>();
    }
}
