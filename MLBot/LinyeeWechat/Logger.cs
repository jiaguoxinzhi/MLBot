using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLBot.Mvc.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Mvc.Servers
{

    /// <summary>
    /// 日志记录器
    /// </summary>
    [Author("Linyee", "2019-01-21")]
    public class Logger
    {
        #region "构造"

        /// <summary>
        /// 创建默认中间件
        /// app.use 模式使用
        /// </summary>
        public Logger()
        {
        }

        /// <summary>
        /// 虚拟机
        /// </summary>
        private IHostingEnvironment _evn;

        /// <summary>
        /// 创建默认中间件
        /// app.use 模式使用
        /// </summary>
        public Logger(RequestDelegate next, IConfiguration configuration, IHostingEnvironment env) : this(next, configuration)
        {
            _evn = env;
        }

        /// <summary>
        /// 配置工具
        /// </summary>
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 创建默认中间件
        /// app.use 模式使用
        /// </summary>
        public Logger(RequestDelegate next, IConfiguration configuration) : this(next)
        {
            Configuration = configuration;
        }

        private readonly RequestDelegate _next;

        /// <summary>
        /// 创建入口
        /// </summary>
        /// <param name="next"></param>
        public Logger(RequestDelegate next) : this()
        {
            _next = next;
        }
        #endregion

        #region "调用入口"
        /// <summary>
        /// 
        /// </summary>
        private static long RequestCount = 0;
        private static object RequestCountLock = new object();
        private static string[] nologurls = new string[] { "http://pg.txwm.cn/" };


        /// <summary>
        /// 调用入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Modifier("Linyee", "2019-06-20","改为调试信息")]
        public async Task InvokeAsync(HttpContext context)
        {
            LogService.AnyLog("MLBot", "=====Logger===InvokeAsync=====");
            var url = context.Request.GetAbsoluteUri();

            LogService.AnyLog("MLBot", "请求地址：" + url);
            LogService.AnyLog("MLBot", "头部信息：" + string.Join("\r\n", context.Request.Headers));

            if (!nologurls.Contains(url))
            {
                LogService.AnyLog("MLBot", "内容主体：" + context.Request.GetBodyString());
            }

            LogService.AnyLog("MLBot", "=====Logger===Out=====");
            if (_next != null)
            {
                await _next.Invoke(context);
            }
            return;
        }
        #endregion
    }
}
