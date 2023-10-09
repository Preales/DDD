using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly  ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) 
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
         await next(context);
        }
        catch (Exception e)
        {            
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ProblemDetails problem = new ()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Server Error",
                Type = "Server Error",
                Detail = "An internal server has ocurred."
            };

            string json =  JsonSerializer.Serialize(problem);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(json);
        }
    }
}