using System;

namespace AssetTracker
{
	public class SpentProduct : Product, IComparable<SpentProduct>, IEquatable<SpentProduct>
	{
		public SpentProduct(string barcode, string name, int spentCount, DateTime spendingDate) : base(barcode, name)
		{
			SpentCount = spentCount;
			this.spendingDate = spendingDate;
		}

		private int spentCount;
		public int SpentCount
		{
			get { return spentCount; }
			set { spentCount = value; }
		}

		private DateTime spendingDate;
		public DateTime SpendingDate
		{
			get { return spendingDate; }
			set { spendingDate = value; }
		}

		public int CompareTo(SpentProduct other)
		{
			return other.spendingDate.CompareTo(spendingDate);
		}

		public bool Equals(SpentProduct other)
		{
			return Name.Equals(other.Name);
		}
	}
}
