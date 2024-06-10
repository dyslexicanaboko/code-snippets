using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcGreeterClient;

public class Program
{
  public static async Task Main(string[] args)
  {
    // The port number must match the port of the gRPC server.
    using var channel = GrpcChannel.ForAddress("https://localhost:7025");

    //await GreeterTest(channel);

    //await MathOperationsTest(channel);

    //await DivideByZeroTest(channel);

    await RudimentaryEntityTest(channel);

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
  }

  private static async Task GreeterTest(ChannelBase channel)
  {
    var client = new Greeter.GreeterClient(channel);

    var reply = await client.SayHelloAsync(
      new HelloRequest { Name = "GreeterClient" });

    Console.WriteLine("Greeting: " + reply.Message);
  }

  private static async Task MathOperationsTest(ChannelBase channel)
  {
    var client = new MathOperations.MathOperationsClient(channel);

    var input = new TwoIntegers { X = 10, Y = 20 };

    var reply = await client.AddIntegersAsync(input);

    Console.WriteLine($"{input.X} + {input.Y} = {reply.Z}");
  }

  private static async Task DivideByZeroTest(ChannelBase channel)
  {
    var client = new MathOperations.MathOperationsClient(channel);

    var input = new TwoIntegers { X = 10, Y = 0 };

    MathResult reply;

    try
    {

      reply = await client.DivideIntegersAsync(input);
    }
    catch (RpcException ex)
    {
      Console.WriteLine("Operation could not complete due to error:");
      Console.WriteLine(ex.ToString());

      return;
    }

    Console.WriteLine($"{input.X} / {input.Y} = {reply.Z}");
  }

  private static async Task RudimentaryEntityTest(ChannelBase channel)
  {
    var client = new RudimentaryService.RudimentaryServiceClient(channel);

    var reply = await client.GetSomeEntityAsync(new Empty());

    Console.WriteLine("Entity: " + reply);

    reply.DoublePrecision = 44.0D;
    reply.FavoriteDayOfTheWeek = DayOfTheWeek.Sunday;
    reply.GuaranteedPositiveInteger = 77;
    reply.GuaranteedPositiveLong = 77L;
    reply.IsBit = false;
    reply.ListOfInt.Clear();
    reply.ListOfInt.AddRange(new []{ 5,5,5,5,5,5 });
    reply.NoOneReallyUsesFloat = 22.77F;
    reply.NotQuiteAByteArray = ByteString.CopyFromUtf8("All your base are belong to us");
    reply.PotentiallyNegativeInteger = -7;
    reply.PotentiallyNegativeLong = -7L;
    reply.MinDate = Timestamp.FromDateTime(new DateTime(1993, 8, 1).ToUniversalTime());
    reply.MaxDate = Timestamp.FromDateTime(DateTime.UtcNow);
    reply.ShouldBeTimeSpan = reply.MaxDate - reply.MinDate;
    reply.Text = "All your base are belong to us";
    reply.ThisWillBeAGuidSomehow = Guid.NewGuid().ToString();

    await client.TakeSomeEntityAsync(reply);
    
    Console.WriteLine("Entity sent back");
  }
}
