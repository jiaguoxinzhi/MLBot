using MLBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Extentions
{
    /// <summary>
    /// List 扩展
    /// </summary>
    public static class ListT_Extentions
    {
        /// <summary>
        /// 添加元素到指定位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<T> Add<T>(this List<T> list, int index,T item)
        {
            var tlist = new List<T>();
            if (index == 0)
            {
                tlist.Add(item);
                tlist.AddRange(list);
            }
            else if (index > 0)
            {
                tlist.AddRange(list.Take(index+1));
                tlist.Add(item);
                tlist.AddRange(list.Skip(index + 1));
            }
            return tlist;
        }
    }
}
