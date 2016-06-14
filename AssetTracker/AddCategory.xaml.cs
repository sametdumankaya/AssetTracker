using System.ComponentModel;
using System.Windows;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for AddCategory.xaml
	/// </summary>
	public partial class AddCategory : Window
	{
		public AddCategory()
		{
			InitializeComponent();
			_listView.ItemsSource = MainWindow.products;
		}

		//add new category
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(_categoryName.Text.Trim().Length == 0)
			{
				MessageBox.Show("Lütfen kategori ismi alanına bir isim girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else if(_listView.SelectedItems.Count == 0)
			{
				MessageBox.Show("Lütfen en az bir adet ürün seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else
			{
				foreach (Category c in MainWindow.categories)
				{
					if(c.Name.Equals(_categoryName.Text.Trim()))
					{
						MessageBox.Show("Bu isimde bir kategori zaten var.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
				}

				string categoryName = _categoryName.Text.Trim();
				Category newCategory = new Category(categoryName);

				foreach (LeftProduct p in _listView.SelectedItems)
				{
					newCategory.ProductList.Add(new LeftProduct(p.Barcode, p.Name, p.Count, p.LastDateAdded));
				}

				MainWindow.categories.Add(newCategory);
				MessageBox.Show("Yeni kategori başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
				return;
			}
		}

		//On closing window
		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}
	}
}
