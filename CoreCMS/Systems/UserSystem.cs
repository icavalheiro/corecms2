using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CoreCMS.Systems
{
    /// <summary>
    /// System to be used as a interface to the database when dealing with CMS users.
    /// </summary>
    public class UserSystem : BaseSystem<User>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="collectionName">Name of the collection in the database.</param>
        public UserSystem(string collectionName) : base(collectionName)
        {
            EnsureRootUserExists();
        }

        /// <summary>
        /// Ensures that there is at leas a root user in the database.
        /// Root user would have the following user and pass: root root
        /// </summary>
        private void EnsureRootUserExists()
        {
            var firstUser = GetPage(0, 1).FirstOrDefault();
            if(firstUser == null)
            {
                //there is no user in the database
                //lets create the root
                var rootUser = new User();
                rootUser.Username = "root";
                rootUser.SetPassword("root");
                
                //save it async
                Task.Run(async () => {
                    await TrySaveAsync(rootUser);
                });
            }
        }

        /// <summary>
        /// Get a given user by its username.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <returns>A user if found, otherwise null.</returns>
        public User GetByUsername(string username)
        {
            var foundList = GetByUsername(new string[] { username });
            if (foundList.Length == 0)
                return null;

            return foundList[0];
        }

        /// <summary>
        /// Get many users by their usernames.
        /// May return different length from the input usernames based on what is in the database.
        /// </summary>
        /// <param name="usernames">Usernames to search for.</param>
        /// <returns>The list containing the found Users.</returns>
        public User[] GetByUsername(params string[] usernames)
        {
            if(usernames == null || usernames.Length == 0)
            {
                return null;
            }

            var list = new List<string>(usernames);
            return Collection.AsQueryable().Where(x => list.Contains(x.Username)).ToArray();
        }

        /// <summary>
        /// Tryes to save the current content into the database.
        /// May fail if we find any given content with a new username that is already taken.
        /// Usernames should be unique.
        /// </summary>
        /// <typeparam name="Y">Type of the content to be saved.</typeparam>
        /// <param name="content">The content to be saved.</param>
        /// <returns>If it was able to successfully save the content into the database.</returns>
        public override async Task<bool> TrySaveAsync<Y>(params Y[] users)
        {
            //TODO: remove this gigantic dependency on LINQ

            //lets check if any of the usernames we have in the contents
            //either are unique or already belong to them
            var usernames = Collection.AsQueryable().Select(x => x.Username).ToArray();
            var foundUsersByUsername = GetByUsername(usernames);
            var mismatch = foundUsersByUsername.Where(x =>
                (users.Where(y => y.Username == x.Username).First().Id != x.Id))
                .Count();
            
            if(mismatch > 0)
            {
                //some usernames are already taken
                return false;
            }

            //nothing wrong lets continue to base
            return await base.TrySaveAsync(users);
        }
    }
}
