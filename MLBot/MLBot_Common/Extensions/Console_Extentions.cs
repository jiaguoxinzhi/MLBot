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
        /// 控制台 有色输出
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
        /// 控制台 有色输出行
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
        /// 控制台 有色输出行
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        [Author("Linyee", "2018-05-05")]
        static void WriteColorLine(string str, ConsoleColor color)
        {
            WriteLine(color, str);
        }

        /// <summary>
        /// 控制台 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color);
        }

        /// <summary>
        /// 控制台 按任意键退出
        /// 用于等待任意键
        /// </summary>
        /// <param name="str">待打印的字符串 为空=按任意键退出</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2019-07-03")]
        public static void WaitAnykey(this string str, ConsoleColor color = ConsoleColor.White)
        {
            var tstr = str;
            if (string.IsNullOrEmpty(tstr)) tstr = "按任意键退出！";
            tstr.WriteInfoLine(color);
            Console.ReadKey();
        }

        /// <summary>
        /// 控制台 请输入一行信息
        /// 用于等待输入入
        /// </summary>
        /// <param name="str">待打印的字符串 为空=按任意键退出</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2019-07-03")]
        public static string WaitReadLine(this string str, ConsoleColor color = ConsoleColor.White)
        {
            var tstr = str;
            if (string.IsNullOrEmpty(tstr)) tstr = "请输入一行信息：";
            tstr.WriteInfoLine(color);
            return Console.ReadLine();
        }

        /// <summary>
        /// 控制台 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 控制台 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        [Author("Linyee", "2018-05-05")]
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 控制台 打印成功的信息
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
