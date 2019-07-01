using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MLBot.Extentions
{
    /// <summary>
    /// 二进制数据的扩展
    /// Linyee
    /// </summary>
    [Author("Linyee", "2019-02-01")]
    public static class Byte_Extentions
    {

        /// <summary>
        /// 比较两个字节组是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsBinary(this byte[] a,byte[] b)
        {
            if (a == null && b != null) return false;
            if (a != null && b == null) return false;
            if (a == null && b == null) return true;

            if (a.LongLength != b.LongLength) return false;
            for(var fi = 0L; fi < a.LongLength; fi++)
            {
                if (a[fi] != b[fi]) return false;
            }
            return true;
        }

        /// <summary>
        /// 十六进串转数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] BytesFromHexString(this string hex)
        {
            StringBuilder hexbd = new StringBuilder();
            hexbd.Append(hex);
            hexbd.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            if (hexbd.Length % 2 != 0) throw new Exception("不是一个有效的十六进串");
            var hexstr = hexbd.ToString();
            List<byte> list = new List<byte>();
            for(var spi = 0; spi < hexstr.Length; spi += 2)
            {
                var bhex = hexstr.Substring(spi, 2);
                byte b = byte.Parse(bhex, NumberStyles.HexNumber);
                list.Add(b);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 转16进 字符串
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [Modifier("Linyee","2019-05-17","优化asc码")]
        public static string ToHexString(this byte[] buf, long start = 0, long len = -1,int lineBytes=32,bool isoutcahr=true)
        {
            var tlen = len;
            if (tlen < 0) tlen = buf.Length;
            StringBuilder sbd = new StringBuilder();
            StringBuilder cbd = new StringBuilder();
            for (var fi = start; fi < start + tlen; fi++)
            {
                if (isoutcahr)
                {
                    cbd.Append(Regex.Replace(((char)buf[fi]).ToString(), "[\x00-\x1F\x7F-\xFF]+", "?"));
                }
                sbd.Append(buf[fi].ToString("X2"));
                sbd.Append(" ");
                if ((fi + 1) % 8 == 0) {
                    if (isoutcahr) cbd.Append("\t");
                    sbd.Append("\t"); //八字节一小组
                }
                if ((fi + 1) % lineBytes == 0)
                {
                    if (isoutcahr) {
                        sbd.Append("\t").Append(cbd.ToString());
                        cbd.Clear();
                    }
                    sbd.AppendLine();//32字节一行
                }
            }
            //最后的输出
            if (isoutcahr)
            {
                sbd.Append("\t").Append(cbd.ToString());
                cbd.Clear();
            }
            sbd.AppendLine();//32字节一行

            if (sbd.Length >= lineBytes) sbd.Insert(0, "\r\n");//开始处插入换行
            var str = sbd.ToString();
            sbd.Clear();
            sbd = null;
            return str;
        }


        /// <summary>
        /// 转16进 字符串
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ToHexString(Stream stream, int start, int len )
        {
            StringBuilder sbd = new StringBuilder();
            for (var fi = start; fi < start + len; fi++)
            {
                stream.Position = start;
                sbd.Append(stream.ReadByte().ToString("X2") + " ");
                if ((fi + 1) % 32 == 0) sbd.AppendLine();
            }
            var str = sbd.ToString();
            sbd.Clear();
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static uint ToUInt32(Stream stream)
        {
            return BitConverter.ToUInt32(GetBytes(stream, 4), 0);
        }

        /// <summary>
        /// 从流中读取uint32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static uint ToUInt32(Stream stream, ref long offset)
        {
            var res = BitConverter.ToUInt32(GetBytes(stream, ref offset, 4), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取uint32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static uint ToUInt32Big(Stream stream, ref long offset,int len=4)
        {
            var ubytes = GetBytes(stream, ref offset, len).Reverse().ToArray();
            List<byte> list = new List<byte>();
            list.AddRange(ubytes);
            list.AddRange(new byte[] { 0,0,0,0});
            var res = BitConverter.ToUInt32(list.ToArray(), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取uint32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static int ToInt32Big(Stream stream, ref long offset, int len = 4)
        {
            var ubytes = GetBytes(stream, ref offset, len).Reverse().ToArray();
            List<byte> list = new List<byte>();
            list.AddRange(ubytes);
            list.AddRange(new byte[] { 0, 0, 0, 0 });
            var res = BitConverter.ToInt32(list.ToArray(), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取uint32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ulong ToUInt64(Stream stream)
        {
            var res = BitConverter.ToUInt64(GetBytes(stream, 8),0);
            return res;
        }

        /// <summary>
        /// 从流中读取uint32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ulong ToUInt64(Stream stream, ref long offset)
        {
            var res = BitConverter.ToUInt64(GetBytes(stream, ref offset, 8), 0);
            return res;
        }


        /// <summary>
        /// 从流中读取int32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static int ToInt32(Stream stream)
        {
            var res = BitConverter.ToInt16(GetBytes(stream, 4), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取int32
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static int ToInt32(Stream stream, ref long offset)
        {
            var res = BitConverter.ToInt16(GetBytes(stream, ref offset, 4), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取ushort
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        internal static ushort ToUInt16(Stream stream)
        {
            return BitConverter.ToUInt16(GetBytes(stream,2), 0);
        }

        /// <summary>
        /// 从流中读取ushort
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ushort ToUInt16(Stream stream, ref long offset)
        {
            var res= BitConverter.ToUInt16(GetBytes(stream,ref offset, 2), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取ushort
        /// 自动设置偏移量
        /// Big序 低字节为高位序
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ushort ToUInt16Big(Stream stream, ref long offset)
        {
            var res = BitConverter.ToUInt16(GetBytes(stream, ref offset, 2).Reverse().ToArray(), 0);
            return res;
        }

        /// <summary>
        /// 从流中读取字节
        /// 自动设置偏移量
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte ToByte(Stream stream, ref long offset)
        {
            stream.Position = offset;
            var res = stream.ReadByte();
            offset= stream.Position;
            byte bres = res < 0 ? (byte)0 : (byte)res;
            return bres;
        }

        /// <summary>
        /// 从字节流中，获取一段数据副本
        /// </summary>
        /// <param name="bufp"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] GetBytes(Stream streamp, int len)
        {
            byte[] buf = new byte[len];
            streamp.Read(buf,0, len);
            return buf;
        }

        /// <summary>
        /// 从字节流中，获取一段数据副本
        /// </summary>
        /// <param name="bufp"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        internal static byte[] GetBytes(Stream streamp,ref long offset, int len)
        {
            streamp.Position = offset;
            byte[] buf = new byte[len];
            streamp.Read(buf, 0, len);
            offset = streamp.Position;
            return buf;
        }

        /// <summary>
        /// 字节组转字符串。
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string GetUTF8String(this byte[] buf)
        {
            return Encoding.UTF8.GetString(buf);
        }

        /// <summary>
        /// 转16进 字符串
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ToString(this byte[] buf, int start = 0, int len = -1)
        {
            var tlen = len;
            if (tlen < 0) tlen = buf.Length;
            var bbuf = buf.Skip(start).Take(tlen).ToArray();
            var str = Encoding.UTF8.GetString(bbuf);
            return str;
        }
    }
}
