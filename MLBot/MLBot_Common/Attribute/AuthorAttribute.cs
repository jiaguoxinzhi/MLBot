using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot.Attributes
{
    /// <summary>
    /// 作者
    /// </summary>
    /// Linyee 2019-04-26
    public class AuthorAttribute : Attribute
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorAttribute(string name, DateTime dt) : this(name)
        {
            this.CreateTime = dt;
        }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorAttribute(string name, string dt) : this(name)
        {
            this.CreateTime = DateTime.Parse(dt);
        }
    }
}
