﻿using CompanyManagementService.DataAccess.Exceptions;
using System.Net;

namespace CompanyManagementServiceApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _exceptionLogger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> exceptionLogger)
        {
            _next = next;
            _exceptionLogger = exceptionLogger ?? throw new ArgumentNullException(nameof(exceptionLogger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogWarning("Exception in ExceptionHandlerMiddleware with text \"{message}\"", ex.Message);
                _exceptionLogger.LogWarning("Exception in ExceptionHandlerMiddleware with text \"{StackTrace}\"", ex.StackTrace);

                var response = httpContext.Response;

                response.StatusCode = ex switch
                {
                    InvalidModelStateException => (int)HttpStatusCode.BadRequest,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    Exception => (int)HttpStatusCode.InternalServerError,
                };

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
