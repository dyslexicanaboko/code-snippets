using Microsoft.Extensions.Configuration;

namespace ApiTemplate.Lib.Services
{
  [ExcludeFromDiScan]
  public class AppConfiguration
    : IAppConfiguration
  {
    private readonly IConfiguration _configuration;

    public AppConfiguration(IConfiguration configuration) => _configuration = configuration;

    public string GetConnectionString()
    {
      var connectionString = _configuration.GetConnectionString("MillionsOfThings");

      return connectionString!;
    }
  }
}
