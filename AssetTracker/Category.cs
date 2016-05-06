using System;
using System.Collections.Generic;

namespace AssetTracker
{
	public class Category : IComparable<Category>, IEquatable<Category>
	{
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private List<LeftProduct> productList;
		public List<LeftProduct> ProductList
		{
			get { return productList; }
			set { productList = value; }
		}

		public Category(string name)
		{
			this.name = name;
			productList = new List<LeftProduct>();
		}

		public int CompareTo(Category other)
		{
			return name.CompareTo(other.name);
		}

		public bool Equals(Category other)
		{
			return name.Equals(other.name);
		}
	}
}
