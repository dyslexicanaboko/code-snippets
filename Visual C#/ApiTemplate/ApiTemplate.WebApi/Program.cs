using Newtonsoft.Json;

namespace ApiTemplate.WebApi;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    // Add services to the container.
    ContainerConfig.Configure(builder.Host);

    //JWT Authentication should go here

    //https://stackoverflow.com/questions/70554844/asp-net-core-6-web-api-making-fields-required
    //The controller was making non-nullable properties required without my permission
    builder.Services
      .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
      .AddNewtonsoftJson(
        options =>
        {
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
          options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
          options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.Logger.LogInformation("InStock API is running");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();

      SetLooseCorsPolicyForDevelopmentPurposesOnly(app);
    }

    //When working with Next.js you may have to disable https to do local development
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<ResponseMiddleware>(app.Logger);

    app.Run();
  }

  private static void SetLooseCorsPolicyForDevelopmentPurposesOnly(IApplicationBuilder app)
  {
    app.UseCors(
      policy =>
      {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
      });
  }
}
