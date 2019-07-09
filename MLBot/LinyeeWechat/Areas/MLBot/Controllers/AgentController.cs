using Microsoft.AspNetCore.Mvc;
using System;
using MLBot.Extentions;

namespace MLBot.Mvc.Areas.MLBot.Controllers
{
    /// <summary>
    /// 代理接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Author("Linyee", "2019-07-05")]
    public class AgentController : BaseApiController
    {
        /// <summary>
        /// 代理会员服务
        /// </summary>
        private IAgency_Member_Service amember_service;

        /// <summary>
        /// 注入 代理接口
        /// </summary>
        /// <param name="agency_Member_Service">代理会员服务</param>
        [Author("Linyee", "2019-07-05")]
        public AgentController(IAgency_Member_Service agency_Member_Service)
        {
            this.amember_service = agency_Member_Service;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult Get(Guid id)
        {
            var item= amember_service.Find(id);
            if (item.Data != null) item.Data.Password = "";
            return item;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/[controller]/GetString/{id}")]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult GetString(string id)
        {
            var item= amember_service.Find(new AgentInfo() { Phone=id}.Id);
            if (item.Data != null) item.Data.Password = "";
            return item;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult Post([FromBody] AgentInfo agent)
        {
            Agency_Member amember = new Agency_Member(agent);
            bool randpass = false;
            if (string.IsNullOrEmpty(amember.Password)) {
                amember.Password = RandomString.BuildAutoRndPwdString();
                randpass = true;
            }
            var pass = amember.Password;
            amember.Password = amember.Password.ToMd5Password();//保存摘要 非明文

            var res = amember_service.Add(amember);
            //消息处理
            if (res.IsOk)
            {
                res.Msg += ",注册成功";
                if (randpass) {
                    res.Data.Password = pass;//返回明文
                    res.Msg += ",请您收好密码，服务端不保存密码明文";
                }
                else
                {
                    res.Data.Password = "";//返回明文
                    res.Msg += ",请记住您所设的密码，服务端不保存密码明文";
                }
            }
            return res;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult Put([FromBody] AgentInfo agent)
        {
            Agency_Member amember = amember_service.Find(agent.Id);
            if (amember == null) return result.SetFail($"未找到此代理会员{agent.Phone}");
            if (agent.Password.ToMd5Password()!= amember.Password) return result.SetFail($"操作密码不正确");

            Util.CopyAFromB(amember, agent);//更新数据
            var res = amember_service.Update(amember);
            return result.Set(res);
        }

        /// <summary>
        /// 更新操作密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("/api/[controller]/PutNewPwd")]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult PutNewPwd ([FromBody] AgentInfo agent)
        {
            Agency_Member amember = amember_service.Find(agent.Id);
            if (amember == null) return result.SetFail($"未找到此代理会员{agent.Phone}");
            if (agent.Password.ToMd5Password() != amember.Password) return result.SetFail($"操作密码不正确");

            bool randpass = false;
            if (string.IsNullOrEmpty(agent.PayPassword))
            {
                amember.Password = RandomString.BuildAutoRndPwdString();
                randpass = true;
            }
            else
            {
                agent.Password = agent.PayPassword;
            }
            var pass = amember.Password;
            amember.Password = amember.Password.ToMd5Password();
            var res = amember_service.Update(amember);
            //消息处理
            if (res.IsOk)
            {
                if (randpass)
                {
                    res.Msg += ",重置密码成功";
                    res.Data.Password = pass;//返回明文
                    res.Msg += ",请您收好您的新密码，服务端不保存密码明文";
                }
                else
                {
                    res.Data.Password = "";//返回明文
                    res.Msg += ",修改密码成功";
                    res.Msg += ",请记住您所设的新密码，服务端不保存密码明文";
                }
            }
            return result.Set(res);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Author("Linyee", "2019-07-05")]
        public ExecuteResult Delete([FromBody] AgentInfo agent)
        {
            Agency_Member amember = amember_service.Find(agent.Id);
            if (amember == null) return result.SetFail($"未找到此代理会员{agent.Phone}");
            if (agent.Password.ToMd5Password() != amember.Password) return result.SetFail($"操作密码不正确");

            var res = amember_service.Delete(amember);
            return result.Set(res);
        }
    }
}