using MLBot.Enums;
using MLBot.NLTK.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MLBot.NLTK.Models
{
    /// <summary>
    /// 单词信息
    /// </summary>
    [Author("Linyee", "2019-07-19")]
    internal class WordInfoOnce
    {
        #region 简

        /// <summary>
        /// 单词
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public string w { get; set; }

        /// <summary>
        /// 原型  prototype
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public string pt { get; set; }

        /// <summary>
        /// 词频
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public double f { get; set; }

        /// <summary>
        /// 词性
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public string p { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public string py { get; set; }

        /// <summary>
        /// 语言种类，可以多种语言共用
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public string l { get; set; }

        /// <summary>
        /// 是否区分大小写
        /// 0 未指定 1不区分 2区分
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public byte? o { get; set; }

        /// <summary>
        /// 是否多音
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        public bool? isp { get; set; }
        #endregion

        #region 全

        /// <summary>
        /// 状态
        /// EntityState.Detached 分离
        /// </summary>
        [JsonIgnore]
        public EntityState State { get; set; } = EntityState.Unchanged;

        /// <summary>
        /// 特征码作为键名这样就可以允许重复的单词
        /// </summary>
        [Author("Linyee", "2019-07-25")]
        [JsonIgnore]
        //public ulong Key => BitConverter.ToUInt64((w+l+p+py).ToMd5Bytes(),0);
        public string Key => $"{w}·{p}";

        /// <summary>
        /// 键的索引方式 
        /// </summary>
        [JsonIgnore]
        public KeyType IndexKeyType { get; set; } = KeyType.Word;
        #endregion

        #region 构造

        /// <summary>
        /// 
        /// </summary>
        [Author("Linyee", "2019-07-19")]
        public WordInfoOnce() { }

        /// <summary>
        /// 构造单词
        /// 不存在时会自动添加到默认词典
        /// </summary>
        /// <param name="word"></param>
        [Author("Linyee", "2019-07-19")]
        public WordInfoOnce(string word)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            this.w = word;
        }

        /// <summary>
        /// 构造单词
        /// </summary>
        /// <param name="word"></param>
        [Author("Linyee", "2019-07-19")]
        public WordInfoOnce(ReadOnlySpan<char> word) : this(string.Join("", word.ToArray()))
        {
        }

        /// <summary>
        /// 构造单词
        /// </summary>
        /// <param name="ch"></param>
        [Author("Linyee", "2019-07-19")]
        public WordInfoOnce(char ch) : this(ch.ToString())//
        {
        }
        #endregion

        /// <summary>
        /// 设置索引类别
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-25")]
        internal WordInfoOnce SetKeyType(KeyType key)
        {
            this.IndexKeyType = KeyType.Key;
            return this;
        }
    }
}
