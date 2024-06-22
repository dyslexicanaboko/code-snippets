using GrpcGreeter.Protos;
using static GrpcGreeter.Protos.RudimentaryEntityHelper;

namespace GrpcGreeterClient
{
  //Imagine that this is using a shared SDK as a NuGet and not a direct DLL reference.
  //I did that just for simplicity.
  //This code is copied, but the essence of what it's doing is shared via NuGet.
  //This will keep the client and server in sync on logic.
  public partial class RudimentaryEntity 
    : IRudimentaryEntityWrappers
  {
    /// <inheritdoc />
    public string NotQuiteAByteArrayWrapper
    {
      get => NotQuiteAByteArrayWrapperGet(NotQuiteAByteArray);
      set => NotQuiteAByteArray = NotQuiteAByteArrayWrapperSet(value);
    }

    /// <inheritdoc />
    public DateTime MinDateWrapper
    {
      get => MinDateWrapperGet(MinDate);
      set => MinDate = MinDateWrapperSet(value);
    }

    /// <inheritdoc />
    public DateTime MaxDateWrapper
    {
      get => MaxDateWrapperGet(MaxDate);
      set => MaxDate = MaxDateWrapperSet(value);
    }

    /// <inheritdoc />
    public TimeSpan ShouldBeTimeSpanWrapper
    {
      get => ShouldBeTimeSpanWrapperGet(ShouldBeTimeSpan);
      set => ShouldBeTimeSpan = ShouldBeTimeSpanWrapperSet(value);
    }

    /// <inheritdoc />
    public Guid ThisWillBeAGuidSomehowWrapper
    {
      get => ThisWillBeAGuidSomehowWrapperGet(ThisWillBeAGuidSomehow);
      set => ThisWillBeAGuidSomehow = ThisWillBeAGuidSomehowWrapperSet(value);
    }
  }
}
