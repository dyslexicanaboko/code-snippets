using System.Net;
using ApiTemplate.Lib.Exceptions;

namespace ApiTemplate.Lib.Models.Client
{
  public class ErrorModel
  {
    public ErrorModel(string message, int errorCode)
    {
      Code = errorCode;

      Message = message;
    }

    public ErrorModel(string message, int errorCode, IEnumerable<InvalidArgumentException> exceptions)
      : this(message, errorCode)
    {
      Fields = exceptions.Select(x => new InvalidArgumentModel(x)).ToList();
    }

    public ErrorModel(Exception exception)
      : this(exception.Message, (int)HttpStatusCode.InternalServerError)
    {
    }

    public ErrorModel(BaseException exception)
      : this(exception.Message, exception.ErrorCode)
    {
    }

    public ErrorModel(BadRequestException exception)
      : this(exception.Message, exception.ErrorCode, exception.InvalidArguments)
    {
    }

    public int Code { get; set; }

    public string Message { get; set; }

    public IList<InvalidArgumentModel> Fields { get; set; } = new List<InvalidArgumentModel>();
  }
}
