using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MLBotClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TcpClient tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 28310);
            var buffer = new byte[4096];
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit") break;

                if (string.IsNullOrEmpty(line)) continue;

                var tcpstream= tcp.GetStream();
                tcpstream.Write(Encoding.UTF8.GetBytes(line));
                var rlen= tcpstream.Read(buffer);
                if (rlen == 0) break;

                var text = Encoding.UTF8.GetString(buffer,0,rlen);
                Console.WriteLine($"响应：{text}");
            }
        }
    }
}
