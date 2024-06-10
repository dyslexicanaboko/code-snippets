using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter.Protos;

namespace GrpcGreeter.Services
{
  public class RudimentaryService : Protos.RudimentaryService.RudimentaryServiceBase
  {
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
        ThisWillBeAGuidSomehow = Guid.Parse("903988d3-b96d-430b-a34b-bb1f0db7c9f7").ToString()
      });
    }

    /// <inheritdoc />
    public override Task<Empty> TakeSomeEntity(RudimentaryEntity request, ServerCallContext context)
    {
      Console.WriteLine($"Received: {request}");
      
      return Task.FromResult(new Empty());
    }
  }
}
