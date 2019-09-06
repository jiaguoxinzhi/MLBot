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
