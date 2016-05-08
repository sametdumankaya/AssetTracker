using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AssetTracker
{
	/// <summary>
	/// Interaction logic for History.xaml
	/// </summary>
	public partial class History : Window
	{
		public History()
		{
			InitializeComponent();

			_spentDataGrid.ItemsSource = MainWindow.spentProducts;
			_addedDataGrid.ItemsSource = MainWindow.addedProducts;

			PopulateSpentComboBox();
			PopulateAddedComboBox();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainWindow.holder = !MainWindow.holder;
			base.OnClosing(e);
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void _start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			FilterSpentDataGrid();
		}

		private void PopulateSpentComboBox()
		{
			List<SpentProduct> list = new List<SpentProduct>();

			list.Add(new SpentProduct("-1", "Hepsi", -1, System.DateTime.Now));

			foreach (SpentProduct sp in MainWindow.spentProducts)
			{
				if (!list.Contains(sp))
				{
					list.Add(sp);
				}
			}

			_spentComboBox.ItemsSource = list;
			_spentComboBox.SelectedIndex = 0;
		}

		private void PopulateAddedComboBox()
		{
			List<AddedProduct> list = new List<AddedProduct>();

			list.Add(new AddedProduct("-2", "Hepsi", -2, System.DateTime.Now));

			foreach (AddedProduct sp in MainWindow.addedProducts)
			{
				if (!list.Contains(sp))
				{
					list.Add(sp);
				}
			}

			_addedComboBox.ItemsSource = list;
			_addedComboBox.SelectedIndex = 0;
		}

		private void _start2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			FilterAddedDataGrid();
		}

		//spent products filtered by name
		private void _comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FilterSpentDataGrid();
		}

		//added products filtered by name
		private void _addedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FilterAddedDataGrid();
		}

		private void FilterSpentDataGrid()
		{
			var selectedStartDate = _start.SelectedDate;
			var selectedEndDate = _end.SelectedDate;
			var selectedStockName = (_spentComboBox.SelectedItem as SpentProduct).Name;

			if(selectedStockName.Equals("Hepsi"))
			{
				if(selectedStartDate != null && selectedEndDate != null)
				{
					var newList = new List<SpentProduct>();

					foreach (SpentProduct sp in MainWindow.spentProducts)
					{
						if (sp.SpendingDate >= selectedStartDate && sp.SpendingDate <= selectedEndDate)
						{
							newList.Add(sp);
						}
					}

					_spentDataGrid.ItemsSource = newList;
					CalculateSpentProductCount();
				}
				else
				{
					_spentDataGrid.ItemsSource = MainWindow.spentProducts;
					CalculateSpentProductCount();
				}
			}
			else
			{
				List<SpentProduct> newList = new List<SpentProduct>();

				if (selectedStartDate != null && selectedEndDate != null)
				{
					foreach (SpentProduct sp in MainWindow.spentProducts)
					{
						if (sp.SpendingDate >= selectedStartDate && sp.SpendingDate <= selectedEndDate && sp.Name.Equals(selectedStockName))
						{
							newList.Add(sp);
						}
					}
				}
				else
				{
					foreach (SpentProduct sp in MainWindow.spentProducts)
					{
						if (sp.Name.Equals(selectedStockName))
						{
							newList.Add(sp);
						}
					}
				}

				_spentDataGrid.ItemsSource = newList;
				CalculateSpentProductCount();
			}
		}

		private void FilterAddedDataGrid()
		{
			var selectedStartDate = _start2.SelectedDate;
			var selectedEndDate = _end2.SelectedDate;
			var selectedStockName = (_addedComboBox.SelectedItem as AddedProduct).Name;

			if (selectedStockName.Equals("Hepsi"))
			{
				if (selectedStartDate != null && selectedEndDate != null)
				{
					var newList = new List<AddedProduct>();

					foreach (AddedProduct ap in MainWindow.addedProducts)
					{
						if (ap.AddingDate >= selectedStartDate && ap.AddingDate <= selectedEndDate)
						{
							newList.Add(ap);
						}
					}

					_addedDataGrid.ItemsSource = newList;
					CalculateAddedProductCount();
				}
				else
				{
					_addedDataGrid.ItemsSource = MainWindow.addedProducts;
					CalculateAddedProductCount();
				}
			}
			else
			{
				List<AddedProduct> newList = new List<AddedProduct>();

				if (selectedStartDate != null && selectedEndDate != null)
				{
					foreach (AddedProduct ap in MainWindow.addedProducts)
					{
						if (ap.AddingDate >= selectedStartDate && ap.AddingDate <= selectedEndDate && ap.Name.Equals(selectedStockName))
						{
							newList.Add(ap);
						}
					}
				}
				else
				{
					foreach (AddedProduct ap in MainWindow.addedProducts)
					{
						if (ap.Name.Equals(selectedStockName))
						{
							newList.Add(ap);
						}
					}
				}

				_addedDataGrid.ItemsSource = newList;
				CalculateAddedProductCount();
			}
		}

		private void CalculateSpentProductCount()
		{
			double result = 0;

			foreach (SpentProduct sp in _spentDataGrid.Items)
			{
				result += sp.SpentCount;
			}

			if(result > 0)
			{
				_totalProduct.Text = "Toplamda " + result.ToString() + " adet ürün bulundu.";
			}
			else
			{
				_totalProduct.Text = "Bu kriterlere uyan hiçbir ürün bulunamadı.";
			}

		}

		private void CalculateAddedProductCount()
		{
			double result = 0;

			foreach (AddedProduct sp in _addedDataGrid.Items)
			{
				result += sp.AddCount;
			}

			if (result > 0)
			{
				_totalProduct2.Text = "Toplamda " + result.ToString() + " adet ürün bulundu.";
			}
			else
			{
				_totalProduct2.Text = "Bu kriterlere uyan hiçbir ürün bulunamadı.";
			}
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(_spentTab.IsSelected)
			{
				_textBlock.Text = "Harcananlar";
			}
			else
			{
				_textBlock.Text = "Eklenenler";
			}
		}
	}
}
