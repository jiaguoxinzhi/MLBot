using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinyeeSeq2Seq
{

    /// <summary>
    /// 权重矩阵
    /// </summary>
    [Serializable]
    public class WeightMatrix
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 列数 每个词的数据量
        /// </summary>
        public int Columns { get; set; } 
        /// <summary>
        /// 权重
        /// </summary>
        public double[] Weight { get; set; }
        /// <summary>
        /// 梯度
        /// </summary>
        public double[] Gradient { get; set; }
        /// <summary>
        /// 现金？
        /// </summary>
        public double[] Cash { get; set; }

        //#region 移植使用
        //public WeightMatrix(Seq2SeqLearn.WeightMatrix matrix):this(matrix.Rows, matrix.Columns, 0.0)
        //{
        //    var n = this.Weight.Length;
        //    for (int i = 0; i < n; i++)
        //    {
        //        this.Weight[i] = matrix.Weight[i];
        //    }
        //}
        //#endregion

        /// <summary>
        /// 权重矩阵
        /// </summary>
        public WeightMatrix()
        {

        }
        /// <summary>
        /// 权重矩阵
        /// </summary>
        /// <param name="weights"></param>
        public WeightMatrix(double[] weights)
        {
            this.Rows = weights.Length;
            this.Columns = 1; 
            this.Weight = new double[this.Rows];
            this.Gradient = new double[this.Rows];
            this.Cash = new double[this.Rows];
            this.Weight = weights ;             
        }

        /// <summary>
        /// 权重矩阵
        /// Weight[i] = RandomGenerator.NormalRandom(0.0, scale)
        /// </summary>
        /// <param name="rows">行</param>
        /// <param name="columns">列</param>
        /// <param name="normal"></param>
        public WeightMatrix(int rows, int columns,  bool normal=false)
        {
            this.Rows = rows;
            this.Columns = columns; 

            var n = rows * columns  ;
            this.Weight = new double[n];
            this.Gradient = new double[n];
            this.Cash = new double[n];

            var scale = Math.Sqrt(1.0 / (rows * columns ));
            if (normal)
            {
                scale = 0.08;
            }
            for (int i = 0; i < n; i++)
            {
                this.Weight[i] = RandomGenerator.NormalRandom(0.0, scale);  
            }

        }
        /// <summary>
        /// 权重矩阵
        /// Weight[i] = c
        /// </summary>
        /// <param name="rows">行</param>
        /// <param name="columns">列</param>
        /// <param name="c">权重初始值</param>
        public WeightMatrix(int rows, int columns, double c)
        {
            this.Rows = rows;
            this.Columns = columns; 

            var n = rows * columns  ;
            this.Weight = new double[n];
            this.Gradient = new double[n];
            this.Cash = new double[n]; 

            for (int i = 0; i < n; i++)
            {
                this.Weight[i] = c;
            }

        }

        public override string ToString()
        {
            
            return "{"+Rows.ToString()+","+Columns.ToString()+"}";
        }
        public double Get(int x, int y)
        {
            var ix = ((this.Columns * x) + y)  ;
            return this.Weight[ix];
        }

        public void Set(int x, int y, double v)
        {
            var ix = ((this.Columns * x) + y)  ;
              this.Weight[ix]=v;
        }

        public void Add(int x, int y, double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Weight[ix] += v;
        }

        public double Get_Grad(int x, int y )
        {
            var ix = ((this.Columns * x) + y)  ;
            return this.Gradient[ix];
        }

        public void Set_Grad(int x, int y,   double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Gradient[ix] = v;
        }

        public void Add_Grad(int x, int y,  double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Gradient[ix] += v;
        }

        public WeightMatrix CloneAndZero()
        {
            return new WeightMatrix(this.Rows, this.Columns,   0.0);

        }

        public WeightMatrix Clone()
        {
            var v= new WeightMatrix(this.Rows, this.Columns,  0.0);
            var n = this.Weight.Length;
            for (int i = 0; i < n; i++)
            {
                v.Weight[i] = this.Weight[i];
            }
            return v;
        }
 
    }




}
