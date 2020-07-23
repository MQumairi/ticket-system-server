using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e, logger);
            }
        }

        private async Task HandleException(HttpContext context, Exception e, ILogger logger)
        {
            object errors = null;

            switch (e)
            {
                case RestException re:
                logger.LogError(re, "REST ERROR");
                errors = re.Error;
                context.Response.StatusCode = (int) re.Code;
                break;
                case Exception ex:
                logger.LogError(ex, "SERVER ERROR");
                errors = ex.Message;
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                break;
            }

            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new {errors});

            await context.Response.WriteAsync(result);
        }
    }


}