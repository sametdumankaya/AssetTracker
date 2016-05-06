using System;

namespace AssetTracker
{
	public class LeftProduct : Product, IComparable<LeftProduct>, IEquatable<LeftProduct>
	{
		public LeftProduct(string barcode, string name, int count, DateTime lastDateAdded) : base(barcode, name)
		{
			this.count = count;
			this.lastDateAdded = lastDateAdded;
		}

		private int count;
		public int Count
		{
			get { return count; }
			set { count = value; }
		}

		private DateTime lastDateAdded;
		public DateTime LastDateAdded
		{
			get { return lastDateAdded; }
			set { lastDateAdded = value; }
		}

		public int CompareTo(LeftProduct other)
		{
			return Name.CompareTo(other.Name);
		}

		public bool Equals(LeftProduct other)
		{
			return Barcode.Equals(other.Barcode);
		}
	}
}
