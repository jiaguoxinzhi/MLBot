using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace MLBot.Extentions
{
    /// <summary>
    /// 流扩展
    /// </summary>
    public static class Stream_Write_Extentions
    {
        /// <summary>
        /// 读取字节数
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="offset">偏移</param>
        /// <param name="maxOffset">最大偏移</param>
        /// <returns></returns>
        public static bool WriteLine(this Stream stream, string text, long offset = 0, long maxOffset =0)
        {
            stream.Position = offset;
            StringBuilder sbd = new StringBuilder(text);
            sbd.AppendLine();
            var buf = Encoding.UTF8.GetBytes(sbd.ToString());
            if (maxOffset > 0 && offset+ buf.Length>= maxOffset)
            {
                return false;
            }
            else
            {
                stream.Write(buf, 0, buf.Length);
                return true;
            }
        }
    }
}
