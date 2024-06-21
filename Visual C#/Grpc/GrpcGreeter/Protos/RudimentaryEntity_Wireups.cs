using static GrpcGreeter.Protos.RudimentaryEntityHelper;

namespace GrpcGreeter.Protos
{
  //Adapting the generated entity with C# specific helper properties.
  //This partial would need to be delivered as part of an SDK/NuGet.
  //Each property is being suffixed with "Wrapper" on purpose just to demonstrate the point
  //but really it should be named better for production level code.
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
