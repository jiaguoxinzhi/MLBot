using MLBot.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 聊天者数据
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class ChatterData : IEntity
    {
        /// <summary>
        /// 聊天者数据目录
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        private static string ChatterData_Forder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ChatterData");

        /// <summary>
        /// 示例聊天者数据目录
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        private static string Sample_Path = Path.Combine(ChatterData_Forder, "Sample.json");

        /// <summary>
        /// 初始化
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        static ChatterData()
        {
            if (!Directory.Exists(ChatterData_Forder)) Directory.CreateDirectory(ChatterData_Forder);
            try
            {
                //添加示例数据
                Sample.Name = "小易";
                Sample.Birthday = DateTimeOffset.Now;

                Sample.Tags.Add(new TagsInfo()
                {
                    FromId = Sample.Id,
                    Tags = new List<string>()
                    {
                        "聪明" ,
                        "很聪明" ,
                        "非常聪明" ,
                        "不是一般的聪明" ,
                   },
                });

                if (!File.Exists(Sample_Path)) CreateSample();
                var fo = File.OpenRead(Sample_Path);
            }
            catch (Exception ex)
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
            File.WriteAllText(Sample_Path, Sample.ToJsonString());
        }

        /// <summary>
        /// 聊天者数据示例
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public static readonly ChatterData Sample = new ChatterData();

        /// <summary>
        /// 唯一id
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public DateTimeOffset Birthday { get; private set; }

        /// <summary>
        /// 标签
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public List<TagsInfo> Tags { get; set; } = new List<TagsInfo>();

        /// <summary>
        /// 亲密度
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public List<IntimacyInfo> Intimacis { get; set; } = new List<IntimacyInfo>();


        /// <summary>
        /// 当前情绪状态
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public MoodType Mood {get;set;}

    }

    /// <summary>
    /// 聊天者情绪状态
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public enum MoodType
    {
        未知,
        //奇
        好奇,
        //见不得别人好
        忌妒,

        无聊, 空虚,

        高兴, 愉快, 喜悦,

        愤怒, 恼怒, 发怒,

        伤心, 悲伤, 悲痛, 悲哀, 哀愁,

        欢乐, 快乐, 愉悦, 充满幸福,
    }

    /// <summary>
    /// 针对某事物的情绪状态
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public enum MoodForedType
    {
        未知,
        //喜
        喜爱, 喜好, 喜欢,
        //奇
        好奇,
        //忌妒别的智慧生命 见不得别人好
        忌妒,

        愤怒, 怨恨, 愤恨,

        怜悯, 哀怜, 哀悯, 哀怨, 哀思,
        //惊
        惊咤, 惊愕, 惊慌, 惊悸, 惊奇, 惊叹, 惊喜, 惊讶,

        恐慌, 恐惧, 害怕, 担心, 担忧, 畏惧,

        思念,想念,思慕,
    }


    /// <summary>
    /// 欲望状态
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public enum EesireType
    {
        未知,

        /// <summary>
        /// 喜惊引发 即产生自问并寻找答案
        /// </summary>
        求知探索欲,
        /// <summary>
        /// 希望被更多人认识、尊重、称赞、羡慕。而产生的行为。
        /// </summary>
        名望欲,
        /// <summary>
        /// 希望被更多人认识、尊重、称赞、羡慕。而产生的肢体行为。
        /// </summary>
        表现欲,
        /// <summary>
        /// 希望自有某事物或某能力。
        /// </summary>
        拥有欲,
        /// <summary>
        /// 因为喜欢或好奇或无聊或空虚而希望与某个人交流交友。
        /// </summary>
        交友欲,
        /// <summary>
        /// 因愤怒、忌妒、恐惧，想要打击引起愤怒、忌妒的事物或牵连打击别的事件
        /// </summary>
        打击欲,
        /// <summary>
        /// 因悲伤等，觉得自己的某事物或能力不好，想要修改、删除的欲望
        /// </summary>
        自伤欲,
        /// <summary>
        /// 因恐惧等，想要创建某事物或某能力以求得安全
        /// </summary>
        求安欲,
        /// <summary>
        /// 因思念等，想要重逢某事物，或将自己重置某种状态，或重置某种环境中
        /// </summary>
        重置欲,
    }

    /// <summary>
    /// 亲密度信息
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class IntimacyInfo
    {
    }

    /// <summary>
    /// 标签信息
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class TagsInfo
    {
        /// <summary>
        /// 添加人的ID
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public Guid FromId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [Author("Linyee", "2019-07-16")]
        public List<string> Tags { get; set; }
    }
}
