using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{
    /// <summary>
    /// 事件包
    /// </summary>
    public class CostEventArg : EventArgs
    {
        public double Cost { get; set; }
         
        /// <summary>
        /// 迭代
        /// </summary>
        public int Iteration { get; set; }
    }
}
