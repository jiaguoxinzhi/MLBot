
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Extentions
{
    /// <summary>
    /// 字符串 扩展
    /// Linyee 2018-06-29
    /// </summary>
    [Author("Linyee", "2019-02-01")]
    public static class String_Security_Extentions
    {

        #region "Md5"
        /// <summary>
        /// 转Md5 字节组
        /// </summary>
        /// <param name="str">要md5的串</param>
        /// <param name="len">摘要字节数 输出字符数=字节数*2</param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-05")]
        public static byte[] ToMd5Bytes(this String str)
        {
            if (str == null) return new byte[0];
            if (str == "") return new byte[0];

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

            return data;
        }

        /// <summary>
        /// 转Md5 大写化
        /// </summary>
        /// <param name="str">要md5的串</param>
        /// <param name="len">摘要字节数 输出字符数=字节数*2</param>
        /// <returns></returns>
        [Author("Linyee", "2018-04-21")]
        [Modifier("Linyee", "2019-07-05")]
        public static string ToMd5String(this String str, int len = 16)
        {
            var data = ToMd5Bytes(str);
            if (data.Length < 1) return "";

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < Math.Min(len, data.Length); i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            string val = sBuilder.ToString();
            return val;
        }

        /// <summary>
        /// Md5密码附加串
        /// </summary>
        [Author("Linyee", "2018-04-21")]
        public static string PassAddStr = "Linyee";

        /// <summary>
        /// 转Md5密码串
        /// </summary>
        /// <param name="str">要md5的串</param>
        /// <param name="endstr">附加字符串，默认使用内置的</param>
        /// <param name="stars">开头字符串</param>
        /// <param name="ends">结束字符串</param>
        /// <returns></returns>
        [Author("Linyee", "2018-04-21")]
        public static string ToMd5Password(this string str, string endstr = null, string stars = "", string ends = "")
        {
            return stars + (str + (endstr ?? PassAddStr)).ToMd5String() + ends;
        }
        #endregion

        #region SHA

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        [Author("Linyee", "2019-05-19")]
        public static string SHA1(this string content, Encoding encoding = null)
        {
            byte[] bytes_out = SHA1Raw(content, encoding);
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result;
        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        [Author("Linyee", "2019-05-19")]
        public static string SHA1Base64(this string content, Encoding encoding = null)
        {

            //SHA1 sha1 = new SHA1CryptoServiceProvider();
            //byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(content);
            //byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            //string str_sha1_out = Convert.ToBase64String(bytes_sha1_out);
            //return str_sha1_out;
            byte[] bytes_out = SHA1Raw(content, encoding);
            return Convert.ToBase64String(bytes_out);
        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        [Author("Linyee", "2019-05-19")]
        public static byte[] SHA1Raw(this string content, Encoding encoding = null)
        {
            var encode = Encoding.UTF8;
            if (encoding != null) encode = encoding;

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = encode.GetBytes(content);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            return bytes_out;
        }
        #endregion

    }
}
