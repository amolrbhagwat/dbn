using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dbn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string displayMessage = "An unhandled exception just occurred: " +
                                    e.Exception.Message +
                                    "\n" +
                                    e.Exception.StackTrace;

            MessageBox.Show(displayMessage, "An Exception occurred");
            e.Handled = true;
        }
    }
}
