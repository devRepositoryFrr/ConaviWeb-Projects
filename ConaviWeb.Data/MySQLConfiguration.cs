using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data
{
    public class MySQLConfiguration
    {
        public MySQLConfiguration(string connectionString, string userconnectionString, string edconnectionString, string expconnectionString)
        {
            ConnectionString = connectionString;
            UserConnectionString = userconnectionString;
            EDConnectionString = edconnectionString;
            ExpConnectionString = expconnectionString;
        }
        //public MySQLConfiguration(string connectionString) => ConnectionString = connectionString;
        public string ConnectionString { get; set; }
        public string UserConnectionString { get; set; }
        public string EDConnectionString { get; set; }
        public string ExpConnectionString { get; set; }
    }
}
