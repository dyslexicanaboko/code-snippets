using ApiTemplate.Lib.Exceptions;
using ApiTemplate.Lib.Models.Client;
using Newtonsoft.Json;
using System.Net;

namespace ApiTemplate.WebApi
{
  public class ResponseMiddleware
  {
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ResponseMiddleware(ILogger logger, RequestDelegate next)
    {
      _logger = logger;

      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (InvalidArgumentException ex)
      {
        await Respond(context, HttpStatusCode.BadRequest, new BadRequestException(ex));
      }
      catch (BadRequestException ex)
      {
        await Respond(context, HttpStatusCode.BadRequest, ex);
      }
      catch (NotFoundException ex)
      {
        await Respond(context, HttpStatusCode.NotFound, ex);
      }
      catch (UnauthorizedException ex)
      {
        await Respond(context, HttpStatusCode.Unauthorized, ex);
      }
      catch (BaseException ex)
      {
        await Respond(context, HttpStatusCode.InternalServerError, ex);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unhandled exception in response middleware.");

        await Respond(context, HttpStatusCode.InternalServerError, new ErrorModel(ex));
      }
    }

    private static Task Respond(HttpContext context, HttpStatusCode httpStatusCode, BadRequestException ex)
    {
      var model = new ErrorModel(ex);

      return Respond(context, httpStatusCode, model);
    }

    private static Task Respond(HttpContext context, HttpStatusCode httpStatusCode, BaseException ex)
    {
      var model = new ErrorModel(ex);

      return Respond(context, httpStatusCode, model);
    }

    private static Task Respond(HttpContext context, HttpStatusCode httpStatusCode, ErrorModel model)
    {
      var jsonResponse = JsonConvert.SerializeObject(model);

      return Respond(context, httpStatusCode, jsonResponse);
    }

    private static async Task Respond(HttpContext context, HttpStatusCode httpStatusCode, string jsonResponse)
    {
      context.Response.StatusCode = (int)httpStatusCode;
      context.Response.ContentType = "application/json";

      await context.Response.WriteAsync(jsonResponse);
    }
  }
}
