using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 机器人上下文
    /// </summary>
    [Author("Linyee", "2019-08-07")]
    public class RebotContext
    {
        public RebotContext(string name)
        {
            UserName = name;
        }

        /// <summary>
        /// 用户名称或Id等，最好具有唯一性。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 服务类别
        /// </summary>
        public ServiceTypeCode ServiceType { get; set; } = ServiceTypeCode.综合服务;


        /// <summary>
        /// 表达式动态计算工具
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-08-07")]
        public async Task<ExecuteResult<string>> Exp_Cal_Async(string exp)
        {
            ExecuteResult<string> result = new ExecuteResult<string>();
            //var ccunit = CodeDomExpCal(exp);
            //Console.WriteLine($"{GenerateCode(ccunit, "C#")}");
            //var cr = CompilerCode("C#", ccunit);
            try
            {
                var res = await CSharpScript.EvaluateAsync(exp);
                //var res = Invoke(cr, "Exp_Cal.Exp_Cal", "Cal");
                return result.SetData(res?.ToString()).SetOk();
            }
            catch (Exception ex)
            {
                return result.SetException(ex);
                //return $"~555~~，我的软体出问题了，快帮我联系技术反馈！信息：{ex.Message}";
            }
        }
    }

    /// <summary>
    /// 当前服务类别
    /// </summary>
    [Author("Linyee", "2019-08-07")]
    public enum ServiceTypeCode
    {
        综合服务,//默认为综合服务
        我会计算,//机器人计算服务
        算术练习,//机器人考用户
        闹钟服务,
    }
}
