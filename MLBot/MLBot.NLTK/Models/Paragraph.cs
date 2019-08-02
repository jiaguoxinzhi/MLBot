using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.NLTK.Models
{
    /// <summary>
    /// 段落信息
    /// </summary>
    [Author("Linyee", "2019-07-24")]
    internal class Paragraph
    {

        [Author("Linyee", "2019-07-24")]
        public Paragraph(WordAnalyResult wordAnalyResult)
        {
            this.Sentences = wordAnalyResult.Data;
        }

        [Author("Linyee", "2019-07-24")]
        public object Sentences { get; private set; }
    }
}
