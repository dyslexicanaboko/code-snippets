namespace EqualityAndComparison.Lib.Entities
{
	public class FlatEntity
		: IEquatable<FlatEntity>
	{
		//Not recommended to include primary keys in comparisons and equality. You should be comparing
		//the substance of your object not the keys
		public Guid PrimaryKey { get; set; }

		public int Age { get; set; }

		public decimal Price { get; set; }

		public string JobCode { get; set; }

		public DateTime SignificantDate { get; set; }

		//Base object override required outside of the IEquatable context - best to define it or risk having problems
		public override bool Equals(object obj) => Equals(obj as FlatEntity);


		//From IEquatable<T> which is necessary for correctly using methods like .Distinct()
		//Will not work if THIS object is null
		public bool Equals(FlatEntity other)
		{
			//Check if the right hand argument is null
			if (other is null)
			{
				return false;
			}

			// Optimization for a common success case. If these are the same object, then they are equal.
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			// If run-time types are not exactly the same, return false.
			if (GetType() != other.GetType())
			{
				return false;
			}

			//Compare logic taylored to the substance of the class
			var areEqual =
				Age == other.Age &&
				Price == other.Price &&
				JobCode == other.JobCode && //Use case insensitiviy only if it matters
				SignificantDate == other.SignificantDate; //Choose whether to compare just the date; or the date and time

			return areEqual;
		}

		//This type of code is only good as a stand in. It serves no purpose otherwise because all it will do is
		//create a new anonymous object and return the hashcode on it. This is essentially a random number which
		//is useless for sorting and comparisons.
		//public override int GetHashCode() => (Disable, Path, RetentionDays).GetHashCode();

		//Getting the hashcode can be as simple as the sum of its parts. Some people try to multiply their
		//sum by a prime number, but I find this futile.
		public override int GetHashCode()
		{
			//The same concerns that applied to the equality apply here
			var hc = 
				Age.GetHashCode() +
				Price.GetHashCode() +
				JobCode.GetObjectHashCode() + //Case sensitivity and strings are nullable by nature
				SignificantDate.GetHashCode(); //Date vs. Date and Time

			return hc;
		}

		//Optionally you can override the equality operator.
		//This is excellent to use if you don't know if Left or Right hand sides are null
		public static bool operator ==(FlatEntity lhs, FlatEntity rhs)
		{
			if (lhs is null)
			{
				if (rhs is null)
				{
					return true;
				}

				// Only the left side is null.
				return false;
			}

			// Equals handles case of null on right side.
			return lhs.Equals(rhs);
		}

		//Optionally you can override the not-equal operator.
		//Just the inverse of ==
		public static bool operator !=(FlatEntity lhs, FlatEntity rhs) => !(lhs == rhs);
	}
}
