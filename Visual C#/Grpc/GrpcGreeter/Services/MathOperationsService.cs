using Grpc.Core;

namespace GrpcGreeter.Services
{
  //https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start
  /*
  After adding a proto file you have to manually include it into the project (which is stupid) 
  <ItemGroup>
  <Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
  </ItemGroup>
   */
  public class MathOperationsService : MathOperations.MathOperationsBase
  {
    public override Task<MathResult> AddIntegers(TwoIntegers request, ServerCallContext context)
    {
      return Task.FromResult(new MathResult
      {
        Z = request.X + request.Y
      });
    }

    public override Task<MathResult> SubtractIntegers(TwoIntegers request, ServerCallContext context)
    {
      return Task.FromResult(new MathResult
      {
        Z = request.X - request.Y
      });
    }

    public override Task<MathResult> MultiplyIntegers(TwoIntegers request, ServerCallContext context)
    {
      return Task.FromResult(new MathResult
      {
        Z = request.X * request.Y
      });
    }

    public override Task<MathResult> DivideIntegers(TwoIntegers request, ServerCallContext context)
    {
      return Task.FromResult(new MathResult
      {
        Z = request.X / request.Y
      });
    }
  }
}
