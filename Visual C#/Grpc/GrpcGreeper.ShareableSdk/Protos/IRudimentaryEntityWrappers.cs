namespace GrpcGreeter.Protos;

public interface IRudimentaryEntityWrappers
{
  string NotQuiteAByteArrayWrapper { get; set; }

  DateTime MinDateWrapper { get; set; }

  DateTime MaxDateWrapper { get; set; }

  TimeSpan ShouldBeTimeSpanWrapper { get; set; }

  Guid ThisWillBeAGuidSomehowWrapper { get; set; }
}
