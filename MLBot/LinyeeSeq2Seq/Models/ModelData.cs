using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinyeeSeq2Seq
{
    [Serializable]
    public class WeightMatrix  :Seq2SeqLearn.WeightMatrix
    {
        public WeightMatrix() : base() { }
        public WeightMatrix(double[] weights): base() { }
        public WeightMatrix(int rows, int columns, bool normal = false) : base() { }
        public WeightMatrix(int rows, int columns, double c) : base() { }
    }

    [Serializable]
    public class Encoder : Seq2SeqLearn.Encoder
    {
        public Encoder(int hdim, int dim, int depth) : base(hdim, dim, depth) { }

        public WeightMatrix Encode(WeightMatrix V, ComputeGraph g)
        {
            return (WeightMatrix) base.Encode((Seq2SeqLearn.WeightMatrix)V,(Seq2SeqLearn.ComputeGraph)g);
        }
        public List<WeightMatrix> Encode2(WeightMatrix V, ComputeGraph g)
        {
            return base.Encode2((Seq2SeqLearn.WeightMatrix)V, (Seq2SeqLearn.ComputeGraph)g).Select(p=>(WeightMatrix)p).ToList();
        }
        public new List<WeightMatrix> getParams()
        {
            return base.getParams().Select(p => (WeightMatrix)p).ToList();
        }
    }
    [Serializable]
    public class LinyeeDecoder : Seq2SeqLearn.AttentionDecoder
    {
        public LinyeeDecoder(int hdim, int dim, int depth) : base(hdim, dim, depth) { }
        public WeightMatrix Decode(WeightMatrix input, List<WeightMatrix> encoderOutput, ComputeGraph g)
        {
            return (WeightMatrix)base.Decode((Seq2SeqLearn.WeightMatrix)input, encoderOutput.Select(p=>(Seq2SeqLearn.WeightMatrix)p).ToList(),(Seq2SeqLearn.ComputeGraph)g);
        }
        public WeightMatrix Decode(WeightMatrix input, WeightMatrix encoderOutput, ComputeGraph g)
        {
            return (WeightMatrix)base.Decode((Seq2SeqLearn.WeightMatrix)input, (Seq2SeqLearn.WeightMatrix)encoderOutput, (Seq2SeqLearn.ComputeGraph)g);
        }


    }

    [Serializable]
    public class ComputeGraph : Seq2SeqLearn.ComputeGraph
    {
        public ComputeGraph(bool needBack = true) : base(needBack) { }
    }
}
