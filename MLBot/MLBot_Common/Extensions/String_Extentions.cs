using MLBot.Attributes;
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
    public static class String_Extentions
    {
        #region 编码
        /// <summary>
        /// base64用到的字符
        /// </summary>
        private static char[] base64CodeArray = new char[]
            {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4',  '5', '6', '7', '8', '9', '+', '/', '='
            };
        /// <summary>
        /// 是否base64字符串
        /// </summary>
        /// <param name="base64Str">要判断的字符串</param>
        /// <returns></returns>
        [Author("Linyee", "2019-05-16")]
        public static bool IsBase64(this string base64Str)
        {
            byte[] bytes;
            return IsBase64(base64Str, out bytes);
        }
        /// <summary>
        /// 是否base64字符串
        /// </summary>
        /// <param name="base64Str">要判断的字符串</param>
        /// <returns></returns>
        [Author("Linyee", "2019-05-16")]
        public static bool IsBase64(this string base64Str, out byte[] bytes)
        {
            //string strRegex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
            bytes = null;
            if (string.IsNullOrEmpty(base64Str)) return false;
            else
            {
                if (base64Str.Contains(",")) base64Str = base64Str.Split(',')[1];
                if (base64Str.Length % 4 != 0) return false;
                if (base64Str.Any(c => !base64CodeArray.Contains(c))) return false;
            }

            try
            {
                bytes = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        /// <summary>
        /// 判断是否乱码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-05-07")]
        public static bool IsLuanMa(this string text, Encoding encoding)
        {
            var enc = Encoding.UTF8;
            if (encoding != null) enc = encoding;
            var bytes = enc.GetBytes(text);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }
        #endregion

        #region "转换"
        /// <summary>
        /// 转Utf8再转单字节编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-04-30")]
        public static string ToISO8859_1(this string text)
        {
            Encoding isoencoding = Encoding.GetEncoding("ISO8859-1");
            var buf = Encoding.UTF8.GetBytes(text);
            return isoencoding.GetString(buf);
        }


        /// <summary>
        /// 转为字节组
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] ToUTF8Bytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
        /// <summary>
        /// 获取字节组
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] GetUTF8Bytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }


        /// <summary>
        /// 转安全显示字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string ToSafeShow(this String str, int l, char c, int r, int cleng = 3)
        {
            var lstr = str.Left(l);
            var rstr = str.Right(r);
            return lstr + new string(c, cleng) + rstr;
        }

        /// <summary>
        /// 转十进制数
        /// Linyee 2018-05-09
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            decimal dec = 0;
            Decimal.TryParse(str, out dec);
            return dec;
        }

        /// <summary>
        /// 转为时间间隔
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string str)
        {
            TimeSpan ts = new TimeSpan();
            TimeSpan.TryParse(str, out ts);
            return ts;
        }
        #endregion

        #region "判断"
        /// <summary>
        /// 判断 指定的字符串是否 null 或 System.String.Empty 字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 判断 指定的字符串是否 有效值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasValue(this string str)
        {
            return str?.Trim().Length > 0;
        }
        #endregion

        #region "截断"
        /// <summary>
        /// 每隔几个字符插入指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="interval"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-05-27")]
        public static string InsertFormat(this string str, int interval, string val)
        {
            for (var fi = interval; fi < str.Length; fi += interval + val.Length)
            {
                str = str.Insert(fi, val);
            }
            return str;
        }


        /// <summary>
        /// 右截
        /// Linyee 2018-05-07
        /// Linyee 2019-04-02 改为中文按两字节算
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string Right(this string str, int lenght)
        {
            if (str == null) return "";
            var buf = Encoding.ASCII.GetBytes(str);
            var blen = buf.Length;
            if (blen <= lenght) return str;
            //return str.Substring(str.Length - lenght);
            //return Encoding.GetEncoding("ISO-8859-1").GetString(buf.TakeLast(lenght).ToArray()) ;
            StringBuilder sbd = new StringBuilder();
            var alen = 0;
            for (var ci = str.Length - 1; ci >= 0; ci--)
            {
                var ch = str[ci];
                if (ch > 255) alen += 2;
                else alen++;
                sbd.Append(ch);
                if (alen >= lenght) break;
            }
            return string.Join("", sbd.ToString().Reverse());
        }

        /// <summary>
        /// 左截
        /// Linyee 2018-05-07
        /// Linyee 2019-04-02 改为中文按两字节算
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string Left(this string str, int lenght)
        {
            //return str.Substring(0, lenght);
            if (str == null) return string.Empty;
            var buf = Encoding.ASCII.GetBytes(str);
            var blen = buf.Length;
            if (blen <= lenght) return str;
            //return str.Substring(str.Length - lenght);
            //return Encoding.GetEncoding("ISO-8859-1").GetString(buf.TakeLast(lenght).ToArray()) ;
            StringBuilder sbd = new StringBuilder();
            var alen = 0;
            for (var ci = 0; ci < str.Length; ci++)
            {
                var ch = str[ci];
                if (ch > 255) alen += 2;
                else alen++;
                sbd.Append(ch);
                if (alen >= lenght) break;
            }
            return sbd.ToString();
        }
        #endregion

        #region "Md5"
        ///// <summary>
        ///// 转Md5 大写化
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string ToMd5String(this String str)
        //{
        //    MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        //    byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

        //    StringBuilder sBuilder = new StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("X2"));
        //    }
        //    string val = sBuilder.ToString();
        //    return val;
        //}
        /// <summary>
        /// 转Md5 大写化
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="stars"></param>
        /// <param name="ends"></param>
        /// <returns></returns>
        public static string ToMd5String(this String str, int len = 16, string stars = "", string ends = "")
        {
            if (str == null) return "";
            if (str == "") return "";

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < Math.Min(len, data.Length); i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            string val = stars + sBuilder.ToString() + ends;
            return val;
        }

        //Md5密码附加串
        private static string sp_PassAddStr = "percode";

        /// <summary>
        /// 转Md5密码串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5Password(this string str)
        {
            return (str + sp_PassAddStr).ToMd5String();
        }
        #endregion

        #region SHA

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        [Author("Linyee","2019-05-19")]
        public static string SHA1(this string content, Encoding encoding=null)
        {
            byte[] bytes_out = SHA1Raw(content,encoding);
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
