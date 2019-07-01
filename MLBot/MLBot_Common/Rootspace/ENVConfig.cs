using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MLBot
{
    /// <summary>
    /// 环境配置信息
    /// </summary>
    [Author("Linyee", "2019-04-24")]
    public class ENVConfig
    {
        /// <summary>
        /// 是否Linux系统
        /// </summary>
        [Author("Linyee", "2019-04-24")]
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        /// <summary>
        /// 是否Windows系统
        /// </summary>
        [Author("Linyee", "2019-04-24")]
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// 是否OSX系统
        /// </summary>
        [Author("Linyee", "2019-04-24")]
        public static bool OSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }

}
