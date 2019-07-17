using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 代理会员
    /// </summary>
    [Author("Linyee", "2019-07-05")]
    public class Agency_Member:IEntity
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        public Guid Id { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// 初次随机生成12-32位且确保三种字符都有
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        public string Password { get; set; }

        /// <summary>
        /// 代理会员
        /// </summary>
        /// <param name="agentInfo"></param>
        [Author("Linyee", "2019-07-05")]
        public Agency_Member(AgentInfo agentInfo)
        {
            Util.CopyAFromB(this, agentInfo);
        }

        /// <summary>
        /// 代理会员
        /// </summary>
        /// <param name="agentInfo"></param>
        [Author("Linyee", "2019-07-06")]
        public Agency_Member()
        {
        }
    }
}
