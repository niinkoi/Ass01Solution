using DataAccess.providers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.utils
{
    public class BaseDAL
    {
        private static readonly string AppSettingsJson = "appsettings.json";
        public DataProvider? provider { get; set; } = null;
        public SqlConnection? connection = null;

        public BaseDAL()
        {
            var connectionString = GetConnectionString();
            provider = new DataProvider(connectionString);
        }

        private string? GetConnectionString()
        {
            string? connectionString = null;
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsJson, true, true).Build();
            connectionString = config["ConnectionStrings:AppConnection"];
            return connectionString;
        }
    }
}
