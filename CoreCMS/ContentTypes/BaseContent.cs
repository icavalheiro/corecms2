using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCMS
{
    /// <summary>
    /// Base content class, should be inherited by all classes that want to be stored
    /// in the database.
    /// </summary>
    public class BaseContent
    {
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// Really important property that will allow us to serialize and deserialize
        /// this objects without losing any data.
        /// </summary>
        [BsonElement("full_type_name")]
        private string _typeName { get; set; }

        [BsonIgnore]
        public string TypeName { get => _typeName; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BaseContent()
        {
            var myType = this.GetType();
            _typeName = myType.FullName;
        }
    }
}
