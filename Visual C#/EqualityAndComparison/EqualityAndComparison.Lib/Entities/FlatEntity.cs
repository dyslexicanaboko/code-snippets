namespace EqualityAndComparison.Lib.Entities
{
    public class FlatEntity
    {
        public Guid PrimaryKey { get; set; }

        public int Age { get; set; }

        public decimal Price { get; set; }

        public string JobCode { get; set; }

        public DateTime SignificantDate { get; set; }
    }
}
