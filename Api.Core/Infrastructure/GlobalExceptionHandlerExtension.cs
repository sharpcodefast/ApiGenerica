using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Api.Core.Dtos.Common;
using System;
using System.Net;
using System.Text;

namespace Api.Core.Infrastructure
{
    public static class GlobalExceptionHandlerExtension
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                    string errorDetails = $@"{exception.Message}
                                            {Environment.NewLine}
                                            {exception.StackTrace}";

                    int statusCode = (int)HttpStatusCode.InternalServerError;

                    context.Response.StatusCode = statusCode;

                    var responseDetails = new Response<object>();
                    responseDetails.Result.HasErrors = true;
                    responseDetails.Result.Messages.Add(errorDetails);

                    if (exception is ValidationException) {
                        responseDetails.Result = (exception as ValidationException).Result;
                    }

                    var serializerSettings = new JsonSerializerSettings();
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    var json = JsonConvert.SerializeObject(responseDetails, serializerSettings);
                                  
                    context.Response.ContentType = "application/json; charset=utf-8";

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                });
            });
        }
    }
}
