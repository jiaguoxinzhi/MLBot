using MLBot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MLBot.Extentions;
using MLBot.Attributes;

namespace MLBotServer
{
    /// <summary>
    /// 服务端
    /// </summary>
    [Author("Linyee", "2019-07-01")]
    public class Program
    {
        /// <summary>
        /// 服务端入口
        /// </summary>
        /// <param name="args"></param>
        [Author("Linyee", "2019-07-01")]
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //主答模式
            TcpListener tcp = new TcpListener(new IPEndPoint(IPAddress.Any, 28310));
            tcp.Start();
            $"主答服务已启动{28310}".WriteSuccessLine();
            new Thread(()=>{
                while (true)
                {
                    try
                    {
                        var tcpclt = tcp.AcceptTcpClient();
                        new Thread(() =>
                        {
                            OnClientConnectioned(tcpclt);
                            var buffer = new byte[4096];
                            while (true)
                            {
                                var tcpstream = tcpclt.GetStream();
                                tcpstream.Read(buffer, 0, 4096);
                                var buf = MLAiBot.Default.Processing(buffer);
                                tcpstream.Write(buf);
                                //
                            }

                        }).Start();
                    }
                    catch
                    {
                        break;
                    }
                }
                "主答服务已关闭".WriteErrorLine();
            }).Start();

            TcpListener tcp01 = new TcpListener(new IPEndPoint(IPAddress.Any, 28311));
            tcp01.Start();
            $"主问服务已启动{28311}".WriteSuccessLine();
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {

                        var tcpclt = tcp01.AcceptTcpClient();
                    new Thread(() =>
                    {
                        OnClientConnectioned(tcpclt);
                        var buffer = new byte[4096];
                        while (true)
                        {
                            var tcpstream = tcpclt.GetStream();
                            tcpstream.Read(buffer, 0, 4096);
                            var buf = MLAiBot.Default.Processing(buffer);
                            tcpstream.Write(buf);
                            //
                        }

                    }).Start();
                    }
                    catch
                    {
                        break;
                    }
                }
                "主问服务已关闭".WriteErrorLine();
            }).Start();

            //一直阻塞
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            while (true)
            {
                resetEvent.WaitOne();
            }
        }


        /// <summary>
        /// 连接完成时
        /// </summary>
        /// <param name="tcpclt"></param>
        [Author("Linyee", "2019-07-01")]
        public static void OnClientConnectioned(TcpClient tcpclt)
        {
            var tcpstream= tcpclt.GetStream() ;
            tcpstream.Write(Encoding.UTF8.GetBytes("欢迎光临，小易正在为您服务！"));
        }
    }
}
