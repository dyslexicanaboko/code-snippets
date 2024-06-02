namespace EqualityAndComparison.Lib.Entities;

/// <summary> This interface is just being used as a convenient way to not have to make new dummy data. </summary>
public interface IFlatEntity
{
  Guid PrimaryKey { get; set; }

  int Age { get; set; }

  decimal Price { get; set; }

  string JobCode { get; set; }

  DateTime SignificantDate { get; set; }
}
