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
    public abstract class BaseContent
    {
        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        /// Really important property that will allow us to serialize and deserialize
        /// this objects without losing any data.
        /// </summary>
        [BsonElement("_full_t")]
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
