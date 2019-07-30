using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using MLBot.Extentions;
using MLBot.Enums;
using System.Diagnostics;
using MLBot.NLTK.Models;
using MLBot.NLTK.Abstracts;
using JiebaNet.Segmenter.PosSeg;
using JiebaNet.Segmenter;

namespace MLBot.NLTK.Analyzers
{



    /// <summary>
    /// 分析器
    /// </summary>
    [Author("Linyee", "2019-07-17")]
    public class LinyeeNLAnalyzer : NLAnalyzer, IDisposable
    {
        #region 默认
        /// <summary>
        /// 默认实例
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        public static readonly LinyeeNLAnalyzer Default=new LinyeeNLAnalyzer();

        /// <summary>
        /// 所有标点符号
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal static readonly List<char> StopChars = new List<char>();

        /// <summary>
        /// 词语停止的字符集，通常是标点符号
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal static readonly List<char> WordStopChars = new List<char>();

        /// <summary>
        /// 语句停止的字符集，通常是句断符号
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal static readonly List<char> SentenceStopChars = new List<char>();

        /// <summary>
        /// 段落停止的字符集，通常是换行符号
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        internal static readonly List<char> ParagraphStopChars = new List<char>();

        /// <summary>
        /// 词典
        /// </summary>
        [Author("Linyee", "2019-07-19")]
        internal static readonly List<WordInfoOnce> WordDict = new List<WordInfoOnce>();

        /// <summary>
        /// 初始化
        /// </summary>
        static LinyeeNLAnalyzer()
        {
            try
            {
                //'α','β','γ','δ','ε','ζ','η','θ','ι','κ','λ','μ','ν','ξ','ο','π','ρ','σ','τ','υ','φ','χ','ψ','ω',
                //'Α','Β','Γ','Δ','Ε','Ζ','Η','Θ','Ι','Κ','Λ','Μ','Ν','Ξ','Ο','Π','Ρ','Σ','Τ','Υ','Φ','Χ','Ψ','Ω',//希腊字母
                //々重复上一字，或上一个字词的复数形式
                //WordStopChars.AddRange(new char[] {
                //    ' ', ',',  '~', '#', '^', '&', '=','(', ')',  '{', '}', '[', ']', '<', '>',  '\'', '\"', ';', ':','|', '/','+','-','_','*','$',//英文
                //    '　', '，', '～', '＃', '…', '＆', '（', '）', '｛', '｝', '［', '］','〖', '〗', '【', '】', '；', '：', '‘','’', '“','”', '｜', '‖', '・','《', '》', '、','—','｀','ˉ','¨','々','＂','〃','＇',//中文
                //    '≈','≡','≠','＝','≤','≥','〈', '〉','≮','≯','∷','±','＋','－','×','÷','／','∫','∮','∝','∞','∧','∨','∑','∏','∪','∩','∈','∵','∴','⊥','∥','∠','⌒','⊙','≌','∽','√',//数学符号
                //    '「','」','『','』',//日文
                //});
                //排除 '々','-','+','&',
                WordStopChars.AddRange(new char[] {
                    ' ', ',',  '~', '#', '^', '=','(', ')',  '{', '}', '[', ']', '<', '>',  '\'', '\"', ';', ':','|', '/','_','*','$',//英文
                    '　', '，', '～', '＃', '…', '＆', '（', '）', '｛', '｝', '［', '］','〖', '〗', '【', '】', '；', '：', '‘','’', '“','”', '｜', '‖', '《', '》', '、','—','｀','ˉ','¨','＂','〃','＇',//中文
                    '≈','≡','≠','＝','≤','≥','〈', '〉','≮','≯','∷','±','＋','－','×','÷','／','∫','∮','∝','∞','∧','∨','∑','∏','∪','∩','∈','∵','∴','⊥','∥','∠','⌒','⊙','≌','∽','√',//数学符号
                    '「','」','『','』',//日文
                });
                SentenceStopChars.AddRange(new char[] {
                    '.', '!', '?',//英文
                     '。', '！','？',//中文
                });
                ParagraphStopChars.AddRange(new char[] {
                    '\r','\n',//通用
                });

                StopChars.AddRange(WordStopChars);
                StopChars.AddRange(SentenceStopChars);
                StopChars.AddRange(ParagraphStopChars);
            }
            catch (Exception ex)
            {
                LogService.Exception(ex);
            }
        }

        /// <summary>
        /// 分析器
        /// </summary>
        [Author("Linyee", "2019-07-17")]
        public LinyeeNLAnalyzer()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            //var dict= LinyeeWrodDict.Default;//初始化词典
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// 主动保存
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-07-25")]
        public WordAnalyResult Save()
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());

            LinyeeWrodDict.Default.OpenSaveCloseSave();
            return new WordAnalyResult().SetOk();
        }

        /// <summary>
        /// 加词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-25")]
        internal WordInfoOnce AddWord(string text)
        {
            return new WordInfoOnce(text);
        }
        #endregion

        /// <summary>
        /// 分词
        /// 默认UTF8编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-30")]
        public WordAnalyResult WordAnalyJieba(string text, Encoding encoding = null)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            if (string.IsNullOrEmpty(text)) return WordAnalyResult.Empty;
            var segmenter = new JiebaSegmenter();
            return new WordAnalyResult().SetData(segmenter.Cut(text));
            //var posSeg = new PosSegmenter(segmenter);
            //return posSeg.Cut(text, hmm).Select(token => string.Format("{0}/{1}", token.Word, token.Flag));

        }

        /// <summary>
        /// 分词
        /// 默认UTF8编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-17")]
        public WordAnalyResult WordAnaly(string text,Encoding encoding=null)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            if (string.IsNullOrEmpty(text)) return WordAnalyResult.Empty;

            var enc = Encoding.UTF8;
            //var buffer = enc.GetBytes(text);
            if (encoding != null) enc = encoding;
            ReadOnlySpan<char> span = new ReadOnlySpan<char>(text.ToCharArray());
            return new LinyeeNLParagraphAnalyzer(span, enc, ParagraphStopChars, SentenceStopChars, WordStopChars).ParagraphAnaly();
        }


        /// <summary>
        /// 新词
        /// 默认UTF8编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Author("Linyee", "2019-07-29")]
        public WordAnalyResult AddWord(string text,Encoding encoding=null)
        {
            if (ConfigBase.Default.IsTraceStack) LogService.AnyLog("Stack", new StackTrace().GetFrame(0).GetMethod().ToString());
            if (string.IsNullOrEmpty(text)) return WordAnalyResult.Empty;

            //var enc = Encoding.UTF8;
            ////var buffer = enc.GetBytes(text);
            //if (encoding != null) enc = encoding;
            ReadOnlySpan<char> span = new ReadOnlySpan<char>(text.ToCharArray());
            return LinyeeWrodDict.Default.AddNew(text,new WordInfoOnce(span));
        }
    }
}
