using MLBot.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MLBot.Extentions
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    [Author("Linyee","2019-06-28")]
    public static class Dictionory_Extentions
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <returns></returns>
        [Author("Linyee", "2019-06-28")]
        public static string Sign(this IDictionary dict,string arrjoin,string kvjoin,string keyname,string key,string signName,params string[] skips)
        {
            var tdict = new Dictionary<string, string>();
            foreach(DictionaryEntry de in dict)
            {
                var dkey = de.Key.ToString();
                if (dkey.Equals("sign", StringComparison.OrdinalIgnoreCase)) continue;//跳过签名字段
                if(skips.Contains(dkey)) continue;//跳过指定字段

                tdict.Add(dkey, de.Value?.ToString());
            }

            var source =string.Join(arrjoin, tdict.Select(d=>$"{d.Key}{kvjoin}{d.Value}"));
            var source_key = $"{source}{arrjoin}{keyname}{kvjoin}{key}";
            var signmd5 = source_key.ToMd5String();
            LogService.AnyLog("Sign", $"{signName}签名源串", source_key);
            LogService.AnyLog("Sign", $"{signName}签名结果", signmd5);
            return signmd5;
        }

        ///// <summary>
        ///// 验签
        ///// </summary>
        ///// <returns></returns>
        //[Author("Linyee", "2019-06-28")]
        //public static bool Check(this IDictionary dict,string key,string value,StringComparison keycomparison= StringComparison.Ordinal, StringComparison valcomparison = StringComparison.Ordinal)
        //{
        //    var jval = value.ToJsonString();
        //    foreach (DictionaryEntry de in dict)
        //    {
        //        var dkey = de.Key?.ToString();
        //        var dval = de.Value?.ToJsonString();
        //        if (dkey.Equals(key, keycomparison))
        //        {
        //            if (dval.Equals(jval, valcomparison)) return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
