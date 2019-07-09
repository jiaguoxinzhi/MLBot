
using MLBot.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MLBot.Mvc
{
    /// <summary>
    /// 执行结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Author("Linyee", "2018-06-29")]
    public class ExecuteResult : ExecuteResult<object>
    {

    }


    /// <summary>
    /// 执行结果
    /// Linyee 2018-12-19
    /// </summary>
    [Author("Linyee", "2018-06-29")]
    public class ExecuteResult<T>
    {

        #region "构造"
        /// <summary>
        /// 创建 空 执行结果
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult()
        {
        }

        /// <summary>
        /// 创建 执行结果
        /// </summary>
        /// <param name="isok"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult(bool isok)
        {
            this.IsOk = isok;
        }

        /// <summary>
        /// 创建 正确 执行结果
        /// </summary>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult(string msg)
        {
            this.IsOk = true;
            this.Msg = msg;
        }

        /// <summary>
        /// 创建 异常 执行结果
        /// </summary>
        /// <param name="ex"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult(Exception ex)
        {
            this.IsOk = false;
            this.Msg = ex.Message;
            this.InnerException = ex;
            if (ConfigBase.Default.IsException) LogService.Exception(ex);
        }

        ///// <summary>
        ///// 捕获运行错误
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public static ExecuteResult<T> TryRun(Func<T> p)
        //{
        //    ExecuteResult<T> result = new ExecuteResult<T>();
        //    try
        //    {
        //        var res = p.Invoke();
        //        return result.SetOk("Ok", res);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return result.SetException(ex);
        //    }
        //    //catch (DbEntityValidationException ex)
        //    //{
        //    //    return result.SetException(ex);
        //    //}
        //    catch (Exception ex)
        //    {
        //        return result.SetException(ex);
        //    }
        //}

        #endregion

        #region "方法"


        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> Disable(string msg = "已禁用")
        {
            Enabled = false;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-03-13")]
        public ExecuteResult<T> SetData(T data)
        {
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="res"></param>
        [Author("Linyee", "2018-06-29")]
        [Modifier("Linyee", "2019-07-06", "增加Desc、Enabled")]
        public ExecuteResult<T> Set(ExecuteResult<T> res)
        {
            this.Code = res.Code;

            this.Desc = res.Desc;   //[Modifier("Linyee", "2019-07-06", "增加Desc、Enabled")]
            this.Msg = res.Msg;
            this.Enabled = res.Enabled;   //[Modifier("Linyee", "2019-07-06", "增加Desc、Enabled")]              

            this.Page = res.Page;
            this.Limit = res.Limit;
            this.Count = res.Count;
            this.Data = res.Data;
            this.InnerException = res.InnerException;

            return this;
        }

        /// <summary>
        /// 设置代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetCode(StatusCodeEnum code)
        {
            this.Code = code;
            return this;
        }

        /// <summary>
        /// 设置成功
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetSuccess(string msg = "Ok")
        {
            return SetOk(msg);
        }

        /// <summary>
        /// 设置完成
        /// </summary>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetOk(string msg = "Ok")
        {
            this.IsOk = true;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置完成
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        [Author("Linyee", "2018-06-29")]
        [Modifier("Linyee", "2019-07-06", "引用SetOk")]
        public ExecuteResult<T> SetOk(string msg, T data)
        {
            this.SetOk(msg);
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 设置错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetError(string msg, StatusCodeEnum code = StatusCodeEnum.FAIL)
        {
            return SetFail(msg, code);
        }

        /// <summary>
        /// 设置失败
        /// </summary>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetFail(string msg, StatusCodeEnum code = StatusCodeEnum.FAIL)
        {
            this.SetFail(code);

            this.Msg = msg;
            this.InnerException = new Exception(msg);
            return this;
        }

        /// <summary>
        /// 设置失败
        /// 会重置Data
        /// </summary>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetFail(StatusCodeEnum code = StatusCodeEnum.FAIL)
        {
            this.Code = code;
            this.IsOk = false;
            this.Desc = code.ToString().Replace("_", " ");
            this.Msg = ConstEnum.GetEnumDescription(code);
            return this;
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetException(Exception ex)
        {
            if (ConfigBase.Default.IsException) LogService.Exception(ex);
            this.IsOk = false;
            this.Msg = ex.Message;
            this.InnerException = ex;
            return this;
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="exmsg"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> SetException(string exmsg)
        {
            if (ConfigBase.Default.IsException) LogService.Exception(exmsg);
            this.IsOk = false;
            this.Msg = exmsg;
            return this;
        }

        /// <summary>
        /// 追加信息
        /// </summary>
        /// <param name="v"></param>
        /// <param name="loginAccount"></param>
        /// <param name="loginPassword"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> AppendMsg(string fmt, params string[] args)
        {
            this.Msg += string.Format(" " + fmt, args);
            return this;
        }


        /// <summary>
        /// 从无类型转为强类型
        /// 同类型则转换Data，不同则为Null
        /// </summary>
        /// <param name="res"></param>
        [Author("Linyee", "2018-06-29")]
        public ExecuteResult<T> From(ExecuteResult res)
        {
            if (res == null) return null;
            if (res.Data != null && res.Data?.GetType() == typeof(T)) this.Data = (T)res.Data;
            this.InnerException = res.InnerException;
            this.IsOk = res.IsOk;
            this.Msg = res.Msg;
            return this;
        }


        #endregion

        #region "类型转换"

        /// <summary>
        /// 转弱类型
        /// </summary>
        /// <param name="view"></param>
        [Author("Linyee", "2018-06-29")]
        [Modifier("Linyee","2019-07-06","优化")]
        public static implicit operator ExecuteResult(ExecuteResult<T> view)
        {
            if (view == null) return null;
            var item = new ExecuteResult()
            {
                Code = view.Code,

                Desc = view.Desc,
                Enabled = view.Enabled,
                Msg = view.Msg,
                 
                Page = view.Page,
                Limit = view.Limit,
                Count = view.Count,
                Data = view.Data,
                InnerException = view.InnerException,
            };
            return item;
        }

        /// <summary>
        /// 转主数据
        /// </summary>
        /// <param name="view"></param>
        [Author("Linyee", "2018-06-29")]
        [Modifier("Linyee", "2019-07-06", "优化")]
        public static implicit operator T(ExecuteResult<T> view)
        {
            if (view == null) return default(T);//[Modifier("Linyee","2019-07-06","优化")]
            return view.Data;
        }

        #endregion

        #region "属性"
        /// <summary>
        /// 启用状态
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public bool Enabled { get; private set; } = true;

        /// <summary>
        /// 子数据
        /// </summary>
        [JsonIgnore]
        [Author("Linyee", "2018-06-29")]
        public readonly List<ExecuteResult> Subs = new List<ExecuteResult>();

        /// <summary>
        /// 数据
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public T Data { get; set; }

        /// <summary>
        /// 消息 通常是中文简要描述
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public string Msg { get; set; }

        /// <summary>
        /// 描述 通常是英文简要描述
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public string Desc { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public StatusCodeEnum Code { get; set; } = StatusCodeEnum.Accepted;

        /// <summary>
        /// 记录总数
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public int Count { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 当前页大小
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public int Limit { get; set; } = int.MaxValue;


        /// <summary>
        /// 是否正常
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        [Modifier("Linyee", "2019-07-06", "增加Desc、Msg设置")]
        public bool IsOk {
            get {
                return Code == StatusCodeEnum.OK;
            }
            set {
                if (value) Code = StatusCodeEnum.OK;
                else if (!value && Code == StatusCodeEnum.OK) Code = StatusCodeEnum.FAIL;
                this.Desc = Code.ToString().Replace("_", " ");
                this.Msg = ConstEnum.GetEnumDescription(Code);
            }
        }

        /// <summary>
        /// 内联异常信息
        /// </summary>
        [Author("Linyee", "2018-06-29")]
        public Exception InnerException { get; protected set; }
        #endregion
    }

}
