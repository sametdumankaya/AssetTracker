using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for EditCategory.xaml
	/// </summary>
	public partial class EditCategory : Window
	{
		private Category selectedCategory;

		public EditCategory(Category selectedCategory)
		{
			InitializeComponent();
			this.selectedCategory = selectedCategory;

			_inCategoryListView.ItemsSource = selectedCategory.ProductList;
		}

		private void _checkBox_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void _checkBox_Unchecked(object sender, RoutedEventArgs e)
		{

		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}
	}
}
