using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void TablesListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;

            ((MainViewModel)this.DataContext).SelectTable(listBoxItem.DataContext.ToString());
        }

        private void FetchButton_Clicked(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)this.DataContext).SelectRows();
        }
    }
}
