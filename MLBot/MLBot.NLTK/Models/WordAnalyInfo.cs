using System;
using System.Collections.Generic;
using System.Text;
using MLBot.Enums;

namespace MLBot.NLTK.Models
{
    /// <summary>
    /// 分析结果
    /// </summary>
    [Author("Linyee", "2019-07-17")]
    public class WordAnalyResult: WordAnalyResult<object>
    {
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-19")]
        internal new WordAnalyResult SetData(object words)
        {
            this.Data = words;
            return this;
        }
    }


    /// <summary>
    /// 分析结果 强类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Author("Linyee", "2019-07-24")]
    public class WordAnalyResult<T>
    {
        /// <summary>
        /// 空
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        public static readonly WordAnalyResult Empty = new WordAnalyResult();

        /// <summary>
        /// 数据
        /// </summary>
        [Author("Linyee", "2019-07-24")]
        public T Data { get; protected set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Author("Linyee", "2019-07-19")]
        public StatusCodeEnum State { get; protected set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        [Author("Linyee", "2019-07-29")]
        public string Desc { get; protected set; }

        /// <summary>
        /// 是否是OK状态
        /// </summary>
        [Author("Linyee", "2019-07-19")]
        public bool IsOK => State == StatusCodeEnum.OK;

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        public WordAnalyResult<T> SetData(T data)
        {
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 设置正确状态
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        public WordAnalyResult<T> SetFail(string msg)
        {
            this.State = StatusCodeEnum.FAIL;
            this.Desc = msg;
            return this;
        }

        /// <summary>
        /// 设置正确状态
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-07-24")]
        public WordAnalyResult<T> SetOk()
        {
            this.State = StatusCodeEnum.OK;
            return this;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="obj"></param>
        [Author("Linyee", "2019-07-24")]
        public static implicit operator WordAnalyResult(WordAnalyResult<T> obj)
        {
            return new WordAnalyResult()
            {
                Data = obj.Data,
                State = obj.State,
            };
        }
    }
}
