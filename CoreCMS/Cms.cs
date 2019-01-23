using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreCMS
{
    public static class Cms
    {
        public static Dictionary<string, Type> AvailableSubTypes = new Dictionary<string, Type>();


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
                AvailableSubTypes.Add(t.AssemblyQualifiedName, t);
            }
        }
    }
}
