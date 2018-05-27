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

        private Dictionary<string, List<string>> columnTableMapping;
        public Dictionary<string, List<string>> ColumnTableMapping
        {
            get
            {
                return columnTableMapping;
            }
            private set
            {
                columnTableMapping = value;
                RaisePropertyChanged("ColumnTableMapping");
            }
        }

        public MainViewModel()
        {
            Connections = SettingsReader.readSettings();
        }

        public void ConnectToDb(DbConnectionInfo dbConnectionInfo)
        {
            this.dbAccessor = new MySqlDbAccessor();
            dbAccessor.Connect(dbConnectionInfo.Hostname, dbConnectionInfo.Port, dbConnectionInfo.Database,
                dbConnectionInfo.Username, dbConnectionInfo.Password);

            Tables = new ObservableCollection<string>(dbAccessor.GetTables() as List<string>);
            GenerateColumnTableMapping();
        }

        private void GenerateColumnTableMapping()
        {
            ColumnTableMapping = new Dictionary<string, List<string>>();

            foreach(string table in Tables)
            {
                foreach(string column in dbAccessor.GetColumns(table))
                {
                    if (!ColumnTableMapping.ContainsKey(column))
                    {
                        ColumnTableMapping.Add(column, new List<string>());
                    }
                    ColumnTableMapping[column].Add(table);
                }
            }
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
                Results = dbAccessor.FetchAllRowsFromTable(currentTable);
            }
        }

        public void SelectRowsMatching(Dictionary<string, string> criteria)
        {
            if (!String.IsNullOrEmpty(currentTable))
            {
                Results = dbAccessor.FetchMatchingRowsFromTable(currentTable, criteria);
            }
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
