using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MLBot.Extentions;

namespace MLBot.Extentions
{
    #region "json转换器"
    /// <summary>
    /// 名值集合转换器
    /// </summary>
    internal class NameValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var otype = objectType;
            var ctype = typeof(NameValueCollection);
            while (otype != null)
            {
                if (otype == ctype) return true;
                otype = otype.BaseType;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as NameValueCollection;
            var dict = new Dictionary<string, string>();
            foreach (string key in obj.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                var val = obj.Get(key);
                dict.Add(key, val);
            }
            serializer.Serialize(writer, dict);
        }
    }
    /// <summary>
    /// 记录转换器
    /// </summary>
    internal class DataRowJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var otype = objectType;
            var ctype = typeof(DataRow);
            while (otype != null)
            {
                if (otype == ctype) return true;
                otype = otype.BaseType;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as DataRow;
            var dict = obj.ToDictionary();
            serializer.Serialize(writer, dict);
        }
    }
    /// <summary>
    /// 记录集转换器
    /// </summary>
    internal class ICollectionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var can = objectType.IsAssignableFrom(typeof(ICollection));
            return can;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as ICollection;
            List<object> list = new List<object>();
            foreach (object item in obj)
            {
                list.Add(item);
            }
            serializer.Serialize(writer, list);
        }
    }
    /// <summary>
    /// 记录集转换器
    /// </summary>
    internal class IListCollectionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var can = objectType.IsAssignableFrom(typeof(IList));
            return can;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as IList;
            List<object> list = new List<object>();
            foreach (object item in obj)
            {
                list.Add(item);
            }
            serializer.Serialize(writer, list);
        }
    }
    #endregion

    /// <summary>
    /// json 助手
    /// Linyee 2018-06-27
    /// </summary>
    public static class Json_Extentions
    {
        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fffffff";
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;//加了这个会变成unicode编码 \u00e6\u0088

            settings.Converters.Add(new NameValueJsonConverter());
            settings.Converters.Add(new DataRowJsonConverter());
            settings.Converters.Add(new ICollectionJsonConverter());
            settings.Converters.Add(new IListCollectionJsonConverter());

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 异步 转Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task<string> ToJsonStringAsync(this object obj)
        {
            return await Task.Run<string>(() =>
            {
                if (obj == null) return null;
                string json = null;
                try
                {
                    json = ToJsonString(obj);
                }
                catch (Exception ex)
                {
                    LogService.Exception(ex);
                    return "\"解析时发生错误\"";
                }
                return json;
            });
        }

        /// <summary>
        /// 转表单字符串
        /// 跳过指定名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="skips"></param>
        /// <returns></returns>
        /// <remarks>这个函数不要随意改，主要用于签名等，区分大小写</remarks>
        public static string ToAscFormString(this object obj, string[] skips, bool isencode = false)
        {
            SortedDictionary<string, object> sdict = new SortedDictionary<string, object>();
            if (obj is IDictionary<string, object>)
            {
                sdict = new SortedDictionary<string, object>((IDictionary<string, object>)obj);
                foreach (var key in skips)
                {
                    if (sdict.ContainsKey(key))
                    {
                        sdict.Remove(key);
                    }
                }
            }
            else
            if (obj is IDictionary<string, string>)
            {
                var json = obj.ToJsonString();
                sdict = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(json);
            }
            //object 有可能限入死循环等，，所以另开一个分支，不要先ToJsonString()
            else
            {
                foreach (var p in obj.GetType().GetProperties())
                {
                    if (p == null) continue;
                    if (p.GetGetMethod().IsPrivate) continue;
                    if (skips != null && skips.Contains(p.Name))
                    {
                        continue;
                    }
                    sdict.Add(p.Name, p.GetValue(obj));
                }
            }

            if (skips != null)
            {
                foreach (var key in skips)
                {
                    if (sdict.ContainsKey(key))
                    {
                        sdict.Remove(key);
                    }
                }
            }

            var ascformstr = string.Join("&", sdict.Select(d => d.Key + "=" + d.Value?.ToString().ToUrlEncode(isencode)).ToArray());
            return ascformstr;
        }


        /// <summary>
        /// 转表单字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isencode">是否需要编码，便于可选使用，默认不编码</param>
        /// <param name="front">前置字符串</param>
        /// <returns></returns>
        public static string ToFormString(this object obj, bool isencode = false, string front = "")
        {
            var json = obj.ToJsonString();// JsonConvert.SerializeObject(obj);
            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return front + string.Join("&", dict.Select(d => d.Key + "=" + d.Value.ToUrlEncode(isencode)).ToArray());
            }
            catch (Exception ex)
            {
                return front + string.Format("json={0}&msg={1}", json, ex.Message);
            }
        }

        /// <summary>
        /// 记录集转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataRowCollection rows)
        {
            List<T> list = new List<T>();
            var ttype = typeof(T);
            var dtype = typeof(Dictionary<string, object>);
            foreach (DataRow dr in rows)
            {
                var dict = dr.ToDictionary();
                var ooo = Activator.CreateInstance(ttype); ;
                T obj = (T)ooo;
                if (ttype == dtype)
                {
                    var dobj = (Dictionary<string, object>)ooo;
                    dobj = dict;
                }
                else
                if (ttype == typeof(JObject))
                {
                    var jobj = (JObject)ooo;
                    foreach (var key in dict.Keys)
                    {
                        jobj.Add(key, JToken.FromObject(dict[key]));
                    }
                }
                else
                {
                    foreach (var key in dict.Keys)
                    //foreach (var tf in typeof(T).GetProperties())
                    {
                        var tf = ttype.GetProperty(key);
                        //HttpContext.Current.Response.Write((tf==null)+"\r\n");
                        if (tf != null) tf.SetValue(obj, dict[key]);
                        //if (dict.ContainsKey(tf.Name)) tf.SetValue(obj, dict[tf.Name]);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static Dictionary<Type, bool> numeritypelist = new Dictionary<Type, bool>();
        static Json_Extentions()
        {
            numeritypelist.Add(typeof(byte), true);
            //numeritypelist.Add(typeof(short), true);
            //numeritypelist.Add(typeof(int), true);
            //numeritypelist.Add(typeof(long), true);
            numeritypelist.Add(typeof(Int16), true);
            numeritypelist.Add(typeof(Int32), true);
            numeritypelist.Add(typeof(Int64), true);
            //numeritypelist.Add(typeof(uint), true);
            //numeritypelist.Add(typeof(ulong), true);
            numeritypelist.Add(typeof(UInt16), true);
            numeritypelist.Add(typeof(UInt32), true);
            numeritypelist.Add(typeof(UInt64), true);
            //numeritypelist.Add(typeof(float), true);
            numeritypelist.Add(typeof(Single), true);
            numeritypelist.Add(typeof(double), true);
            numeritypelist.Add(typeof(decimal), true);
        }
        /// <summary>
        /// 是否是数值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type type)
        {
            if (numeritypelist.ContainsKey(type)) return numeritypelist[type];
            return false;
        }

        /// <summary>
        /// 转Json 主要用于4.0平台 用默认json.net4.0时
        /// 新版Json基本上正常
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this DataRow list)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            {
                foreach (DataColumn col in list.Table.Columns)
                {
                    dict.Add(col.ColumnName, list[col.ColumnName]);
                }
            }
            return dict;
        }


        ///// <summary>
        ///// 序列化为JSON字符串
        ///// Linyee 2018-05-08
        ///// 转Json 主要用于4.0平台 用默认json.net4.0时
        ///// 新版Json基本上正常
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this object obj)
        //{
        //    if (obj.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
        //    {
        //        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //    }

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("{");
        //    foreach (var tf in obj.GetType().GetProperties())
        //    {
        //        if (tf.GetGetMethod().GetParameters().Length > 0) continue;//索引器

        //        object value = null;
        //        try
        //        {
        //            value = tf.GetValue(obj);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogService.Exception(ex);
        //            continue;
        //        }
        //        if (value == null)
        //        {
        //            strbd.Append("\"" + tf.Name + "\":null,");
        //            continue;
        //        }

        //        var valtype = value.GetType();
        //        if (valtype.ToString().StartsWith("System.Collections.Generic.List"))//ToJsonString(value as IList) list
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + ToJsonString(value as IList) + ",");
        //        }
        //        else if (value as IDictionary != null)//ToJsonString(value as IList) list
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + ToJsonString(value as IDictionary) + ",");
        //        }
        //        else if (valtype == typeof(DataRowCollection))//ToJsonString(value as IList) list
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + ToJsonString(value as DataRowCollection) + ",");
        //        }
        //        else if (valtype == typeof(DataRow))//ToJsonString(value as IList) list
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + ToJsonString(value as DataRow) + ",");
        //        }
        //        else if (valtype.IsNumeric())//ToString() numeric
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + (value == null ? "" : value.ToString()) + ",");
        //        }
        //        else if (valtype == typeof(string))//\"ToString().Replace\" string
        //        {
        //            strbd.Append("\"" + tf.Name + "\":\"" + (value == null ? "" : value.ToString().Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"")) + "\",");
        //        }
        //        else if (valtype == typeof(DateTime))//\"ToString("yyyy-MM-dd HH:mm:ss.fffffff")\" string
        //        {
        //            //strbd.Append("\"" + tf.Name + "\":\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "\",");
        //            strbd.Append("\"" + tf.Name + "\":\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",");
        //        }
        //        else if (valtype == typeof(bool))//\"ToString()\" string
        //        {
        //            strbd.Append("\"" + tf.Name + "\":\"" + (value == null ? "" : value.ToString()) + "\",");
        //        }
        //        else//ToJsonString() object
        //        {
        //            strbd.Append("\"" + tf.Name + "\":" + value.ToJsonString() + ",");
        //        }
        //    }
        //    if (strbd.Length > 1)
        //        strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("}");
        //    return strbd.ToString();
        //    //return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //}

        ///// <summary>
        ///// 转Json 主要用于4.0平台 用默认json.net4.0时
        ///// 新版Json基本上正常
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this IList list)
        //{
        //    if (list == null) return "[]";
        //    if (list.Count < 1) return "[]";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("[");

        //    if (list.Count > 0 && list[0].GetType().IsNumeric())
        //    {
        //        foreach (var obj in list)
        //        {
        //            strbd.Append((obj != null ? obj.ToString() : "0") + ",");
        //        }
        //    }
        //    else if (list.Count > 0 && list[0].GetType() == typeof(string))
        //    {
        //        foreach (var obj in list)
        //        {
        //            strbd.Append("\"" + (obj == null ? "" : obj.ToString()) + "\",");
        //        }
        //    }
        //    else
        //    {
        //        foreach (var obj in list)
        //        {
        //            strbd.Append(obj.ToJsonString() + ",");
        //        }
        //    }
        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("]");
        //    return strbd.ToString();
        //}

        ///// <summary>
        ///// 转Json 主要用于4.0平台 用默认json.net4.0时
        ///// 新版Json基本上正常
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this DataRowCollection list)
        //{
        //    if (list == null) return "[]";
        //    if (list.Count < 1) return "[]";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("[");

        //    {
        //        foreach (DataRow row in list)
        //        {
        //            strbd.Append(row.ToJsonString());
        //        }
        //    }
        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("]");
        //    return strbd.ToString();
        //}

        ///// <summary>
        ///// 转Json 主要用于4.0平台 用默认json.net4.0时
        ///// 新版Json基本上正常
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this DataRow list)
        //{
        //    if (list == null) return "{}";
        //    if (list.Table.Columns.Count < 1) return "{}";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("{");

        //    {
        //        foreach (DataColumn col in list.Table.Columns)
        //        {
        //            strbd.Append("\"" + col.ColumnName + "\":\"" + list[col.ColumnName] + "\",");
        //        }
        //    }
        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("}");
        //    return strbd.ToString();
        //}
        ///// <summary>
        ///// 转Json 主要用于4.0平台 用默认json.net4.0时
        ///// 新版Json基本上正常
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this IDictionary list)
        //{
        //    if (list == null) return "{}";
        //    if (list.Count < 1) return "{}";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("{");
        //    {
        //        foreach (string key in list.Keys)
        //        {
        //            var val = list[key];
        //            var vtype = val.GetType();
        //            if (vtype == typeof(string))
        //            {
        //                strbd.Append("\"" + key + "\":\"" + list[key] + "\",");
        //            }
        //            else if(vtype.IsNumeric())
        //            {
        //                strbd.Append("\"" + key + "\":" + list[key] + ",");
        //            }
        //           else
        //            {
        //                strbd.Append("\"" + key + "\":" + list[key].ToJsonString() + ",");
        //            }
        //        }
        //    }

        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("}");
        //    return strbd.ToString();
        //}
        ///// <summary>
        ///// 转Json
        ///// 键值对集合
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this NameValueCollection list)
        //{
        //    if (list == null) return "{}";
        //    if (list.Count < 1) return "{}";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("{");
        //    {
        //        foreach (string key in list.Keys)
        //        {
        //            strbd.Append("\"" + key + "\":\"" + list[key]?.Replace("\"","\\\"") + "\",");
        //        }
        //    }

        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("}");
        //    return strbd.ToString();
        //}
        ///// <summary>
        ///// 转换头信息
        ///// </summary>
        ///// <param name="Headers"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this Net.WebHeaderCollection Headers)
        //{
        //    if (Headers == null) return "{}";
        //    if (Headers.Count < 1) return "{}";

        //    StringBuilder strbd = new StringBuilder();
        //    strbd.Append("{");
        //    {
        //        foreach (string key in Headers.AllKeys)
        //        {
        //            var values = Headers.GetValues(key);
        //            string valjson = values.Length == 1 ? values[0] : values.ToJsonString();
        //            strbd.Append("\"" + key + "\":\"" + valjson + "\",");
        //        }
        //    }
        //    strbd.Remove(strbd.Length - 1, 1);
        //    strbd.Append("}");
        //    return strbd.ToString();
        //}
    }
}

