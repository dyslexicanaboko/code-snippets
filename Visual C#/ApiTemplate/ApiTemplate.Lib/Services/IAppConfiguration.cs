namespace ApiTemplate.Lib.Services;

[ExcludeFromDiScan]
public interface IAppConfiguration
{
  string GetConnectionString();
}
