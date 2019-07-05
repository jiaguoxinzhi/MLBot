using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MLBot.Wechat
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // options.CheckConsentNeeded = context => true;
                //options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(optsions =>
                {
                    optsions.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            // 设置 Session 过期时间
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseMiddleware(typeof(ErrorHandlingMiddleware));


            //app.UseWebSockets();
            //app.UseMiddleware(typeof(WebSocketMiddleware));

            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseStaticFiles(new StaticFileOptions
            {
                //FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
                //设置不限制content-type 该设置可以下载所有类型的文件，但是不建议这么设置，因为不安全
                //ServeUnknownFileTypes = true
                //下面设置可以下载apk和nupkg类型的文件
                ContentTypeProvider = new FileExtensionContentTypeProvider(
                   new Dictionary<string, string>
                   {
                        { ".apk","application/vnd.android.package-archive"},
                        { ".ipa","application/octet-stream.ipa"},
                        { ".nupkg","application/zip"}

                   })
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
