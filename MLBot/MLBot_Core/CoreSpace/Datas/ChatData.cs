using MLBot.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 问答数据
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class QAData: IEntity
    {

        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid Id { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string Answer { get; set; }
    }


    /// <summary>
    /// 问答模式数据
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class QAPatternData : IEntity
    {
        /// <summary>
        /// 聊天数据目录
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        private static string ChatData_Forder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ChatData");

        /// <summary>
        /// 示例聊天数据目录
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        private static string Sample_Path = Path.Combine(ChatData_Forder, "Sample.json");

        /// <summary>
        /// 初始化
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        static QAPatternData() {
            if (!Directory.Exists(ChatData_Forder)) Directory.CreateDirectory(ChatData_Forder);
            try
            {
                //添加示例数据
                SampleData.Add(new QAPatternData() { QuestionPattern = "[你您]?是哪[只条头][？\\?]?",QuestionScenario="关系亲密或关系陌生但污侮性", AnswerPattern = "[我]?([A-Za-z\x20]+|[\u4E00-\u9FA5]+)" });
                SampleData.Add(new QAPatternData() { QuestionPattern = "[你您]?是哪[位个][？\\?]?", AnswerPattern = "[我]?([A-Za-z\x20]+|[\u4E00-\u9FA5]+)" });
                SampleData.Add(new QAPatternData() { QuestionPattern = "[你您]?是谁[？\\?]?", AnswerPattern = "[我]?([A-Za-z\x20]+|[\u4E00-\u9FA5]+)" });
                SampleData.Add(new QAPatternData() { QuestionPattern= "[你您]?叫什么(名字)?[？\\?]?",AnswerPattern= "[我]?([A-Za-z\x20]+|[\u4E00-\u9FA5]+)" });
                SampleData.Add(new QAPatternData() { QuestionPattern = "([你您]的)?姓名是[什么]?[？\\?]?", AnswerPattern = "[我]?([A-Za-z\x20]+|[\u4E00-\u9FA5]+)" });
                SampleData.Add(new QAPatternData() { QuestionPattern = "[你您]?今年几岁[了]?[？\\?]?", AnswerPattern = "[我]?(\\d+|<岁数>)", });
                SampleData.Add(new QAPatternData() { QuestionPattern = "[你您]?是([A-Za-z\u4E00-\u9FA5]+)还是([A-Za-z\u4E00-\u9FA5]+)", AnswerPattern = "$1|$2", });

                if (!File.Exists(Sample_Path)) CreateSample();
                var fo= File.OpenRead(Sample_Path);
            }catch(Exception ex)
            {
                LogService.Exception(ex);
            }
        }

        /// <summary>
        /// 创建示例数据
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        private static void CreateSample()
        {
            File.WriteAllText(Sample_Path,SampleData.ToJsonString());
        }

        /// <summary>
        /// 内置示例数据
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public static readonly List<QAPatternData> SampleData=new List<QAPatternData>();

        /// <summary>
        /// 聊天数据示例
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public static readonly IEnumerable<QAPatternData> Sample;

        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid Id { get; set; }

        /// <summary>
        /// 答案模式
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string AnswerPattern { get; set; }

        /// <summary>
        /// 问题模式
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string QuestionPattern { get; set; }

        /// <summary>
        /// 发问声景
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string QuestionScenario { get; set; }
    }
}
