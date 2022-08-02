using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualityAndComparison.Lib.Entities
{
    public class CompoundEntity
    {
        public string Label { get; set; }

        public FlatEntity NestedEntity { get; set; }
    }
}
