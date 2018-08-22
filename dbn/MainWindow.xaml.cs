using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace dbn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void ConnectionsListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;
            DbConnectionInfo dbConnectionInfo = listBoxItem.DataContext as DbConnectionInfo;

            ((MainViewModel)this.DataContext).ConnectToDb(dbConnectionInfo);
        }

        private void FetchButton_Clicked(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> columnValuePairs = new Dictionary<string, string>();

            ListBox columnsListBox = ColumnsListBox;
            for(int i = 0; i < ColumnsListBox.Items.Count; i++)
            {
                ListBoxItem listBoxItem = ColumnsListBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);

                DataTemplate dataTemplate = contentPresenter.ContentTemplate;
                TextBlock keyTextBlock = (TextBlock)dataTemplate.FindName("ColumnName", contentPresenter);
                TextBox valueTextBox = (TextBox)dataTemplate.FindName("ColumnValue", contentPresenter);

                if(!String.IsNullOrEmpty(valueTextBox.Text) && !String.IsNullOrWhiteSpace(valueTextBox.Text))
                {
                    columnValuePairs.Add(keyTextBlock.Text, valueTextBox.Text);
                }
            }

            if(columnValuePairs.Count > 0)
            {
                ((MainViewModel)this.DataContext).SelectRowsMatching(columnValuePairs);
            }
            else
            {
                ((MainViewModel)this.DataContext).SelectRows();
            }
        }

        // Source: https://docs.microsoft.com/en-us/dotnet/framework/wpf/data/how-to-find-datatemplate-generated-elements
        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void ResultsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // Selected cells change when a cell is selected and the grid was reloaded,
            // since no new cells are selected, we need to return
            if(dataGrid.SelectedCells.Count == 0)
            {
                return;
            }

            string selectedColumn = dataGrid.SelectedCells[0].Column.Header.ToString();
            List<string> tablesWhereColumnPresent = ((MainViewModel)this.DataContext).getTableNamesWhereColumnPresent(selectedColumn);

            ContextMenu contextMenu = (ContextMenu) ResultsDataGrid.Resources["CtxMenu"];

            contextMenu.Items.Clear();
            foreach(string table in tablesWhereColumnPresent)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Click += ContextMenuItem_Click;
                menuItem.Header = table;

                contextMenu.Items.Add(menuItem);
            }
        }

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string table;
            string column;
            string value;

            MenuItem menuItem = (MenuItem)sender;
            table = menuItem.Header.ToString();

            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            DataGrid dataGrid = ResultsDataGrid;

            column = dataGrid.SelectedCells[0].Column.Header.ToString();

            var cellInfo = dataGrid.SelectedCells[0];
            TextBlock content = (TextBlock)cellInfo.Column.GetCellContent(cellInfo.Item);

            value = content.Text;


            Dictionary<string, string> columnValuePairs = new Dictionary<string, string>();
            columnValuePairs.Add(column, value);

            ((MainViewModel)this.DataContext).CurrentTable = table;
            ((MainViewModel)this.DataContext).SelectRowsMatching(columnValuePairs);
        }
    }
}
