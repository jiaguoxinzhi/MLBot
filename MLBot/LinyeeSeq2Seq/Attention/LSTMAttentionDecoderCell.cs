﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{


    [Serializable]
    public class LSTMDecoderCell : LSTMCell 
    {
        public WeightMatrix WiC { get; set; }
        public WeightMatrix WfC { get; set; }
        public WeightMatrix WoC { get; set; }

        public WeightMatrix WcC { get; set; }


        //public LSTMDecoderCell(Seq2SeqLearn.LSTMAttentionDecoderCell decoder) : this(decoder.hdim, decoder.dim)
        //{
        //    WiC =new WeightMatrix( decoder.WiC);
        //    WfC = new WeightMatrix(decoder.WfC);
        //    WoC = new WeightMatrix(decoder.WoC);
        //    WcC = new WeightMatrix(decoder.WcC);

        //    bc = new WeightMatrix(decoder.bc);
        //    bf = new WeightMatrix(decoder.bf);
        //    bi = new WeightMatrix(decoder.bi);
        //    bo = new WeightMatrix(decoder.bo);

        //    ct = new WeightMatrix(decoder.ct);
        //    ht = new WeightMatrix(decoder.ht);
        //    Wch = new WeightMatrix(decoder.Wch);
        //    Wcx = new WeightMatrix(decoder.Wcx);

        //    Wfh = new WeightMatrix(decoder.Wfh);
        //    Wfx = new WeightMatrix(decoder.Wfx);
        //    Wih = new WeightMatrix(decoder.Wih);
        //    Wix = new WeightMatrix(decoder.Wix);

        //    Woh = new WeightMatrix(decoder.Woh);
        //    Wox = new WeightMatrix(decoder.Wox);
        //}



        public LSTMDecoderCell(int hdim, int dim)
            : base(hdim, dim)
        {
            int contextSize = hdim * 2;
            this.WiC = new WeightMatrix(contextSize, hdim, true); 
            this.WfC = new WeightMatrix(contextSize, hdim, true); 
            this.WoC = new WeightMatrix(contextSize, hdim, true); 
            this.WcC = new WeightMatrix(contextSize, hdim, true);

        }

        public WeightMatrix Step(WeightMatrix context, WeightMatrix input, ComputeGraph innerGraph)
        {

            var hidden_prev = ht;
            var cell_prev = ct;

            var cell = this;
           var h0 = innerGraph.mul(input, cell.Wix);
            var h1 = innerGraph.mul(hidden_prev, cell.Wih);
            var h11 = innerGraph.mul(context, cell.WiC);
            var input_gate = innerGraph.sigmoid(
                innerGraph.add(innerGraph.add(innerGraph.add(h0, h1), h11), cell.bi));


             var h2 = innerGraph.mul(input, cell.Wfx); 
            var h3 = innerGraph.mul(hidden_prev, cell.Wfh);
            var h33 = innerGraph.mul(context, cell.WfC);
            var forget_gate = innerGraph.sigmoid(
                innerGraph.add(innerGraph.add(innerGraph.add(h3, h2), h33),
                    cell.bf
                )
                );



            var h4 = innerGraph.mul(input, cell.Wox);
            var h5 = innerGraph.mul(hidden_prev, cell.Woh);
            var h55 = innerGraph.mul(context, cell.WoC);
            var output_gate = innerGraph.sigmoid(
                innerGraph.add(
                innerGraph.add(innerGraph.add(h5, h4), h55),
                    cell.bo
                )
                );




            var h6 = innerGraph.mul(input, cell.Wcx);
            var h7 = innerGraph.mul(hidden_prev, cell.Wch);
            var h77 = innerGraph.mul(context, cell.WcC);
            var cell_write = innerGraph.tanh(
                innerGraph.add(
                innerGraph.add(innerGraph.add(h7, h6), h77),
                    cell.bc
                )
                );

            // compute new cell activation
            var retain_cell = innerGraph.eltmul(forget_gate, cell_prev); // what do we keep from cell
            var write_cell = innerGraph.eltmul(input_gate, cell_write); // what do we write to cell
            var cell_d = innerGraph.add(retain_cell, write_cell); // new cell contents



            // compute hidden state as gated, saturated cell activations
            var hidden_d = innerGraph.eltmul(output_gate, innerGraph.tanh(cell_d));


            this.ht = hidden_d;
            this.ct = cell_d;

            return ht;
        }

        public override List<WeightMatrix> getParams()
        {
            List<WeightMatrix> response = new List<WeightMatrix>();
            response.AddRange(base.getParams()); 
            
            response.Add(this.WiC);
            response.Add(this.WfC);
            response.Add(this.WoC);
            response.Add(this.WcC);
            return response;
        }
         

    }


}
