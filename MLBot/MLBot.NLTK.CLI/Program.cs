using MLBot.Extentions;
using System;
using MLBot.NLTK.Analyzers;

namespace LYNLTK.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            //../../MLbot/MLbot/MLBot.NLTK.CLI/bin/debug/netcoreapp3.0/MLBot.NLTK.CLI "很多读者指出了不朽凡人的不足，有些我认为说的是对的，有些我认为和我的想法不同。如果说不朽凡人结尾的不好，我同意。要说烂尾，我真的不同意。关于量劫带来宇宙涅化的结尾，我还是比较满意的。因为在这里有各种人性的显露，人性在生死存亡面前的脆弱。"
            Console.WriteLine("(∩＿∩) 欢迎使用 LYNLTK!");
            if (args.Length > 1 && args[0].Equals("add"))
            {
                Console.WriteLine($"{args[0]} {args[1]} {LinyeeNLAnalyzer.Default.AddWord(args[1]).ToJsonString()}");
            }
            if (args.Length > 0 && args[0].Equals("save"))
            {
                Console.WriteLine($"{args[0]} {LinyeeNLAnalyzer.Default.Save().ToJsonString()}");
            }
            else
            {
                foreach (var line in args)
                {
                    Console.WriteLine($"{line} 分词：{LinyeeNLAnalyzer.Default.WordAnaly(line).ToJsonString()}");
                }
            }
        }
    }
}
