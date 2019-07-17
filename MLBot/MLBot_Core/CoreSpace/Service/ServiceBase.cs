using Microsoft.Extensions.Configuration;
using MLBot.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 基础 服务
    /// 请勿用 AddSingleton 注入 会导致result信息与之前的重叠
    /// </summary>
    [Author("Linyee", "2019-07-05")]
    public class ServiceBase<T>: IServiceBase<T>
        where T: IEntity
    {
        /// <summary>
        /// 泛型解析后的名称
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        private string[] tnames;

        [Author("Linyee", "2019-07-05")]
        public ServiceBase()
        {
            var ttype = typeof(T);
            var tname = ttype.Name;
            tnames = tname.Split('_');

            AgencyPath = Path.Combine(Environment.CurrentDirectory, "App_Data", tnames[0]);
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        ExecuteResult<T> result = new ExecuteResult<T>();

        /// <summary>
        /// 代理目录
        /// </summary>
        [Author("Linyee", "2019-07-05")]
        private string AgencyPath ;

        [Author("Linyee", "2019-07-05")]
        public ExecuteResult<T> Add(T amem)
        {
            var dirname = amem.Id.ToString("N");
            var jdir = Path.Combine(AgencyPath, dirname);
            if (!Directory.Exists(jdir)) Directory.CreateDirectory(jdir);

            var jfile = Path.Combine(jdir, $"{tnames[1]}.json");
            if (!File.Exists(jfile))
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(tnames[1],amem);
                File.WriteAllText(jfile, dict.ToJsonString());
                return result.SetData(amem).SetOk("添加成功");
            }
            else
            {
                return result.SetFail($"已存在{dirname}/{tnames[1]}");
            }
        }

        [Author("Linyee", "2019-07-05")]
        public ExecuteResult<T> Delete(T amem)
        {
            var dirname = amem.Id.ToString("N");
            var dirs = Directory.GetDirectories(AgencyPath);
            var jdir = Path.Combine(AgencyPath, dirname);
            var jfile = Path.Combine(jdir, $"{tnames[1]}.json");
            if (File.Exists(jfile))
            {
                File.Delete(jfile);
                return result.SetData(amem).SetOk("删除成功");
            }
            else
            {
                return result.SetFail($"不存在{dirname}/{tnames[1]}");
            }
        }

        [Author("Linyee", "2019-07-05")]
        public ExecuteResult<T> Find(Guid id)
        {
            var dirname = id.ToString("N");
            var jdir = Path.Combine(AgencyPath, dirname);
            var jfile = Path.Combine(jdir, $"{tnames[1]}.json");
            if (File.Exists(jfile))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(jfile)
                    .Build();
                var amember = config.GetSection(tnames[1]).Get<T>();
                return result.SetData(amember).SetOk();
            }
            else
            {
                return result.SetFail($"不存在{dirname}/{tnames[1]}");
            }
        }

        [Author("Linyee", "2019-07-05")]
        public ExecuteResult<T> Update(T amem)
        {
            var dirname = amem.Id.ToString("N");
            var jdir = Path.Combine(AgencyPath, dirname);
            var jfile= Path.Combine(jdir, $"{tnames[1]}.json");
            if (File.Exists(jfile))
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(tnames[1], amem);
                File.WriteAllText(jfile, dict.ToJsonString());
                return result.SetData(amem).SetOk("更新成功");
            }
            else
            {
                return result.SetFail($"不存在{dirname}/{tnames[1]}");
            }
        }
    }
}
