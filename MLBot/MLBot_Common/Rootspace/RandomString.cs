using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MLBot
{
    /// <summary>
    /// 随机字符串工具
    /// </summary>
    [Author("Linyee", "2018-10-03")]
    public static class RandomString
    {
        [Author("Linyee", "2018-10-03")]
        private const string sCharLow = "abcdefghijklmnopqrstuvwxyz";
        [Author("Linyee", "2018-10-03")]
        private const string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [Author("Linyee", "2018-10-03")]
        private const string sNumber = "0123456789";

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="strLen"></param>
        /// <param name="StrOf"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static string BuildRndString(int strLen, string StrOf = sCharLow + sCharUpp + sNumber)
        {
            if (StrOf == null) StrOf = sCharLow + sCharUpp + sNumber;

            System.Random RandomObj = new System.Random(RandomNumber.GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < strLen; i++)
            {
                buildRndCodeReturn += StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
            }
            return buildRndCodeReturn;
        }

        /// <summary>
        /// 多选一
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static string SelOnce(string[] sources)
        {
            var ind = RandomHelper.GetRanLng(0, sources.LongLength - 1);
            return sources[ind];
        }
    }
}
