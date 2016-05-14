using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for EditCategory.xaml
	/// </summary>
	public partial class EditCategory : Window
	{
		private Category selectedCategory;
		private List<LeftProduct> deletionList = new List<LeftProduct>();
		private List<LeftProduct> additionList = new List<LeftProduct>();

		public EditCategory(Category selectedCategory)
		{
			InitializeComponent();
			this.selectedCategory = selectedCategory;

			selectedCategory.ProductList.Sort();
			_inCategoryListView.ItemsSource = selectedCategory.ProductList;

			var outcategory = MainWindow.products.Except(selectedCategory.ProductList).ToList();
			outcategory.Sort();

			_outCategoryListView.ItemsSource = outcategory;
		}


		//deletion checkbox checked
		private void _checkBox_Checked(object sender, RoutedEventArgs e)
		{
			LeftProduct selected = (sender as CheckBox).DataContext as LeftProduct;
			deletionList.Add(selected);
		}

		//deletion checkbox unchecked
		private void _checkBox_Unchecked(object sender, RoutedEventArgs e)
		{
			LeftProduct unselected = (sender as CheckBox).DataContext as LeftProduct;
			deletionList.Remove(unselected);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		//remove selected
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(deletionList.Count > 0)
			{
				MessageBoxResult result = MessageBox.Show("Toplam " + deletionList.Count.ToString() + " adet ürün kategoriden çıkarılacak. Onaylıyor musunuz ?", "Onayla", MessageBoxButton.OKCancel, MessageBoxImage.Question);

				if(result == MessageBoxResult.OK)
				{
					selectedCategory.ProductList = selectedCategory.ProductList.Except(deletionList).ToList();
					MessageBox.Show("Seçilen ürünler " + selectedCategory.Name + " isimli kategoriden çıkarıldı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
					Close();
				}
			}
		}

		//addition checkbox checked 
		private void _checkBox_Checked_1(object sender, RoutedEventArgs e)
		{
			LeftProduct selected = (sender as CheckBox).DataContext as LeftProduct;
			additionList.Add(selected);
		}

		//addition checkbox unchecked
		private void _checkBox_Unchecked_1(object sender, RoutedEventArgs e)
		{
			LeftProduct unselected = (sender as CheckBox).DataContext as LeftProduct;
			additionList.Remove(unselected);
		}

		//add selected
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (additionList.Count > 0)
			{
				MessageBoxResult result = MessageBox.Show("Toplam " + additionList.Count.ToString() + " adet ürün " + selectedCategory.Name + " isimli kategoriye eklenecek. Onaylıyor musunuz ?", "Onayla", MessageBoxButton.OKCancel, MessageBoxImage.Question);

				if (result == MessageBoxResult.OK)
				{
					selectedCategory.ProductList.AddRange(additionList);
					MessageBox.Show("Seçilen ürünler " + selectedCategory.Name + " isimli kategoriye eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
					Close();
				}
			}
		}
	}
}
