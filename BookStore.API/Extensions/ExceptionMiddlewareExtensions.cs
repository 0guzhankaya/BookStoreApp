using BookStore.Entities.ErrorModel;
using BookStore.Entities.Exceptions;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BookStore.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        // WebApplication için eklenti method.
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFuture = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFuture is not null)
                    {
                        context.Response.StatusCode = contextFuture.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFuture.Error}");

                        await context.Response.WriteAsync(new ErrorDetails() { 
                            StatusCode = context.Response.StatusCode,
                            Message = contextFuture.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
