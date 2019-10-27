
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{

    [Serializable]
    public class Encoder
    {
        public List<LSTMCell> encoders = new List<LSTMCell>();

        public int hdim { get; set; }
        public int dim { get; set; }
        public int depth { get; set; }

        //public Encoder(Seq2SeqLearn.Encoder encoder) : this(encoder.hdim,encoder.dim,encoder.depth)
        //{

        //}

        public Encoder(int hdim, int dim, int depth )
        {
             encoders.Add(new LSTMCell(hdim, dim));
 
            //for (int i = 1; i < depth; i++)
            //{
            //   encoders.Add(new LSTMCell(hdim, hdim));
 
            //}

            this.hdim = hdim;
            this.dim = dim;
            this.depth = depth;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            foreach (var item in encoders)
            {
                item.Reset();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="V"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public WeightMatrix Encode(WeightMatrix V, ComputeGraph g)
        {
            foreach (var encoder in encoders)
            {
                var e = encoder.Step(V, g); 
                    V = e;   
            }
            return V;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="V"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public List<WeightMatrix> Encode2(WeightMatrix V, ComputeGraph g)
        {
            List<WeightMatrix> res = new List<WeightMatrix>();
            foreach (var encoder in encoders)
            {
                var e = encoder.Step(V, g);
                V = e;
                res.Add(e);
            }
            return res;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public List<WeightMatrix> getParams()
        {
            List<WeightMatrix> response = new List<WeightMatrix>();

            foreach (var item in encoders)
            {
                response.AddRange(item.getParams());
            }
            return response;
        }

    }
}
