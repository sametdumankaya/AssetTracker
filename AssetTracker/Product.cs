using System;

namespace AssetTracker
{
	public class Product : IComparable<Product>, IEquatable<Product>
    {
		public Product(string barcode, string name)
		{
			this.barcode = barcode;
			this.name = name;
		}

		private string barcode;
		public string Barcode
		{
			get { return barcode; }
		}

		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int CompareTo(Product other)
		{
			return name.CompareTo(other.name);
		}

		public bool Equals(Product other)
		{
			return barcode.Equals(other.barcode);
		}

		public override int GetHashCode()
		{
			return Barcode.GetHashCode();
		}
	}
}
