using IniParser;
using System;
using System.Collections.Generic;

namespace dbn
{
    class SettingsReader
    {
        private static string defaultConnectionsFile =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "\\dbn\\dbn.ini";

        public static List<DbConnectionInfo> readSettings(String filename = "")
        {
            if (String.IsNullOrEmpty(filename))
            {
                filename = defaultConnectionsFile;
            }

            List<DbConnectionInfo> dbConnections = new List<DbConnectionInfo>();

            var parser = new FileIniDataParser();
            IniParser.Model.IniData data = parser.ReadFile(filename);

            DbConnectionInfo thisConnection;
            foreach (IniParser.Model.SectionData section in data.Sections)
            {
                thisConnection = new DbConnectionInfo();

                IniParser.Model.KeyDataCollection keyDataCollection = section.Keys;

                string hostname = keyDataCollection["hostname"];
                string port     = keyDataCollection["port"];
                string database = keyDataCollection["database"];
                string username = keyDataCollection["username"];
                string password = keyDataCollection["password"];

                thisConnection.ConnectionName = section.SectionName;
                thisConnection.Hostname = hostname;
                thisConnection.Port     = port;
                thisConnection.Database = database;
                thisConnection.Username = username;
                thisConnection.Password = password;

                dbConnections.Add(thisConnection);
            }
            return dbConnections;
        }
    }
}
