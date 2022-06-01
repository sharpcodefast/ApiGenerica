using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Infrastructure
{
    public static class MaintainCorsExtension
    {
        public static IApplicationBuilder MaintainCorsHeadersOnError(this IApplicationBuilder builder)
        {
            return builder.Use(async (httpContext, next) =>
            {
                var corsHeaders = new HeaderDictionary();
                foreach (var pair in httpContext.Response.Headers)
                {
                    if (!pair.Key.StartsWith("access-control-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                    corsHeaders[pair.Key] = pair.Value;
                }

                httpContext.Response.OnStarting(o =>
                {
                    var ctx = (HttpContext)o;
                    var headers = ctx.Response.Headers;
                    foreach (var pair in corsHeaders)
                    {
                        if (headers.ContainsKey(pair.Key)) { continue; }
                        headers.Add(pair.Key, pair.Value);
                    }
                    return Task.CompletedTask;
                }, httpContext);

                await next();
            });
        }
    }
}
