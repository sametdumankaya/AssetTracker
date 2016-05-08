using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for AddStock.xaml
	/// </summary>
	public partial class AddStock : Window
	{
		public AddStock()
		{
			InitializeComponent();
		}

		private void AddStockInCategories(string barcode, int count)
		{
			foreach (Category cat in MainWindow.categories)
			{
				foreach (LeftProduct lp in cat.ProductList)
				{
					if (lp.Barcode.Equals(barcode))
					{
						lp.Count += count;
						lp.LastDateAdded = DateTime.Now;
						break;
					}
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (_barcode.Text.Trim().Length == 0)
			{
				MessageBox.Show("Lütfen barkod alanına tıklayıp barkodu okutun.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else if (_stockName.Text.Trim().Length == 0)
			{
				MessageBox.Show("Lütfen stok ismi alanına bir isim girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else if (_count.Text.Trim().Length == 0)
			{
				MessageBox.Show("Lütfen adet alanına bir sayı girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else
			{
				int countToAdd;
				string barcodeToAdd = _barcode.Text.Trim();
				string stockNameToAdd = _stockName.Text.Trim();

				try
				{
					countToAdd = Convert.ToInt32(_count.Text.Trim());
				}
				catch (Exception ex)
				{
					MessageBox.Show("Lütfen adet alanına bir sayı girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (countToAdd != 0)
				{
					foreach (LeftProduct p in MainWindow.products)
					{
						if (p.Barcode.Equals(barcodeToAdd))
						{
							MessageBox.Show("Bu ürün daha önce stoğa eklenmiş. Stok sayısı artırıldı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Information);
							p.Count += countToAdd;
							p.LastDateAdded = DateTime.Now;
							MainWindow.addedProducts.Add(new AddedProduct(barcodeToAdd, stockNameToAdd, countToAdd, DateTime.Now));
							AddStockInCategories(barcodeToAdd, countToAdd);
							CleanAndFocus();
							return;
						}
					}

					foreach (LeftProduct p in MainWindow.products)
					{
						if (p.Name.Equals(stockNameToAdd))
						{
							MessageBoxResult result = MessageBox.Show("Farklı barkod numarası ancak aynı isimde bir stok daha önce girilmiş. Yine de eklemek istiyor musunuz?", "Onayla", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

							if (result == MessageBoxResult.Yes)
							{
								LeftProduct productToAdd2 = new LeftProduct(barcodeToAdd, stockNameToAdd, countToAdd, DateTime.Now);
								MainWindow.products.Add(productToAdd2);
								MainWindow.addedProducts.Add(new AddedProduct(barcodeToAdd, stockNameToAdd, countToAdd, DateTime.Now));
								MessageBox.Show("Ürün stoğa başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
								AddStockInCategories(barcodeToAdd, countToAdd);
								CleanAndFocus();
							}
							return;
						}
					}

					LeftProduct productToAdd = new LeftProduct(barcodeToAdd, stockNameToAdd, countToAdd, DateTime.Now);
					MainWindow.products.Add(productToAdd);
					MainWindow.addedProducts.Add(new AddedProduct(barcodeToAdd, stockNameToAdd, countToAdd, DateTime.Now));
					MessageBox.Show("Yeni ürün stoğa başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
					CleanAndFocus();
					return;
				}
				else
				{
					MessageBox.Show("Stok sayısı sıfır olamaz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}
		}

		private void CleanAndFocus()
		{
			_barcode.Text = "";
			_stockName.Text = "";
			_count.Text = "";
			_barcode.Focus();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		private void _barcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		private static bool IsTextAllowed(string text)
		{
			Regex regex = new Regex("[^0-9]+");
			return !regex.IsMatch(text);
		}

		private void _count_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				_ok.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			}
		}

		private void _barcode_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				var barcode = _barcode.Text.Trim();

				if (barcode.Length == 0)
				{
					_barcode.Focus();
					return;
				}


				LeftProduct productToAdd = FindProduct(barcode);

				if (productToAdd != null)
				{
					_stockName.Text = productToAdd.Name;
					_count.Focus();
				}
				else
				{
					_stockName.Focus();
				}
			}
		}

		private LeftProduct FindProduct(string barcode)
		{
			foreach (LeftProduct p in MainWindow.products)
			{
				if (p.Barcode.Equals(barcode))
				{
					return p;
				}
			}
			return null;
		}
	}
}
