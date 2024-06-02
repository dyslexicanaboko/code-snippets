using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcGreeterClient;

public class Program
{
  public static async Task Main(string[] args)
  {
    // The port number must match the port of the gRPC server.
    using var channel = GrpcChannel.ForAddress("https://localhost:7025");

    await GreeterTest(channel);

    await MathOperationsTest(channel);

    await DivideByZeroTest(channel);

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
}
