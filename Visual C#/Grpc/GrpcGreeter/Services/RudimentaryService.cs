using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter.Protos;
using Newtonsoft.Json;

namespace GrpcGreeter.Services
{
  public class RudimentaryService : Protos.RudimentaryService.RudimentaryServiceBase
  {
    private const string TestGuidString = "903988d3-b96d-430b-a34b-bb1f0db7c9f7";

    /// <inheritdoc />
    public override Task<RudimentaryEntity> GetSomeEntity(Empty request, ServerCallContext context)
    {
      return Task.FromResult(new RudimentaryEntity
      {
        DoublePrecision = 10.2D,
        FavoriteDayOfTheWeek = DayOfTheWeek.Thursday,
        GuaranteedPositiveInteger = 42,
        GuaranteedPositiveLong = 42L,
        IsBit = true,
        ListOfInt = { 1,2,3,4,5 },
        NoOneReallyUsesFloat = 3.14F,
        NotQuiteAByteArray = ByteString.CopyFromUtf8("Hello, World!"),
        PotentiallyNegativeInteger = -99,
        PotentiallyNegativeLong = -99L,
        //Conversion from DateTime to Timestamp requires the DateTime kind to be Utc (Parameter 'dateTime')'
        MinDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()),
        MaxDate = Timestamp.FromDateTime(DateTime.MaxValue.ToUniversalTime()),
        ShouldBeTimeSpan = Duration.FromTimeSpan(TimeSpan.FromDays(1)),
        Text = "Hello, World!",
        ThisWillBeAGuidSomehow = Guid.Parse(TestGuidString).ToString()
      });
    }

    public override Task<RudimentaryEntity> GetSomeEntityUsingHelpers(Empty request, ServerCallContext context)
    {
      return Task.FromResult(new RudimentaryEntity
      {
        DoublePrecision = 10.2D,
        FavoriteDayOfTheWeek = DayOfTheWeek.Thursday,
        GuaranteedPositiveInteger = 42,
        GuaranteedPositiveLong = 42L,
        IsBit = true,
        ListOfInt = { 1, 2, 3, 4, 5 },
        NoOneReallyUsesFloat = 3.14F,
        NotQuiteAByteArrayWrapper = "Hello, World!",
        PotentiallyNegativeInteger = -99,
        PotentiallyNegativeLong = -99L,
        MinDateWrapper = DateTime.Parse("2024-06-20 10:00:00"),
        MaxDateWrapper = DateTime.Parse("2024-06-21 23:19:52"),
        ShouldBeTimeSpanWrapper = TimeSpan.FromDays(1),
        Text = "Hello, World!",
        ThisWillBeAGuidSomehowWrapper = new Guid(TestGuidString)
      });
    }

    /// <inheritdoc />
    public override Task<Empty> TakeSomeEntity(RudimentaryEntity request, ServerCallContext context)
    {
      var prettyJson = JsonConvert.SerializeObject(request, Formatting.Indented);

      Console.WriteLine($"Received:\n {prettyJson}");
      
      return Task.FromResult(new Empty());
    }
  }
}
