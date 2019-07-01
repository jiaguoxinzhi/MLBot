using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot
{
    /// <summary>
    /// 尽量不重复的随机数
    /// Linyee 2018-10-03
    /// </summary>
    [Author("Linyee", "2018-10-03")]
    public class RandomHelper
    {
        [Author("Linyee", "2018-10-03")]
        private static int lstint = 0;
        [Author("Linyee", "2018-10-03")]
        private static long lstlng = 0;

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static int GetRanInt()
        {
            return GetRanInt(0, int.MaxValue);
        }

        /// <summary>
        /// 随机整数
        /// Linyee 2019-04-08
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static int GetRanInt(int min, int max)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int rmax = max - min;
            int cint = lstint;
            while (cint == lstint || cint < min || cint > max)
            {
                cint = ran.Next(min, max);
            }
            return cint;
        }
        /// <summary>
        /// 随机长整数
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static long GetRanLng()
        {
            return GetRanLng(0, long.MaxValue);
        }

        /// <summary>
        /// 随机长整数
        /// Linyee 2019-04-08
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static long GetRanLng(long min, long max)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            #region "最大值"
            int Max2 = 0, Max1 = 0;
            var rmax = max - min;//差值
            if (rmax > int.MaxValue)
            {
                Max1 = int.MaxValue;
                Max2 = (int)(rmax >> 32);
            }
            else
            {
                Max1 = (int)rmax;
                Max2 = 0;
            }
            #endregion
            long clng = lstlng;
            while (clng == lstlng || clng < min || clng > max)
            {
                var i1 = ran.Next(Max1);
                var i2 = ran.Next(Max2);
                clng = ((((long)i2) << 32) | (long)i1) + min;
            }
            lstlng = clng;
            return clng;
        }

        /// <summary>
        /// 随机id
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static Guid GetNewId()
        {
            return Guid.NewGuid();
        }
    }
}
