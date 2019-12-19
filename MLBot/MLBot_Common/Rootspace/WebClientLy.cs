using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace MLBot
{
    /// <summary>
    /// web客户端
    /// </summary>
    [Author("Linyee", "2018-05-05")]
    public class WebClientLy : WebClient
    {
        #region 客户端
        public WebClientLy() { }
        public WebClientLy(X509Certificate2 cer) : this()
        {
            this.cert = cer;
        }

        private X509Certificate2 cert { get; set; }

        public WebClientLy(RemoteCertificateValidationCallback rcvc) : this()
        {
            this.ServerCertificateValidation = rcvc;
        }
        private RemoteCertificateValidationCallback ServerCertificateValidation { get; set; }
        public WebClientLy(X509Certificate2 cer, RemoteCertificateValidationCallback rcvc) : this(cer)
        {
            this.ServerCertificateValidation = rcvc;
        }
        #endregion

        [Author("Linyee", "2018-05-05")]
        static WebClientLy()
        {
            //并发能力
            ServicePointManager.DefaultConnectionLimit = 512;

            //// 解决WebClient不能通过https下载内容问题
            //ServicePointManager.ServerCertificateValidationCallback +=
            //    delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //             System.Security.Cryptography.X509Certificates.X509Chain chain,
            //             System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //    {
            //        return true; // **** Always accept
            //    };
        }

        /// <summary>
        /// cookies
        /// </summary>
        [Author("Linyee", "2018-05-05")]
        public CookieContainer CookieContainer { get; } = new CookieContainer();

        /// <summary>
        /// 超时 毫秒
        /// </summary>
        [Author("Linyee", "2018-05-05")]
        public int Timeout { get; set; } = 10000;

        /// <summary>
        /// 超时 毫秒
        /// </summary>
        [Author("Linyee", "2018-05-05")]
        public IPAddress ipAddress { get; set; } = IPAddress.Any;//网卡上的IP

        /// <summary>
        /// 获取请求对象时
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-05-05")]
        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = base.GetWebRequest(address);
            webRequest.Timeout = Timeout;
            if (webRequest is HttpWebRequest)
            {
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;
                httpRequest.CookieContainer = CookieContainer;

                httpRequest.ServicePoint.BindIPEndPointDelegate += (servicePoint, remoteEndPoint, retryCount) =>
                {
                    return new IPEndPoint(ipAddress, 0);
                };
            }

            return webRequest;
        }
    }
}

