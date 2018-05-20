using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dbn
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DbAccessor dbAccessor;
        public List<DbConnectionInfo> Connections { get; private set; }

        private ObservableCollection<string> tables;
        public ObservableCollection<string> Tables
        {
            get
            {
                return tables;
            }
            private set
            {
                tables = value;
                RaisePropertyChanged("Tables");
            }
        }

        private string currentTable;
        private ObservableCollection<string> columns;
        public ObservableCollection<string> Columns
        {
            get
            {
                return columns;
            }
            private set
            {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }

        private DataTable results;
        public DataTable Results
        {
            get
            {
                return results;
            }
            private set
            {
                results = value;
                RaisePropertyChanged("Results");
            }
        }

        public MainViewModel()
        {
            Connections = SettingsReader.readSettings();
        }

        public void ConnectToDb(DbConnectionInfo dbConnectionInfo)
        {
            this.dbAccessor = new MySqlDbAccessor();
            dbAccessor.Connect(dbConnectionInfo.Hostname, dbConnectionInfo.Database,
                dbConnectionInfo.Username, dbConnectionInfo.Password);

            Tables = new ObservableCollection<string>(dbAccessor.GetTables() as List<string>);
        }

        public void SelectTable(string table)
        {
            if (!String.IsNullOrEmpty(table))
            {
                Columns = new ObservableCollection<string>(dbAccessor.GetColumns(table) as List<string>);
                currentTable = table;
            }
        }

        public void SelectRows()
        {
            if (!String.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine(currentTable);
                Results = dbAccessor.FetchAllRowsFromTable(currentTable);
            }
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
