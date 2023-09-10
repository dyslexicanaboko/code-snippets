namespace EqualityAndComparison.Lib.Entities
{
  /// <summary>
  /// Referring to this as a Dumb Entity only because it does not implement any of the
  /// comparison and equality interfaces.
  /// </summary>
  public class DumbEntity : IFlatEntity
  {
    public DumbEntity()
    {
      
    }

    public DumbEntity(IFlatEntity entity)
    {
      PrimaryKey = entity.PrimaryKey;
      Age = entity.Age;
      Price = entity.Price;
      JobCode = entity.JobCode;
      SignificantDate = entity.SignificantDate;
    }

    public Guid PrimaryKey { get; set; }

    public int Age { get; set; }

    public decimal Price { get; set; }

    public string JobCode { get; set; }

    public DateTime SignificantDate { get; set; }
  }
}