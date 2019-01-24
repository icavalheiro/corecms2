using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoreCMS
{
    /// <summary>
    /// Represents a CMS user.
    /// </summary>
    public class User : BaseContent
    {
        public string HashedPassword { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public int AccessLevel { get; set; }

        /// <summary>
        /// Set a new password (it will hash it and store it hashed).
        /// </summary>
        /// <param name="newPassword">Password to hash and store.</param>
        public void SetPassword(string newPassword)
        {
            HashedPassword = GetHashedPassword(newPassword);
        }

        /// <summary>
        /// Converts plain text password into hashed passwords.
        /// Utilizes SHA256, but you can overwirte it if you want a custom method of hashing.
        /// </summary>
        /// <param name="passwordToHash">Password to hash.</param>
        /// <returns>A hash representing the password.</returns>
        protected virtual string GetHashedPassword(string passwordToHash)
        {
            byte[] data = Encoding.UTF8.GetBytes(passwordToHash + Salt);
            data = new SHA256Managed().ComputeHash(data);
            var hashedPassword = Encoding.UTF8.GetString(data);
            return hashedPassword;
        }

        /// <summary>
        /// Check if the given password is the same as the stored password.
        /// </summary>
        /// <param name="passwordToTest">Plain text password to hash and compare.</param>
        /// <returns>Whether the given password is equal to the stored password.</returns>
        public bool TestPassword(string passwordToTest)
        {
            return GetHashedPassword(passwordToTest) == HashedPassword;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public User()
        {
            Salt = Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
