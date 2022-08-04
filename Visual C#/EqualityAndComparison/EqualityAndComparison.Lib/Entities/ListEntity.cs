namespace EqualityAndComparison.Lib.Entities
{
	public class ListEntity
		: IEquatable<ListEntity>
	{
		public List<int> Sequence { get; set; }
		
		public List<string> SqlKeyWords { get; set; }
		
		public List<CompoundEntity> CompoundEntities { get; set; }

		public override bool Equals(object obj) => Equals(obj as ListEntity);

		public bool Equals(ListEntity other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (GetType() != other.GetType())
			{
				return false;
			}

			/* Comparing and equating complex object will have complex code. Unfortunately there is no "right"
			 * way to do this because it's mostly subjective on how this is implemented. Since this object
			 * has lists in it, this now introduces new complexities:
			 *   1. Are the lists distinct? This will be the biggest problem by far. 
			 *   2. When comparing strings does case matter? */
			var areEqualSequence = Sequence.SequenceEqual(other.Sequence);
			var areEqualSqlKeywords = SqlKeyWords.SequenceEqual(other.SqlKeyWords);
			var areEqualCompoundEntities = CompoundEntities.SequenceEqual(other.CompoundEntities);

			//For the sake of debugging, it's a good idea to keep the
			//result of each complex compare in a separate variable.
			var areEqual =
				areEqualSequence &&
				areEqualSqlKeywords &&
				areEqualCompoundEntities;

			return areEqual;
		}
		
		public override int GetHashCode()
		{
			//The same concerns that applied to the equality apply here
			var hc =
				Sequence.GetListHashCode() + //Default compare is used
				SqlKeyWords.GetListHashCode() + //Default compare is used
				CompoundEntities.GetListHashCode(); //IEquatable<T> definitions are used

			return hc;
		}

		public static bool operator ==(ListEntity lhs, ListEntity rhs)
		{
			if (lhs is null)
			{
				if (rhs is null)
				{
					return true;
				}

				return false;
			}

			return lhs.Equals(rhs);
		}

		public static bool operator !=(ListEntity lhs, ListEntity rhs) => !(lhs == rhs);
	}
}
