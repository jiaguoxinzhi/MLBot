using Microsoft.AspNetCore.Mvc;

namespace MLBot.Mvc.Areas.MLBot.Controllers
{
    /// <summary>
    /// api 基类
    /// </summary>
    [ApiController]
    [Author("Linyee", "2019-07-05")]
    public class BaseApiController : ControllerBase
    {

        /// <summary>
        /// 运行结果
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        protected ExecuteResult result = new ExecuteResult();
    }
}