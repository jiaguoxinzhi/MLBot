using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MLBot.Mvc
{
    /// <summary>
    /// Swagger 文件上传参数
    /// </summary>
    [Author("Linyee", "2019-07-05")]//从bot搬过来的
    public class SwaggerFileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "Import")
            {
                operation.Parameters.Clear();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload Zip File， meta.json is the extra data for customized function.",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
