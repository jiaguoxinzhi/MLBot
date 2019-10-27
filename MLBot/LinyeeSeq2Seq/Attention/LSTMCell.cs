﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{


    [Serializable]
    public class LSTMCell 
    {
        public WeightMatrix Wix { get; set; }
        public WeightMatrix Wih { get; set; }
        public WeightMatrix bi { get; set; }

        public WeightMatrix Wfx { get; set; }
        public WeightMatrix Wfh { get; set; }
        public WeightMatrix bf { get; set; }

        public WeightMatrix Wox { get; set; }
        public WeightMatrix Woh { get; set; }
        public WeightMatrix bo { get; set; }

        public WeightMatrix Wcx { get; set; }
        public WeightMatrix Wch { get; set; }
        public WeightMatrix bc { get; set; }



        public WeightMatrix ht { get; set; }
        public WeightMatrix ct { get; set; }


        public int hdim { get; set; }
        public int dim { get; set; }


        public LSTMCell(int hdim, int dim)
        {

            this.Wix = new WeightMatrix(dim, hdim, true);
            this.Wih = new WeightMatrix(hdim, hdim,  true);
            this.bi = new WeightMatrix(1, hdim, 0);


            this.Wfx = new WeightMatrix(dim, hdim, true);
            this.Wfh = new WeightMatrix(hdim, hdim,  true);
            this.bf = new WeightMatrix(1, hdim, 0);


            this.Wox = new WeightMatrix(dim, hdim, true);
            this.Woh = new WeightMatrix(hdim, hdim,  true);
            this.bo = new WeightMatrix(1, hdim, 0);


            this.Wcx = new WeightMatrix(dim, hdim, true);
            this.Wch = new WeightMatrix(hdim, hdim,  true);
            this.bc = new WeightMatrix(1, hdim, 0);



            this.ht = new WeightMatrix(1, hdim, 0);
            this.ct = new WeightMatrix(1, hdim, 0);
            this.hdim = hdim;
            this.dim = dim;


        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="innerGraph"></param>
        /// <returns></returns>
        public  WeightMatrix Step(WeightMatrix input, ComputeGraph innerGraph)
        {
            var hidden_prev = ht;
            var cell_prev = ct;

            var cell = this;
            var h0 = innerGraph.mul(input, cell.Wix);
            var h1 = innerGraph.mul(hidden_prev, cell.Wih);
            var input_gate = innerGraph.sigmoid(
                innerGraph.add(
                    innerGraph.add(h0, h1),
                    cell.bi
                )
                );

            var h2 = innerGraph.mul(input, cell.Wfx);
            var h3 = innerGraph.mul(hidden_prev, cell.Wfh);
            var forget_gate = innerGraph.sigmoid(
                innerGraph.add(
                    innerGraph.add(h2, h3),
                    cell.bf
                )
                );

            var h4 = innerGraph.mul(input, cell.Wox);
            var h5 = innerGraph.mul(hidden_prev, cell.Woh);
            var output_gate = innerGraph.sigmoid(
                innerGraph.add(
                    innerGraph.add(h4, h5),
                    cell.bo
                )
                );

            var h6 = innerGraph.mul(input, cell.Wcx);
            var h7 = innerGraph.mul(hidden_prev, cell.Wch);
            var cell_write = innerGraph.tanh(
                innerGraph.add(
                    innerGraph.add(h6, h7),
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

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public virtual List<WeightMatrix> getParams()
        {
            List<WeightMatrix> response = new List<WeightMatrix>();

            response.Add(this.bc);
            response.Add(this.bf);
            response.Add(this.bi);
            response.Add(this.bo);
            response.Add(this.Wch);
            response.Add(this.Wcx);
            response.Add(this.Wfh);

            response.Add(this.Wfx);
            response.Add(this.Wih);
            response.Add(this.Wix);
            response.Add(this.Woh);
            response.Add(this.Wox);

            return response;
        }

        public void Reset()
        {
            ht = new WeightMatrix(1, hdim, 0);
            ct = new WeightMatrix(1, hdim, 0);
        }

    }
     
}
