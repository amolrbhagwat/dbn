using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbn
{
    class MySqlDbAccessor : DbAccessor
    {
        private MySqlConnection connection;

        public Boolean Connected
        {
            get
            {
                return connection.State == ConnectionState.Open;
            }
        }

        public override bool Connect(string server, string port, string database, string user, string password)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
            connectionStringBuilder.Server = server;
            connectionStringBuilder.Port = UInt16.Parse(port);
            connectionStringBuilder.Database = database;
            connectionStringBuilder.UserID = user;
            connectionStringBuilder.Password = password;
            connectionStringBuilder.SslMode = MySqlSslMode.None;

            connection = new MySqlConnection(connectionStringBuilder.ToString());
            connection.Open();

            return Connected;
        }

        public override void Disconnect()
        {
            if(connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private List<string> GetList(string query)
        {
            List<string> resultsList = new List<string>();

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(query, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    resultsList.Add(reader[0].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return resultsList;
        }

        public override List<string> GetTables()
        {
            string fetchTablesQuery = "show tables;";

            return GetList(fetchTablesQuery);
        }

        public override List<string> GetColumns(string table = "productlines")
        {
            string fetchColumnsQuery = String.Format("select column_name from information_schema.columns where table_name = '{0}';",
                table);

            return GetList(fetchColumnsQuery);
        }

        private DataTable FetchRows(string query)
        {
            DataTable results = new DataTable();

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(query, connection);
                reader = command.ExecuteReader();

                results.Load(reader);
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return results;
        }

        public override DataTable FetchAllRowsFromTable(string table)
        {
            string fetchResultsQuery = String.Format("select * from {0};", table);

            return FetchRows(fetchResultsQuery);
        }

        public override DataTable FetchMatchingRowsFromTable(string table, Dictionary<string, string> criteria)
        {
            StringBuilder whereCriteria = new StringBuilder();
            foreach (var fieldValuePair in criteria)
            {
                if (whereCriteria.Length > 0) { whereCriteria.Append(" and "); }

                whereCriteria.Append(fieldValuePair.Key);
                whereCriteria.Append(" = ");
                whereCriteria.Append("'" + fieldValuePair.Value + "'");
            }

            string fetchResultsQuery = String.Format("select * from {0} where {1};", table, whereCriteria);

            Console.WriteLine("The criteria is: {0}", fetchResultsQuery);

            return FetchRows(fetchResultsQuery);
        }
    }
}
