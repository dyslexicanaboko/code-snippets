using ApiTemplate.Lib;
using ApiTemplate.Lib.Services;
using Serilog;
using System.Reflection;

namespace ApiTemplate.WebApi
{
  public static class ContainerConfig
  {
    public static void Configure(IHostBuilder host)
    {
      host.ConfigureServices((_, services) =>
          {
            //Common registrations
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IDateTimeService, DateTimeService>();

            var asm = Assembly.Load("ApiTemplate.Lib");

            //Namespaces that must be excluded from the DI scan
            var excludeNamespaces = asm.GetTypes()
              .Where(t => 
                t.Namespace != null && 
                (t.Namespace.Contains("ApiTemplate.Lib.Entities") ||
                  t.Namespace.Contains("ApiTemplate.Lib.Models")))
              .Select(t => t.Namespace)
              .Distinct()
              .ToArray();

            services.Scan(scan =>
            {
              scan.FromAssemblies(asm)
                .AddClasses(classes => 
                  classes.NotInNamespaces(excludeNamespaces)
                    .WithoutAttribute<ExcludeFromDiScanAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime();
            });
          })
        .UseSerilog(
          (hostContext, loggerConfiguration) =>
          {
            //TODO: Make this configurable
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Log.log");

            //Since this is configured here, don't do it in the JSON also otherwise the logging will appear twice
            loggerConfiguration
              .ReadFrom.Configuration(hostContext.Configuration)
              .WriteTo.File(path, rollingInterval: RollingInterval.Day);

            if (hostContext.HostingEnvironment.IsDevelopment())
              loggerConfiguration.WriteTo.Seq("http://localhost:5341");
          });
    }
  }
}
