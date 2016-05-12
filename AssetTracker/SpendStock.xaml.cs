using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for SpendStock.xaml
	/// </summary>
	public partial class SpendStock : Window
	{
		List<string> spent;

		public SpendStock()
		{
			InitializeComponent();
			spent = new List<string>();
		}

		private void AlertUserForLowStock(LeftProduct lp)
		{
			MessageBox.Show(lp.Name + " isimli üründen " + lp.Count.ToString() +  " adet kaldı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		private void DropStockFromCategories(string barcode)
		{
			foreach (Category cat in MainWindow.categories)
			{
				foreach (LeftProduct lp in cat.ProductList)
				{
					if(lp.Barcode.Equals(barcode))
					{
						lp.Count--;
						break;
					}
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		private void _barcode_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				var barcode = _barcode.Text.Trim();

				if(barcode.Length == 0)
				{
					return;
				}

				LeftProduct productToSpent = FindProduct(barcode);

				if (productToSpent != null)
				{
					if (productToSpent.Count > 0)
					{
						productToSpent.Count--;
						spent.Add(productToSpent.Name);
						MainWindow.spentProducts.Add(new SpentProduct(productToSpent.Barcode, productToSpent.Name, 1, DateTime.Now));
						_listView.Items.Add(productToSpent.Name);
						_barcode.Text = "";
						DropStockFromCategories(productToSpent.Barcode);

						if(productToSpent.Count < 11)
						{
							AlertUserForLowStock(productToSpent);
						}

					}
					else
					{
						MessageBox.Show("Bu üründen stokta hiç kalmamış. Ürünü harcamadan önce stoğa ekleyin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
						_barcode.Text = "";
					}
				}
				else
				{
					MessageBox.Show("Bu ürün stokta yok. Ürünü harcamadan önce stoğa ekleyin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
					_barcode.Text = "";
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Toplam " + _listView.Items.Count.ToString() + " adet ürün harcandı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
			Close();
		}
	}
}
