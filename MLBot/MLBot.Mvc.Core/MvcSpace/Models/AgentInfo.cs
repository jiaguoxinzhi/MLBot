using System;
using System.Collections.Generic;
using System.Text;
using MLBot.Extentions;

namespace MLBot.Mvc
{
    /// <summary>
    /// 代理信息
    /// </summary>
    [Author("Linyee", "2019-07-05")]
    public class AgentInfo:IEntity
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        public string Phone { get; set; }


        /// <summary>
        /// 密码
        /// 为空时 随机生成12-32位且确保三种字符都有
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        public string Password { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [Author("Linyee", "2019-07-06")]
        public Guid Id => new Guid(Phone.ToMd5Bytes());

        /// <summary>
        /// 支付密码或新密码
        /// </summary>
        [Author("Linyee", "2019-07-06")]
        public string PayPassword { get; set; }
    }
}
