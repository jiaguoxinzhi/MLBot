using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Extentions
{
    /// <summary>
    /// 控制台 扩展
    /// </summary>
    [Author("Linyee", "2018-05-05")]
    public static class Console_Extentions
    {
        /// <summary>
        /// 有色输出
        /// </summary>
        /// <param name="color"></param>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-05-05")]
        public static void Write(ConsoleColor color, string msg)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = currentForeColor;
        }

        /// <summary>
        /// 有色输出行
        /// </summary>
        /// <param name="color"></param>
        /// <param name="msg"></param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteLine(ConsoleColor color, string msg)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = currentForeColor;
        }

        /// <summary>
        /// 有色输出行
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        [Author("Linyee", "2018-05-05")]
        static void WriteColorLine(string str, ConsoleColor color)
        {
            WriteLine(color, str);
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color);
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(str, color);
        }
    }
}
