
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Enums
{
    /// <summary>
    /// 实体状态
    /// </summary>
    [Author("Linyee", "2019-02-18")]
    public enum EntityState
    {
        /// <summary>
        /// 未变更
        /// </summary>
        Unchanged,
        /// <summary>
        /// 新增的
        /// </summary>
        Added,
        /// <summary>
        /// 修改的
        /// </summary>
        Modified,

        /// <summary>
        /// 删除项，小于此可视为有效数据
        /// </summary>
        Deleted=127,
    }
}
