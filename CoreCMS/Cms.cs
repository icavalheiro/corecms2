using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreCMS.Systems;

namespace CoreCMS
{
    /// <summary>
    /// Base CMS class, responsible for initializing the CMS into memory.
    /// </summary>
    public static class Cms
    {
        public static Dictionary<string, Type> AvailableSubTypes = new Dictionary<string, Type>();
        public static ContentSystem ContentSystem = new ContentSystem("cms_contents");
        public static UserSystem UserSystem = new UserSystem("cms_users");
        public static LoginTokenSystem LoginTokenSystem = new LoginTokenSystem("cms_login_tokens");
        
        /// <summary>
        /// Constructor.
        /// </summary>
        static Cms()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the type dictionary that is the base of the CMS.
        /// </summary>
        private static void Initialize()
        {
            RegisterAllClassesFromType(typeof(BaseContent));
        }

        /// <summary>
        /// Register into the MongoDB Driver all the types that inherit the "type" so that the driver
        /// knows how to load them properly from the Database.
        /// </summary>
        /// <param name="type">The type to load into the MongoDB Driver.</param>
        private static void RegisterAllClassesFromType(Type type)
        {
            var availableTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.IsSubclassOf(type));

            //Register the base class
            BsonClassMap.LookupClassMap(type);

            //Lets register on the MongoDB  Driver all this types
            //so it knows how to handle them properly
            foreach (var t in availableTypes)
            {
                BsonClassMap.LookupClassMap(t);
                
                //cache it for later use
                AvailableSubTypes.Add(t.FullName, t);
            }
        }
    }
}
