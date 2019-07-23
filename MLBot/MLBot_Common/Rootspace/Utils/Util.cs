
using MLBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MLBot
{
    /// <summary>
    /// 通用功能类
    /// Linyee
    /// </summary>
    [Author("Linyee", "2019-03-07")]
    public static class Util
    {

        /// <summary>
        /// A从B复制
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-03-07")]
        public static object CopyAFromB(object a, object b)
        {
            if (b == null) return a;
            if (a == null) return a;

            var btype = b.GetType();
            foreach (var ap in a.GetType().GetProperties())
            {
                var bp = btype.GetProperty(ap.Name);
                if (bp == null) continue;
                if (bp.GetGetMethod() == null || bp.GetGetMethod().IsPrivate) continue;
                if (ap.GetSetMethod() == null || ap.GetSetMethod().IsPrivate) continue;
                ap.SetValue(a, bp.GetValue(b));
            }
            return a;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="view"></param>
        /// <param name="item"></param>
        /// <param name="skip"></param>
        [Author("Linyee", "2019-03-07")]
        public static T CopyAtoB<T>(T view, ref T item, object skip)
        {
            if (view == null) throw new Exception("传入的源数据不能为空");
            string[] skips = null;
            if (skip != null)
            {
                skips = skip.GetType().GetProperties().Select(p => p.Name).ToArray();
            }

            return CopyAtoB(view, ref item, CopyTypeEnum.全部, skips);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="view"></param>
        /// <param name="item"></param>
        /// <param name="copyType"></param>
        /// <param name="skips"></param>
        [Author("Linyee", "2019-03-07")]
        public static T CopyAtoB<T>(T view, ref T item, CopyTypeEnum copyType, string[] skips)
        {
            if (view == null) return item;
            if (item == null) return item;

            var viewType = view.GetType();
            var itemType = item.GetType();
            foreach (var p in itemType.GetProperties())
            {
                if (p == null) continue;
                if (p.GetSetMethod() == null || p.GetSetMethod().IsPrivate) continue;//跳过无设置方法

                if (skips != null && skips.Contains(p.Name, StringComparer.OrdinalIgnoreCase)) continue;

                var sp = viewType.GetProperty(p.Name);
                if (sp == null) continue;
                if (p.PropertyType != sp.PropertyType) continue;
                var val = sp.GetValue(view);

                //跳过空串 用于部分更新
                if (copyType == CopyTypeEnum.跳过空值 && val == null)
                {
                    continue;
                }

                p.SetValue(item, val);
            }

            foreach (var p in itemType.GetFields())
            {
                if (p == null) continue;
                if (p.IsPrivate) continue;//跳过私有

                if (skips != null && skips.Contains(p.Name, StringComparer.OrdinalIgnoreCase)) continue;

                var sp = viewType.GetField(p.Name);
                if (sp == null) continue;
                if (p.FieldType != sp.FieldType) continue;
                var val = sp.GetValue(view);

                //跳过空串 用于部分更新
                if (copyType == CopyTypeEnum.跳过空值 && val == null)
                {
                    continue;
                }

                p.SetValue(item, val);
            }


            return item;
        }


        /// <summary>
        /// 获取ipv4的值
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-03-07")]
        public static long GetIpNum(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || ip.IndexOf(".") <= 0)
            {
                return 0L;
            }

            var list = ip.Split('.');
            if (list.Length < 4)
            {
                throw new Exception("ip 地址不正确");
            }
            return long.Parse(list[0]) * 256 * 256 * 256 + long.Parse(list[1]) * 256 * 256 + long.Parse(list[2]) * 256 + long.Parse(list[3]);
        }


        /// <summary>
        /// 任意Ip 字符串
        /// </summary>
        [Author("Linyee", "2019-03-07")]
        public static string AnyIpV4 = "0.0.0.0";

        ///// <summary>
        ///// 获取IP信息
        ///// </summary>
        ///// <returns></returns>
        //public static string GetIP()
        //{
        //    try
        //    {
        //        string result = HttpContext.Current.Request.Headers.Get("HTTP_X_FORWARDED_FOR");
        //        if (string.IsNullOrEmpty(result))
        //            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //        if (string.IsNullOrEmpty(result))
        //            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        if (string.IsNullOrEmpty(result))
        //            result = HttpContext.Current.Request.UserHostAddress;
        //        if (string.IsNullOrEmpty(result))
        //            return "127.0.0.1";

        //        if (result == "::1")
        //            return "127.0.0.1";

        //        return result;
        //    }
        //    catch
        //    {

        //    }
        //    return "127.0.0.1";
        //}

        /// <summary>
        /// 是否内部IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-03-07")]
        public static bool IsLanIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || ip.StartsWith("127.") || ip.StartsWith("10.") || ip.StartsWith("192.168") || ip.StartsWith("169.254.") || ip == "191.255.255.255" || ip == "0.0.0.0")
            {
                return true;
            }
            else if (ip.StartsWith("172."))
            {
                var ip1 = int.Parse("0" + ip.Split('.')[1]);
                if (ip1 >= 16 && ip1 <= 31)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// decimal > 0。空返回false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-03-07")]
        public static bool IsDecimalGreaterThanZero(string str)
        {
            if (str == String.Empty)
                return false;
            try
            {
                decimal tmp = Decimal.Parse(str);
                if (tmp > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }

    }
}
