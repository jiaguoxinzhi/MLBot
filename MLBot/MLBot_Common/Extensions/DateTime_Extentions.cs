using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Extentions
{
    /// <summary>
    /// 时间日期
    /// </summary>
    [Author("Linyee","2018-05-05")]
    public static class DateTime_Extentions
    {
        /// <summary>
        /// 10位时间戳
        /// 从1970-1-1开始 秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-05-05")]
        public static UInt32 GetTimestamp10(this DateTime dt)
        {
            return (UInt32)(dt - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        /// <summary>
        /// 13位时间戳
        /// 从1970-1-1开始毫秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-05-05")]
        public static long GetTimestamp(this DateTime dt)
        {
            return (long)(dt - new DateTime(1970, 1, 1).ToLocalTime()).TotalMilliseconds;
        }

        /// <summary>
        /// 从13位时间戳获取时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ts"></param>
        /// <returns></returns>

        [Author("Linyee", "2018-05-05")]
        public static DateTime TimestampToDateTime(this long ts)
        {
            DateTime dt=DateTime.MinValue;
            var tsstr = ts.ToString();
            if (tsstr.Length == 13)
            {
                dt = new DateTime(1970, 1, 1).AddMilliseconds(ts);
            }
            else if (tsstr.Length == 10)
            {
                dt = new DateTime(1970, 1, 1).AddSeconds(ts);
            }
            else
            {
                throw new Exception("不是有效时间戳");
            }
            return dt.ToLocalTime();
        }

        /// <summary>
        /// 从13位时间戳获取时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ts"></param>
        /// <returns></returns>

        [Author("Linyee", "2018-05-05")]
        public static DateTime GetFromTimestamp(this DateTime dt,long ts)
        {
            var tsstr = ts.ToString();

            if (tsstr.Length == 13)
            {
                dt = new DateTime(1970, 1, 1).AddMilliseconds(ts);
            }
            else if (tsstr.Length == 10)
            {
                dt = new DateTime(1970, 1, 1).AddSeconds(ts);
            }
            else
            {
                    throw new Exception("不是有效时间戳");
            }
            return dt;
        }
    }
}
