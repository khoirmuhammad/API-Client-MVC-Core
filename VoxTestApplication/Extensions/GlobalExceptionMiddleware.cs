using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Net;
using VoxTestApplication.Data;
using VoxTestApplication.Models;
using VoxTestApplication.Repositories;

namespace VoxTestApplication.Extensions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;
        private readonly IWebHostEnvironment env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext httpContext, IApplicationLogRepository appRepository)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                ApplicationLog log = new ApplicationLog();

                int statusCode = GetStatusCodeFromException(ex);

                if (env.IsDevelopment())
                {
                    log.ErrorMessage = ex.Message;
                    log.Level = "Error";
                    log.User = httpContext.User.Identity?.Name ?? string.Empty;
                    log.Exception = ex.StackTrace ?? string.Empty;
                }
                else
                {
                    log.ErrorMessage = ex.Message;
                    log.Level = "Error";
                    log.User = httpContext.User.Identity?.Name ?? string.Empty;
                    log.Exception = ex.StackTrace ?? string.Empty;
                }

                await appRepository.SaveLog(log);

                httpContext.Response.Redirect("Error");
            }
        }

        private int GetStatusCodeFromException(Exception ex)
        {
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                return (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                return (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
