using System;

namespace CoreCMS.Tools
{
    public static class DatabaseTools
    {
        /// <summary>
        /// Builds a fully functional connection string to be used in a MongoDB Driver.
        /// </summary>
        /// <param name="url">The URL to the Mongo server (localhost or 127.0.0.1 or the DNS of the cloud server...).</param>
        /// <param name="port">The port the server is running on.</param>
        /// <param name="username">Username to access the database.</param>
        /// <param name="password">Password to access the database.</param>
        /// <returns>A valid MongoDB connection string in the following patter: "mongodb://USER:PASS@URL:PORT"</returns>
        public static string ConnectionStringBuilder(string url = "localhost", string port = "27017", string username = "", string password = "")
        {
            //the minimun we need is the url and port to the database
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(port))
            {
                throw new Exception("The URL or PORT for the mongoDB connection string cannot be null. Make sure you have filled both properly");
            }

            //check for possible unknown usage of this methoid
            if (url.StartsWith("mongodb://"))
            {
                throw new Exception("Please do not use the mongo DB connection string as a paremeter. We expect only the DNS address to the database");
            }

            //if no user was defined than we ignore user and password
            if (string.IsNullOrEmpty(username))
                return $"mongodb://{url}:{port}";

            //no passwords? returns it with username only (is it allowed?)
            if (string.IsNullOrEmpty(password))
                return $"mongodb://{username}@{url}:{port}";


            //return a complete connection string
            return $"mongodb://{username}:{password}@{url}:{port}";
        }
    }
}
