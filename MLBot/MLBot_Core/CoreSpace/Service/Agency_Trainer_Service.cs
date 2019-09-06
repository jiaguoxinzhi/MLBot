using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace MLBot
{
    /// <summary>
    /// Agency_Trainer 服务
    /// </summary>
    [Author("Linyee", "2019-07-16")]
    public class Agency_Trainer_Service : ServiceBase<Agency_Trainer>, IAgency_Trainer_Service
    {

        [Author("Linyee", "2019-07-16")]
        public new ExecuteResult<Agency_Trainer> Update(Agency_Trainer atrain)
        {
            var res= base.Update(atrain);

            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load Data
            IDataView data = mlContext.Data.LoadFromEnumerable(ChatData.Sample);

            // Define data preparation estimator
            IEstimator<ITransformer> dataPrepEstimator =
                mlContext.Transforms.Concatenate("Features", new string[] { "Size", "HistoricalPrices" })
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                    ;
            // Create data preparation transformer
            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(data);
            //Console.WriteLine("数据准备 填充完成");
            // Pre-process data using data prep operations
            IDataView transformedData = dataPrepTransformer.Transform(data);
            //Console.WriteLine($"数据视图： {Newtonsoft.Json.JsonConvert.SerializeObject(mlContext.Data.CreateEnumerable<TransformedHousingData>(transformedData, true))}");

            // Define StochasticDualCoordinateAscent regression algorithm estimator
            var sdcaEstimator = mlContext.Regression.Trainers.Sdca();//labelColumnName: "Label", featureColumnName: "Features"
            //var sdcaEstimator = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            //Console.WriteLine("正在训练");
            // Train regression model
            RegressionPredictionTransformer<LinearRegressionModelParameters> trainedModel = sdcaEstimator.Fit(transformedData);
            //Console.WriteLine("训练完成");

            // Save Data Prep transformer
            mlContext.Model.Save(dataPrepTransformer, data.Schema, "data_preparation_pipeline.zip");

            // Save Trained Model
            mlContext.Model.Save(trainedModel, transformedData.Schema, "model11.zip");
            Console.WriteLine("预训数据 保存完成");

            return res;
        }
    }
}
