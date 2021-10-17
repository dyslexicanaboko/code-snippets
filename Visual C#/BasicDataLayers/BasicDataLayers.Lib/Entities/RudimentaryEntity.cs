using System;

namespace BasicDataLayers.Lib.Entities
{
    /// <summary>
    /// Capturing some of the more rudimentary properties I have seen to test things that are the most common
    /// Default values used for the sake of testing
    /// </summary>
    public class RudimentaryEntity
    {
        public int PrimaryKey { get; set; } = 1;
        
        public int ForeignKey { get; set; } = 10;

        public Guid ReferenceId { get; set; } = Guid.Parse("903988d3-b96d-430b-a34b-bb1f0db7c9f7");

        public bool IsYes { get; set; } = true;

        public int LuckyNumber { get; set; } = 7;

        public decimal DollarAmount { get; set; } = 100;

        public double MathCalculation { get; set; } = 0.678593902;

        public string Label { get; set; } = "Poisonous";
        
        public DateTime RightNow { get; set; } = DateTime.UtcNow;
    }
}
