using System;

namespace BasicDataLayers.Entities
{
    /// <summary>
    /// Capturing some of the more rudimentary properties I have seen to test things that are the most common
    /// </summary>
    public class RudimentaryEntity
    {
        public bool IsYes { get; set; }

        public int LuckyNumber { get; set; }
        
        public decimal DollarAmount { get; set; }
        
        public double MathCalculation { get; set; }

        public string Label { get; set; }
        
        public DateTime RightNow { get; set; }
    }
}
