using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.WebApi.Controllers
{
  public class AppBaseController
    : ControllerBase
  {
    public int UserId { get; set; } = 1; //TODO: Stand in for actual UserId
  }
}
