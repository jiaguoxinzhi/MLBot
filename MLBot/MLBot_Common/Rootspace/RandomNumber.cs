using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MLBot
{

    /// <summary>
    /// 随机整数工具
    /// </summary>
    [Author("Linyee", "2018-10-03")]
    public static class RandomNumber
    {
        /// <summary>
        /// 强随机整数
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static int GetRndInt(int minValue=0, int maxValue=int.MaxValue)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();//生成字节数组
            int iRoot = BitConverter.ToInt32(buffer, 0);//利用BitConvert方法把字节数组转换为整数
            Random rdmNum = new Random(iRoot);//以这个生成的整数为种子
            return rdmNum.Next(minValue, maxValue);
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2018-10-03")]
        public static long GetRndLong(long minValue = 0, long maxValue = long.MaxValue)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();//生成字节数组
            int iRoot = BitConverter.ToInt32(buffer, 0);//利用BitConvert方法把字节数组转换为整数
            Random rdmNum = new Random(iRoot);//以这个生成的整数为种子
            return (long)(rdmNum.NextDouble() * (maxValue-minValue)+ minValue);
        }
    }
}
