namespace EqualityAndComparison.Lib.Entities
{
    public class ListEntity
    {
        public List<int> Sequence { get; set; }
        
        public List<string> SqlKeyWords { get; set; }
        
        public List<CompoundEntity> CompoundEntities { get; set; }
    }
}
