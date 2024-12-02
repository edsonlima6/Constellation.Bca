using Constellation.Bca.Domain.Exceptions;
using System.Net;
using System;
using System.Text.Json;

namespace Constellation.Bca.HostWebApi.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(OperatorNotAllowedException op)
            {
                await HandlerExceptionMessage(context, op);
            }
            catch (Exception ex)
            {
                await HandlerExceptionMessage(context, ex);
            }
        }

        private async Task HandlerExceptionMessage(HttpContext context, Exception op)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMessage = op.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
