using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using CoreCMS.Tools;

namespace CoreCMS
{
    /// <summary>
    /// Context to be used to communicate with the MongoDB.
    /// </summary>
    public static class DatabaseContext
    {
        //singleton db connections
        private static IMongoDatabase _database;
        public static IMongoDatabase Database { get { return GetInstanceOfDatabase(); } }

        private static string _connectionString = "";
        private static string _databaseName = "";

        /// <summary>
        /// Set the proper parameters to create a new connection to a MongoDB.
        /// Calling this method will reset the current connection and recreate it.
        /// Use this method if you do not want to load this parameters via ENV VARIABLES.
        /// </summary>
        /// <param name="url">Url of the database (localhost, 127.0.0.1 or the cloud server DNS).</param>
        /// <param name="port">Port that the MongoDB server is using, usually 27017.</param>
        /// <param name="username">Username to access the database.</param>
        /// <param name="password">Password to access the database.</param>
        /// <param name="databaseName">The name of the database to be created/used in the MongoDB server.</param>
        public static void SetConnectionParameters(string url = "localhost", string port = "27017", string username = "", string password = "", string databaseName = "corecms")
        {
            _databaseName = databaseName;
            _connectionString = DatabaseTools.ConnectionStringBuilder(url, port, username, password);
            LoadNewDatabase();
        }

        /// <summary>
        /// Used by the "Database" property.
        /// </summary>
        /// <returns>A valid instance of a IMongoDatabase.</returns>
        private static IMongoDatabase GetInstanceOfDatabase()
        {
            if(_database == null)
            {
                Debug.Log("No database connection found. Trying to create a new one...");
                LoadNewDatabase();
            }

            return _database;
        }

        /// <summary>
        /// Loads a new database connection instance.
        /// </summary>
        private static void LoadNewDatabase()
        {
            Debug.Log("Loading a new database connection.");

            if (string.IsNullOrEmpty(_connectionString))
            {
                Debug.Log("No connection string was set. Creating one using ENV VARIABLES...");
                _connectionString = GetConnectionStringFromEnv();
            }

            if (string.IsNullOrEmpty(_databaseName))
            {
                Debug.Log("No database name was set. Creating one using ENV VARIABLES...");
                _databaseName = GetDatabaseNameFromEnv();
            }

            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_databaseName);
        }

        /// <summary>
        /// Read the database name set as an ENV VARIABLE: MONGODB_DB_NAME
        /// If no value is set, it defaults to "corecms".
        /// </summary>
        /// <returns>Returns a valid database name.</returns>
        private static string GetDatabaseNameFromEnv()
        {
            Debug.Log("Reading database name using ENV VARIABLE: MONGODB_DB_NAME");
            var databaseName = Environment.GetEnvironmentVariable("MONGODB_DB_NAME");

            if (string.IsNullOrEmpty(databaseName))
            {
                Debug.Log("Found no DB name in ENV VARIABLES, using default \"corecms\"");
                databaseName = "corecms";
            }

            return databaseName;
        }

        /// <summary>
        /// Returns a valid MongoDB connection string by reading needed values from ENV variables: 
        /// MONGODB_HOST, MONGODB_PORT, MONGODB_USER, MONGODB_PASS
        /// </summary>
        /// <returns>A valid MongoDB connection string if all the ENV variables were set.</returns>
        private static string GetConnectionStringFromEnv()
        {
            Debug.Log("Creating connection string using ENV VARIABLES: MONGODB_HOST, MONGODB_PORT, MONGODB_USER, MONGODB_PASS");

            var url = Environment.GetEnvironmentVariable("MONGODB_HOST");
            var port = Environment.GetEnvironmentVariable("MONGODB_PORT");
            var username = Environment.GetEnvironmentVariable("MONGODB_USER");
            var password = Environment.GetEnvironmentVariable("MONGODB_PASS");

            return DatabaseTools.ConnectionStringBuilder(url, port, username, password);
        }
    }
}
