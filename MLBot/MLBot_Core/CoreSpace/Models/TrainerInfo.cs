using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 训练员信息
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class TrainerInfo: IEntity
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid Id { get; set; }

        /// <summary>
        /// 密码
        /// 为空时 随机生成12-32位且确保三种字符都有
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string Password { get; set; }

        /// <summary>
        /// 训练密码
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string TrainPassword { get; set; }
    }
}
