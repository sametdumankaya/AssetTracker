using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for RenameCategory.xaml
	/// </summary>
	public partial class RenameCategory : Window
	{
		Category c;
		TextBlock text;

		public RenameCategory(Category categoryToRename, TextBlock text)
		{
			InitializeComponent();
			c = categoryToRename;
			this.text = text;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var str = _newCategoryName.Text.Trim();

			if(str.Length != 0)
			{

				foreach (Category c in MainWindow.categories)
				{
					if(c.Name.Equals(str))
					{
						MessageBox.Show("Bu isimde bir kategori zaten var.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
				}

				c.Name = str;
				MessageBox.Show("Kategori ismi başarıyla değiştirildi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
				text.Text = str;
				Close();
			}
			else
			{
				MessageBox.Show("Lütfen yeni bir kategori ismi girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		private void _newCategoryName_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
			{
				_ok.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			}
		}
	}
}
