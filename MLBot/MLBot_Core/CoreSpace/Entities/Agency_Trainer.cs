using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 训练员
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class Agency_Trainer : IEntity
    {
        /// <summary>
        /// 训练员
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Agency_Trainer()
        {
        }

        /// <summary>
        /// 训练员
        /// </summary>
        /// <param name="trainer"></param>
        [Author("Linyee", "2019-07-16")]
        public Agency_Trainer(TrainerInfo trainer)
        {
            Util.CopyAFromB(this, trainer);
        }

        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid Id { get; set; }

        /// <summary>
        /// 训练密码
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string TrainPassword { get; set; }
    }
}
