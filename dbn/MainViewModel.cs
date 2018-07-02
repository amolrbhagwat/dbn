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

        private string currentTable;
        public string CurrentTable {
            get
            {
                return currentTable;
            }
            set
            {
                currentTable = value;
                RaisePropertyChanged("CurrentTable");
                UpdateColumnNames();
            }
        }

        private Dictionary<string, List<string>> columnTableMapping;

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

            Columns?.Clear();
            Results = null;
        }

        private void GenerateColumnTableMapping()
        {
            columnTableMapping = new Dictionary<string, List<string>>();

            foreach(string table in Tables)
            {
                foreach(string column in dbAccessor.GetColumns(table))
                {
                    if (!columnTableMapping.ContainsKey(column))
                    {
                        columnTableMapping.Add(column, new List<string>());
                    }
                    columnTableMapping[column].Add(table);
                }
            }
        }

        public void UpdateColumnNames()
        {
            if (!String.IsNullOrEmpty(CurrentTable))
            {
                Columns = new ObservableCollection<string>(dbAccessor.GetColumns(CurrentTable) as List<string>);
            }
        }

        public void SelectRows()
        {
            if (!String.IsNullOrEmpty(CurrentTable))
            {
                Results = dbAccessor.FetchAllRowsFromTable(CurrentTable);
            }
        }

        public void SelectRowsMatching(Dictionary<string, string> criteria)
        {
            if (!String.IsNullOrEmpty(CurrentTable))
            {
                Results = dbAccessor.FetchMatchingRowsFromTable(CurrentTable, criteria);
            }
        }

        public List<string> getTableNamesWhereColumnPresent(string columnName)
        {
            if (!String.IsNullOrEmpty(columnName))
            {
                return columnTableMapping[columnName];
            }
            else
            {
                return new List<string>();
            }
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
