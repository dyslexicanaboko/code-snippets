using GrpcGreeter.Protos;
using static GrpcGreeter.Protos.RudimentaryEntityHelper;

namespace GrpcGreeterClient
{
  //Imagine that this is using a shared SDK as a NuGet and not a direct DLL reference.
  //I did that just for simplicity.
  //This code is copied, but the essence of what it's doing is shared via NuGet.
  public partial class RudimentaryEntity 
    : IRudimentaryEntityWrappers
  {
    /// <inheritdoc />
    public string NotQuiteAByteArrayWrapper
    {
      get => NotQuiteAByteArrayWrapperGet(NotQuiteAByteArray);
      set => NotQuiteAByteArrayWrapperSet(value);
    }

    /// <inheritdoc />
    public DateTime MinDateWrapper
    {
      get => MinDateWrapperGet(MinDate);
      set => MinDateWrapperSet(value);
    }

    /// <inheritdoc />
    public DateTime MaxDateWrapper
    {
      get => MaxDateWrapperGet(MaxDate);
      set => MaxDateWrapperSet(value);
    }

    /// <inheritdoc />
    public TimeSpan ShouldBeTimeSpanWrapper
    {
      get => ShouldBeTimeSpanWrapperGet(ShouldBeTimeSpan);
      set => ShouldBeTimeSpanWrapperSet(value);
    }

    /// <inheritdoc />
    public Guid ThisWillBeAGuidSomehowWrapper
    {
      get => ThisWillBeAGuidSomehowWrapperGet(ThisWillBeAGuidSomehow);
      set => ThisWillBeAGuidSomehowWrapperSet(value);
    }
  }
}
