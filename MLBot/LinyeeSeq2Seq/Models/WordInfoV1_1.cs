using System;
using System.Collections.Generic;
using System.Text;

namespace LinyeeSeq2Seq.Models
{
    /// <summary>
    /// 词汇信息
    /// </summary>
    //[Author("Linyee", "2019-07-19")]
    [Serializable]
    public class WordInfoV1_1
    {
        /// <summary>
        /// 索引
        /// </summary>
        //[Author("Linyee", "2019-07-25")]
        public int i { get; set; }

        /// <summary>
        /// 词
        /// </summary>
        //[Author("Linyee", "2019-07-25")]
        public string w { get; set; }

        /// <summary>
        /// 词频
        /// </summary>
        //[Author("Linyee", "2019-07-25")]
        public double f { get; set; }

    }
}
