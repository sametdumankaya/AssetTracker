using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static List<LeftProduct> products = new List<LeftProduct>();
		public static List<Category> categories = new List<Category>();
		public static List<SpentProduct> spentProducts = new List<SpentProduct>();
		public static List<AddedProduct> addedProducts = new List<AddedProduct>();

		public static bool holder = false;

		public MainWindow()
		{
			InitializeComponent();

			LoadProductsFromJson();
			LoadCategoriesFromJson();
			LoadSpentFromJson();
			LoadAddedFromJson();

			products.Sort();
			categories.Sort();
			spentProducts.Sort();
			addedProducts.Sort();

			_listView.ItemsSource = categories;
			_dataGrid.ItemsSource = products;
		}

		private void _listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selected = _listView.SelectedItem;

			if (selected != null)
			{
				_textBlock.Text = (selected as Category).Name;
				_dataGrid.ItemsSource = (selected as Category).ProductList;
			}
			else
			{
				_textBlock.Text = "Tüm Stoklar";
			}		
		}

		

		public void ControlForEmptyCategories()
		{
			foreach (Category c in categories.ToList())
			{
				if(c.ProductList.Count == 0)
				{
					categories.Remove(c);
				}
			}
		}

		public void RefreshDataGrid()
		{
			_showAll.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
		}

		public void RefreshCategoryList()
		{
			_listView.Items.Refresh();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			WriteProductsToJson();
			WriteCategoriesToJson();
			WriteAddedToJson();
			WriteSpentToJson();

			base.OnClosing(e);
			Application.Current.Shutdown();
		}

		private void LoadSpentFromJson()
		{
			if (System.IO.File.Exists("spent.json"))
			{
				using (System.IO.StreamReader r = new System.IO.StreamReader("spent.json"))
				{
					string json = r.ReadToEnd();
					spentProducts = JsonConvert.DeserializeObject<List<SpentProduct>>(json);
				}
			}
		}

		private void LoadAddedFromJson()
		{
			if (File.Exists("added.json"))
			{
				using (System.IO.StreamReader r = new System.IO.StreamReader("added.json"))
				{
					string json = r.ReadToEnd();
					addedProducts = JsonConvert.DeserializeObject<List<AddedProduct>>(json);
				}
			}
		}

		public void WriteCategoriesToJson()
		{
			using (StreamWriter file = File.CreateText("categories.json"))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, categories);
			}
		}

		public void LoadCategoriesFromJson()
		{
			if (File.Exists("categories.json"))
			{
				using (StreamReader r = new StreamReader("categories.json"))
				{
					string json = r.ReadToEnd();
					categories = JsonConvert.DeserializeObject<List<Category>>(json);
				}
			}
		}

		public void WriteProductsToJson()
		{
			using (StreamWriter file = File.CreateText("products.json"))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, products);
			}
		}

		public void LoadProductsFromJson()
		{
			if (File.Exists("products.json"))
			{
				using (StreamReader r = new StreamReader("products.json"))
				{
					string json = r.ReadToEnd();
					products = JsonConvert.DeserializeObject<List<LeftProduct>>(json);
				}
			}
		}

		private void WriteSpentToJson()
		{
			using (System.IO.StreamWriter file = System.IO.File.CreateText("spent.json"))
			{
				Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
				serializer.Serialize(file, spentProducts);
			}
		}

		private void WriteAddedToJson()
		{
			using (System.IO.StreamWriter file = System.IO.File.CreateText("added.json"))
			{
				Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
				serializer.Serialize(file, addedProducts);
			}
		}

		//add stock
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			new AddStock().Show();
			_mainWindow.IsEnabled = false;
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			if (holder)
			{
				_mainWindow.IsEnabled = true;
				ControlForEmptyCategories();
				RefreshDataGrid();
				RefreshCategoryList();
				holder = !holder;
			}
		}

		//delete stock
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (_dataGrid.SelectedItem != null)
			{
				LeftProduct selectedProduct = _dataGrid.SelectedItem as LeftProduct;

				MessageBoxResult result = MessageBox.Show(selectedProduct.Name + " isimli ürünü gerçekten silmek istiyor musunuz? Bu ürün bütün kategorilerden de silinecektir.",
					"Onayla",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					foreach (Category c in categories)
					{
						foreach (LeftProduct p in c.ProductList.ToList())
						{
							if(p.Barcode.Equals(selectedProduct.Barcode))
							{
								c.ProductList.Remove(p);
							}
						}
					}

					foreach (LeftProduct p in products.ToList())
					{
						if(p.Barcode.Equals(selectedProduct.Barcode))
						{
							products.Remove(p);
						}
					}

					ControlForEmptyCategories();
					RefreshDataGrid();
					RefreshCategoryList();
					
					//programmatically click the button
					_showAll.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
				}
			}
			else
			{
				MessageBox.Show("Lütfen silmek istediğiniz stoğu tablodan seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		//rename stock
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (_dataGrid.SelectedItem != null)
			{
				LeftProduct selectedProduct = _dataGrid.SelectedItem as LeftProduct;
				new RenameStock(selectedProduct).Show();
				_mainWindow.IsEnabled = false;
			}
			else
			{
				MessageBox.Show("Lütfen adını değiştirmek istediğiniz stoğu tablodan seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}
		
		//show all stocks
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			_dataGrid.ItemsSource = null;
			_listView.ItemsSource = null;
			_dataGrid.ItemsSource = products;
			_listView.ItemsSource = categories;
			_textBlock.Text = "Tüm Stoklar";
		}

		//add category
		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			new AddCategory().Show();
			_mainWindow.IsEnabled = false;
		}

		//delete category
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			var selectedCategory = (_listView.SelectedItem as Category);

			if(selectedCategory != null)
			{
				MessageBoxResult result = MessageBox.Show(selectedCategory.Name + " isimli kategoriyi gerçekten silmek istiyor musunuz?",
					"Onayla",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					categories.Remove(selectedCategory);
					RefreshCategoryList();
					_showAll.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
				}	
			}
			else
			{
				MessageBox.Show("Lütfen silmek için bir kategori seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}		
		}
		
		//Rename category
		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			var selectedCategory = (_listView.SelectedItem as Category);

			if(selectedCategory != null)
			{
				new RenameCategory(selectedCategory, _textBlock).Show();
				_mainWindow.IsEnabled = false;
			}
			else
			{
				MessageBox.Show("Lütfen yeniden adlandırmak için bir kategori seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	
		//spend stock
		private void Button_Click_7(object sender, RoutedEventArgs e)
		{
			new SpendStock().Show();
			_mainWindow.IsEnabled = false;
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			new History().Show();
			_mainWindow.IsEnabled = false;
		}
		
		//delete category
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			var selectedCategory = (_listView.SelectedItem as Category);

			if (selectedCategory != null)
			{
				MessageBoxResult result = MessageBox.Show(selectedCategory.Name + " isimli kategoriyi gerçekten silmek istiyor musunuz?",
					"Onayla",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					categories.Remove(selectedCategory);
					RefreshCategoryList();
					_showAll.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
				}
			}
			else
			{
				MessageBox.Show("Lütfen silmek için bir kategori seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//rename category
		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			var selectedCategory = (_listView.SelectedItem as Category);

			if (selectedCategory != null)
			{
				new RenameCategory(selectedCategory, _textBlock).Show();
				_mainWindow.IsEnabled = false;
			}
			else
			{
				MessageBox.Show("Lütfen yeniden adlandırmak için bir kategori seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		
		//edit category
		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			var selectedCategory = (_listView.SelectedItem as Category);

			if(selectedCategory != null)
			{
				new EditCategory(selectedCategory).Show();
				_mainWindow.IsEnabled = false;
			}
			else
			{
				MessageBox.Show("Lütfen düzenlemek için bir kategori seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
