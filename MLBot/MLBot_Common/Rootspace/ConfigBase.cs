
using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 配置基类
    /// </summary>
    [Author("Linyee", "2019-05-12")]
    public class ConfigBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConfigBase Default = new ConfigBase();

        /// <summary>
        /// 是否输出客户端调试信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsWebSocketClient10Minute { get; set; } = false;
        /// <summary>
        /// 是否输出WS调试信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsWebSocket10Minute { get; set; } = false;
        /// <summary>
        /// 是否输出运行信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsRuntime { get; set; } = false;
        /// <summary>
        /// 是否输出签名信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsSignRuntime { get; set; } = false;

        /// <summary>
        /// 是否跟踪16进制数据
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsTraceHex { get; set; } = false;
        /// <summary>
        /// 是否输出异常信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsException { get; set; } = true;
        /// <summary>
        /// 是否跟踪锁信息
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsTraceLock { get; set; } = false;
        /// <summary>
        /// 是否跟踪收发消息
        /// 必须先打开 IsWebSocket10Minute 或 IsWebSocketClient10Minute 才能正常输出
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public bool IsTraceRSMSG { get; set; } = false;

        /// <summary>
        /// 设置到默认实例
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public virtual void SetToDefault()
        {
            Default.IsRuntime = IsRuntime;
            Default.IsSignRuntime = IsSignRuntime;
            Default.IsException = IsException;
            Default.IsTraceHex = IsTraceHex;
            Default.IsTraceLock = IsTraceLock;
            Default.IsWebSocket10Minute = IsWebSocket10Minute;
            Default.IsWebSocketClient10Minute = IsWebSocketClient10Minute;
            Default.IsTraceRSMSG = IsTraceRSMSG;
        }

        /// <summary>
        /// 全部打开
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public virtual void OpenAll()
        {
            this.IsException = true;
            this.IsRuntime = true;
            this.IsSignRuntime = true;
            this.IsException = true;
            this.IsTraceHex = true;
            this.IsTraceLock = true;
            this.IsWebSocket10Minute = true;
            this.IsWebSocketClient10Minute = true;
            this.IsTraceRSMSG = true;
        }

        /// <summary>
        /// 全部打开
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public virtual void CloseAll()
        {
            this.IsException = false;
            this.IsRuntime = false;
            this.IsSignRuntime = false;
            this.IsException = false;
            this.IsTraceHex = false;
            this.IsTraceLock = false;
            this.IsWebSocket10Minute = false;
            this.IsWebSocketClient10Minute = false;
            this.IsTraceRSMSG = false;
        }

        /// <summary>
        /// 恢复到默认
        /// </summary>
        [Author("Linyee", "2019-05-12")]
        public virtual void ResetToDefault()
        {
            this.IsException = false;
            this.IsRuntime = false;
            this.IsSignRuntime = false;
            this.IsException = true;
            this.IsTraceHex = false;
            this.IsTraceLock = false;
            this.IsWebSocket10Minute = false;
            this.IsWebSocketClient10Minute = false;
            this.IsTraceRSMSG = false;
        }



        /// <summary>
        /// 从旧对象复制
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-05-12")]
        public ConfigBase From(ConfigBase configuration)
        {
            //调试配置
            this.IsSignRuntime = configuration.IsSignRuntime;
            this.IsException = configuration.IsException;
            this.IsRuntime = configuration.IsRuntime;
            this.IsSignRuntime = configuration.IsSignRuntime;
            this.IsTraceHex = configuration.IsTraceHex;
            this.IsTraceLock = configuration.IsTraceLock;
            this.IsTraceRSMSG = configuration.IsTraceRSMSG;
            this.IsWebSocket10Minute = configuration.IsWebSocket10Minute;
            this.IsWebSocketClient10Minute = configuration.IsWebSocketClient10Minute;
            return this;
        }

    }

}
