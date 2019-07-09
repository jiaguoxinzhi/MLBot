
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MLBot.Enums
{
    /// <summary>
    /// 常数及枚举处理
    /// Linyee 2018-11-02
    /// </summary>
    public class ConstEnum
    {



        /// <summary>
        /// A从B复制
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static object CopyAFromB(object a, object b)
        {
            var btype = b.GetType();
            foreach (var ap in a.GetType().GetProperties())
            {
                var bp = btype.GetProperty(ap.Name);
                if (bp == null) continue;
                if (bp.GetGetMethod() == null || bp.GetGetMethod().IsPrivate) continue;
                if (ap.GetSetMethod() == null || ap.GetSetMethod().IsPrivate) continue;
                ap.SetValue(a, bp.GetValue(b));
            }
            return a;
        }

        /// <summary>
        /// 获取描述属性内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object obj)
        {
            if (obj == null) return null;
            var oType = obj.GetType();
            if (oType.IsEnum)
            {
                var name = Enum.GetName(oType, obj);
                var f = oType.GetField(name);
                if (f == null)
                {
                    return null;
                }
                else
                {
                    var des=(DescriptionAttribute[]) f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return string.Join(",", des.Select(p => p.Description));
                }
            }
            else
            {
                throw new Exception("只支持Enum类型");
            }
        }


        /// <summary>
        /// 获取二维码类别
        /// </summary>
        /// <param name="qrtype"></param>
        /// <returns></returns>
        public static string GetQrtypeEnName(int qrtype)
        {
            var istype = qrtype;
            switch (istype)
            {
                case 1:
                    return "alipay";
                case 2:
                    return "wechat";
                case 3:
                    return "union";
                case 5:
                    return "QQ";
                case 6:
                    return "jd";
                default:
                    return "unknow";
            }
        }

        /// <summary>
        /// 获取二维码类别
        /// </summary>
        /// <param name="qrtype"></param>
        /// <returns></returns>
        public static string GetQrtypeName(int qrtype)
        {
            var istype = qrtype;
            switch (istype)
            {
                case 1:
                    return "支付宝";
                case 2:
                    return "微信";
                case 3:
                    return "银联";
                case 5:
                    return "QQ";
                case 6:
                    return "京东";
                default:
                    return "未知方式";
            }
        }
    }
}
