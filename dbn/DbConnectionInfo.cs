﻿namespace dbn
{
    public class DbConnectionInfo
    {
        public string ConnectionName { get; set; }

        public string Hostname { get; set; }
        public string Port     { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return ConnectionName;
        }
    }
}
