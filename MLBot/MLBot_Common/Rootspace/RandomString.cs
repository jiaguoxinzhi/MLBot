
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
        /// <summary>
        /// 小写字母
        /// </summary>
        [Author("Linyee", "2018-10-03")]
        private const string sCharLow = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 大定字母
        /// </summary>
        [Author("Linyee", "2018-10-03")]
        private const string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 数字
        /// </summary>
        [Author("Linyee", "2018-10-03")]
        private const string sNumber = "0123456789";
        /// <summary>
        /// 特殊符号
        /// </summary>
        [Author("Linyee", "2018-07-05")]
        private const string sSpecial = "`=/*-+,.;'\\[]~!@#$%^&()_{}:\"|<>?";

        /// <summary>
        /// 小写字母
        /// </summary>
        [Author("Linyee", "2018-10-03")]
        private const string pwdCharLow = "abcdefghijkmnopqrstuvwxyz";//无l
        /// <summary>
        /// 大定字母
        /// </summary>
        [Author("Linyee", "2018-10-03")]
        private const string pwdCharUpp = "ABCDEFGHJKLMNPQRSTUVWXYZ";//无OI
        /// <summary>
        /// 特殊符号
        /// </summary>
        [Author("Linyee", "2018-07-05")]
        private const string pwdSpecial = "`=/*-+\\%<>|&^?,.()'[]~{}!@#$_\"";//无;:

        /// <summary>
        /// 随机密码串
        /// 确保 数字+大小写字母+特殊符号
        /// </summary>
        /// <param name="strLen"></param>
        /// <param name="StrOf"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-07-05")]
        public static string BuildAutoRndPwdString(int minLen = 8, int maxLen = 32)
        {
            var strLen = RandomNumber.GetRndInt(minLen, maxLen);
            var spelen = RandomNumber.GetRndInt(1, Math.Max(strLen / 3, 3));
            var numlen = RandomNumber.GetRndInt(1, strLen / 3);
            var uprlen = RandomNumber.GetRndInt(1, strLen / 3);
            var lowlen = strLen - spelen - numlen;
            var spstr = BuildRndPwdString(spelen, sSpecial);
            var nmstr = BuildRndPwdString(numlen, sNumber);
            var upstr = BuildRndPwdString(uprlen, pwdCharUpp);
            var lwstr = BuildRndPwdString(lowlen, pwdCharLow);
            return new string(new StringBuilder().Append(spstr).Append(nmstr).Append(upstr).Append(lwstr).ToString().OrderBy(c => RandomNumber.GetNewSeed()).ToArray());
        }

        /// <summary>
        /// 随机密码串
        /// </summary>
        /// <param name="strLen"></param>
        /// <param name="StrOf"></param>
        /// <returns></returns>
        [Author("Linyee", "2018-07-05")]
        public static string BuildRndPwdString(int strLen, string StrOf = sCharLow + sCharUpp + sNumber + sSpecial)
        {
            return BuildRndString(strLen, StrOf);
        }

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
