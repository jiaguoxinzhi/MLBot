using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBot
{
    /// <summary>
    /// 修改者
    /// </summary>
    /// Linyee 2019-04-26
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ModifierAttribute : Attribute
    {
        /// <summary>
        /// 修改者
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 描述 修改原因等
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; private set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public ModifierAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 修改者
        /// </summary>
        public ModifierAttribute(string name, DateTime dt) : this(name)
        {
            this.ModifyTime = dt;
        }

        /// <summary>
        /// 修改者
        /// </summary>
        public ModifierAttribute(string name, string dt) : this(name)
        {
            this.ModifyTime = DateTime.Parse(dt);
        }

        /// <summary>
        /// 修改者
        /// </summary>
        public ModifierAttribute(string name, DateTime dt, string desc) : this(name, dt)
        {
            this.Description = desc;
        }

        /// <summary>
        /// 修改者
        /// </summary>
        public ModifierAttribute(string name, string dt, string desc) : this(name, dt)
        {
            this.Description = desc;
        }
    }
}
