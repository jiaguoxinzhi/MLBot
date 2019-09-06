using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// 微信客服
    /// </summary>
    [Author("Linyee", "2019-08-16")]
    public sealed partial class WechatCustomService : IHostedService, IDisposable
    {

        private static Dictionary<Guid, WechatCustomService> wcss = new Dictionary<Guid, WechatCustomService>();

        public static readonly WechatCustomService Default = new WechatCustomService();

        private Guid id;
        private Queue<Func<bool>> actions = new Queue<Func<bool>>();
        public WechatCustomService()
        {
            id = Guid.NewGuid();
            wcss.Add(id, this);
        }

        /// <summary>
        /// 添加任务
        /// 5秒判断一次
        /// </summary>
        /// <param name="func">如果正确执行请返回true 返回false会一直执行到true，请谨慎检查 false 分支要尽量快</param>
        public void Enqueue(Func<bool> func)
        {
            actions.Enqueue(func);
        }
        #region IDisposable
        /// <summary>
        /// 释放数据
        /// </summary>
        [Author("Linyee", "2019-08-09")]
        public void Dispose()
        {
            CheckService();
            timer?.Dispose();
            wcss.Remove(id);
        }
        #endregion

        #region IHostedService 自动保存服务
        private Timer timer = null;
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-19")]
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-19")]
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            foreach (var wcs in wcss.Values)
            {
                wcs.Dispose();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 运行服务
        /// 5秒判断一次
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-08-09")]
        private Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (timer != null) return Task.CompletedTask;

            timer = new Timer((obj) => {
                foreach (var wcs in wcss.Values)
                {
                    wcs.CheckService();
                }
            }, null, 5 * 1000, 5 * 1000);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 检查服务
        /// </summary>
        public void CheckService()
        {
            var tactions = new Queue<Func<bool>>();
            while (actions.Count > 0)
            {
                var ac = actions.Dequeue();
                try
                {
                    var b = ac.Invoke();
                    if (!b) tactions.Enqueue(ac);
                }
                catch (Exception ex)
                {
                    LogService.Exception(ex);
                }
            }

            foreach (var ac in tactions)
            {
                actions.Enqueue(ac);
            }
        }
        #endregion
    }
}
