using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace GrpcGreeter.Protos
{
  //Adapting the generated entity with C# specific helper properties.
  //This partial is to be delivered as part of an SDK/NuGet.
  //Each property is being suffixed with "Wrapper" on purpose just to demonstrate the point
  //but really it should be named better for production level code.
  public static class RudimentaryEntityHelper
  {
    public static string NotQuiteAByteArrayWrapperGet(ByteString target)
    {
      return target.ToStringUtf8();
    }

    public static ByteString NotQuiteAByteArrayWrapperSet(string value)
    {
      return ByteString.CopyFromUtf8(value);
    }

    public static DateTime MinDateWrapperGet(Timestamp target)
    {
      return target.ToDateTime().ToLocalTime();
    }

    public static Timestamp MinDateWrapperSet(DateTime value)
    {
      //The fact that you have to convert your date from local to UTC is dangerous for the client.
      //It could be better to just send all datetime data as a string or a long
      return Timestamp.FromDateTime(value.ToUniversalTime());
    }

    public static DateTime MaxDateWrapperGet(Timestamp target)
    {
      //On the client side you have to convert from UTC to local, but this is machine specific which is also dangerous.
      return target.ToDateTime().ToLocalTime();
    }

    public static Timestamp MaxDateWrapperSet(DateTime value)
    {
      return Timestamp.FromDateTime(value.ToUniversalTime());
    }

    public static TimeSpan ShouldBeTimeSpanWrapperGet(Duration target)
    {
      return target.ToTimeSpan();
    }

    public static Duration ShouldBeTimeSpanWrapperSet(TimeSpan value)
    {
      return Duration.FromTimeSpan(value);
    }

    public static Guid ThisWillBeAGuidSomehowWrapperGet(string target)
    {
      return Guid.Parse(target);
    }

    public static string ThisWillBeAGuidSomehowWrapperSet(Guid value)
    {
      return value.ToString();
    }
  }
}
