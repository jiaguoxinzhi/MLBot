using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLBot.Mvc.Extentions
{
    /// <summary>
    /// 获取Url扩展类
    /// </summary>
    [Author("Linyee", "2019-04-17")]
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// 获取一个请求流副本
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-06-20")]
        public static MemoryStream GetMemoryStream(this HttpRequest Request)
        {
            Request.EnableRewind();
            if (Request.Body != null && Request.Body.CanSeek)
            {
                var mem = new MemoryStream();
                {
                    try
                    {
                        Request.Body.Seek(0, SeekOrigin.Begin);
                        Request.Body.CopyTo(mem);//有时会抛出 Unexpected end of request content.
                    }catch(Exception ex)
                    {
                        LogService.AnyLog("WarningException",ex.ToString());
                    }
                    finally
                    {
                        mem.Seek(0, SeekOrigin.Begin);//还原到初始位置
                        Request.Body.Seek(0, SeekOrigin.Begin);//还原到初始位置
                    }
                }
                return mem;
            }
            else
            {
                throw new System.Exception("无请求流或无法倒流");
            }
        }

        /// <summary>
        /// 获取请求流的字节组
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-09-06")]
        public static string GetBodyString(this HttpRequest Request)
        {
            return new StreamReader(Request.GetMemoryStream()).ReadToEnd();
        }

        /// <summary>
        /// 获取整个地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        /// <summary>
        /// 获取网站根地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string GetUrlRoot(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString();
        }

        /// <summary>
        /// 获取请求路径
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string GetPath(this HttpRequest request)
        {
            return request.Path;
        }

        /// <summary>
        /// 转Json
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string ToJson(this object t)
        {
            return JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }

        /// <summary>
        /// 获取远端IP
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-17")]
        public static string GetIP(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                ip = ip.Split(',')[0].Split(':')[0];//如果存在多ip则只用第一个 存在端口号，则去掉 Linyee 2019-06-28
            }
            return ip;
        }
    }
}
