using System;
using System.Collections.Generic;
using System.Text;

namespace MLBot
{
    /// <summary>
    /// 不要为Null
    /// </summary>
    /// Linyee 2019-08-05
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullAttribute : Attribute
    {
        public NotNullAttribute()
        {

        }

        public override bool Match(object obj)
        {
            if (obj == null) return false;
            return true;
        }
    }
}
