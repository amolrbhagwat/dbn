﻿using System;
using System.Collections.Generic;
using System.Data;

namespace dbn
{
    abstract class DbAccessor
    {
        Boolean Connected { get; }

        public abstract Boolean Connect(string server, string port, string database, string user, string password);

        public abstract void Disconnect();

        public abstract List<string> GetColumns(string table);

        public abstract List<string> GetTables();

        public abstract DataTable FetchAllRowsFromTable(string table);

        public abstract DataTable FetchMatchingRowsFromTable(string table, Dictionary<string, string> criteria);
    }
}
