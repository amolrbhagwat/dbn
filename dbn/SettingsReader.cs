﻿using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbn
{
    class SettingsReader
    {
        public static List<DbConnectionInfo> readSettings(String filename = "D:\\data\\dbn.ini")
        {
            List<DbConnectionInfo> dbConnections = new List<DbConnectionInfo>();

            var parser = new FileIniDataParser();
            IniParser.Model.IniData data = parser.ReadFile(filename);

            DbConnectionInfo thisConnection;
            foreach (IniParser.Model.SectionData section in data.Sections)
            {
                thisConnection = new DbConnectionInfo();

                IniParser.Model.KeyDataCollection keyDataCollection = section.Keys;

                string hostname = keyDataCollection["hostname"];
                string database = keyDataCollection["database"];
                string username = keyDataCollection["username"];
                string password = keyDataCollection["password"];

                thisConnection.ConnectionName = section.SectionName;
                thisConnection.Hostname = hostname;
                thisConnection.Database = database;
                thisConnection.Username = username;
                thisConnection.Password = password;

                dbConnections.Add(thisConnection);
            }
            return dbConnections;
        }
    }
}
