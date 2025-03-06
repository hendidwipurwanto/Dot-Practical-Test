using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Middleware
{
    public class ExceptionDotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionDotMiddleware> _logger;

        public ExceptionDotMiddleware(RequestDelegate next, ILogger<ExceptionDotMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "Something went wrong!",
                details = "Something went wrong on our side. Please try again later or please ask the Administrator!" //exception.Message 
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

}