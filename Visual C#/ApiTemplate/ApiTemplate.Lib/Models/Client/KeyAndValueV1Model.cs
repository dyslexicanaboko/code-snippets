namespace ApiTemplate.Lib.Models.Client
{
  public class KeyAndValueV1Model<TEnumeration>
    where TEnumeration : struct, Enum
  {
    private readonly TEnumeration _enumeration;

    public KeyAndValueV1Model(int enumerationId) => _enumeration = Enum.Parse<TEnumeration>(enumerationId.ToString());

    public KeyAndValueV1Model(TEnumeration enumeration) => _enumeration = enumeration;

    public int Id => Convert.ToInt32(_enumeration);

    public string Description => Convert.ToString(_enumeration)!;
  }
}
