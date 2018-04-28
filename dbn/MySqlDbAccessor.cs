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

        public override bool Connect(string server, string database, string user, string password)
        {
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                server, database, user, password);

            connection = new MySqlConnection(connectionString);
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
      
        public override List<string> GetTables()
        {
            List<string> tables = new List<string>();

            string fetchTablesQuery = "show tables;";

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(fetchTablesQuery, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tables.Add(reader[0].ToString());
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return tables;
        }

        public override List<string> GetColumns(string table = "productlines")
        {
            List<string> columns = new List<string>();

            string fetchColumnsQuery = String.Format("select column_name from information_schema.columns where table_name = '{0}';",
                table);

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(fetchColumnsQuery, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    columns.Add(reader[0].ToString());
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

            return columns;
        }

        public override DataTable FetchAllRowsFromTable(string table)
        {
            DataTable results = new DataTable();

            string fetchResultsQuery = String.Format("select * from {0};", table);

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(fetchResultsQuery, connection);
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

        public override DataTable FetchMatchingRowsFromTable(string table, Dictionary<string, string> criteria)
        {
            DataTable results = new DataTable();

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

            MySqlCommand command;
            MySqlDataReader reader = null;
            try
            {
                command = new MySqlCommand(fetchResultsQuery, connection);
                reader = command.ExecuteReader();

                results.Load(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
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
    }
}
