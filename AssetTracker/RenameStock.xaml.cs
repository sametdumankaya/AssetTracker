using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for RenameStock.xaml
	/// </summary>
	public partial class RenameStock : Window
	{
		LeftProduct productToRename;

		public RenameStock(LeftProduct stockToRename)
		{
			InitializeComponent();
			productToRename = stockToRename;
		}
		
		//rename stock
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(_newStockName.Text.Trim().Length == 0)
			{
				MessageBox.Show("Lütfen yeni stok ismi alanına bir isim girin", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				foreach (LeftProduct p in MainWindow.products)
				{
					if(p.Name.Equals(_newStockName.Text.Trim()))
					{
						MessageBoxResult result = MessageBox.Show("Bu isimde bir ürün zaten var. Yine de değiştirmek istiyor musunuz?", "Hata", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

						if(result == MessageBoxResult.Yes)
						{
							productToRename.Name = _newStockName.Text.Trim();
							MessageBox.Show("Stok ismi başarıyla değiştirildi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
							Close();
						}
						return;
					}
				}
				productToRename.Name = _newStockName.Text.Trim();
				MessageBox.Show("Stok ismi başarıyla değiştirildi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		private void _newStockName_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
			{
				_ok.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			}
		}
	}
}
