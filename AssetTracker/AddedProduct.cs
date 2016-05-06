using System;

namespace AssetTracker
{
	public class AddedProduct : Product, IComparable<AddedProduct>, IEquatable<AddedProduct>
	{
		public AddedProduct(string barcode, string name, int addCount, DateTime addingDate) : base(barcode, name)
		{
			this.addCount = addCount;
			this.addingDate = addingDate;
		}

		private int addCount;
		public int AddCount
		{
			get { return addCount; }
			set { addCount = value; }
		}

		private DateTime addingDate;
		public DateTime AddingDate
		{
			get { return addingDate; }
			set { addingDate = value; }
		}

		public int CompareTo(AddedProduct other)
		{
			return other.addingDate.CompareTo(addingDate);
		}

		public bool Equals(AddedProduct other)
		{
			return Name.Equals(other.Name);
		}
	}
}
