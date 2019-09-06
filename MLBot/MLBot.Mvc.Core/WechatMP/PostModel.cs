using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Mvc.WechatMP
{
    /// <summary>
    /// Post信息
    /// </summary>
    [Author("Linyee", "2019-08-16")]
    public class PostModel
    {
        public string AppId
        {
            get;
            set;
        }

        public string DomainId
        {
            get
            {
                return this.AppId;
            }
            set
            {
                this.AppId = value;
            }
        }


        public string EncodingAESKey
        {
            get;
            set;
        }

        public string Msg_Signature
        {
            get;
            set;
        }

        public string Nonce
        {
            get;
            set;
        }

        public string Signature
        {
            get;
            set;
        }

        public string Timestamp
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public void SetSecretInfo(string token, string encodingAESKey)
        {
            this.Token = token;
            this.EncodingAESKey = encodingAESKey;
        }
        public void SetSecretInfo(string token, string encodingAESKey, string appId)
        {
            SetSecretInfo(token, encodingAESKey);
            this.AppId = appId;
        }
    }
}
