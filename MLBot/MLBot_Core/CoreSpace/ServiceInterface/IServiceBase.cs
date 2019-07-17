using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// Agency_Member 服务
    /// </summary>
    [Author("Linyee", "2019-07-05")]
    public interface IServiceBase<T>
        where T: IEntity
    {
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-05")]
        ExecuteResult<T> Find(Guid id);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="amember"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-05")]
        ExecuteResult<T> Add(T amember);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="amember"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-05")]
        ExecuteResult<T> Update(T amember);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="amember"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-05")]
        ExecuteResult<T> Delete(T amember);
    }
}
