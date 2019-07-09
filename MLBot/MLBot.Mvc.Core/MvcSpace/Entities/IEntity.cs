using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc
{
    /// <summary>
    /// 实体接口
    /// </summary>
    [Author("Linyee", "2019-07-06")]
    public interface IEntity
    {
        /// <summary>
        /// 唯一id
        /// 主键
        /// </summary>
        [Author("Linyee", "2019-07-06")]
        Guid Id { get; }

    }
}
