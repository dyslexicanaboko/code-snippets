namespace EqualityAndComparison.Lib.Entities
{
	public class CompoundEntity
		: IEquatable<CompoundEntity>
	{
		public string Label { get; set; }

		public FlatEntity NestedEntity { get; set; }

		public override bool Equals(object obj) => Equals(obj as CompoundEntity);

		public bool Equals(CompoundEntity other)
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

			var areEqualLabel = Label.AreEqualIgnoreCase(other.Label);
			var areEqualFlatEntity = NestedEntity == other.NestedEntity; //operator has to be used because left hand could be null

			//For the sake of debugging, it's a good idea to keep the
			//result of each complex compare in a separate variable.
			var areEqual =
				areEqualLabel &&
				areEqualFlatEntity;

			return areEqual;
		}

		public override int GetHashCode()
		{
			//The same concerns that applied to the equality apply here
			var hc =
				Label.GetObjectHashCode() +
				NestedEntity.GetObjectHashCode(); //IEquatable<T> definitions are used

			return hc;
		}

		public static bool operator ==(CompoundEntity lhs, CompoundEntity rhs)
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

		public static bool operator !=(CompoundEntity lhs, CompoundEntity rhs) => !(lhs == rhs);
	}
}
