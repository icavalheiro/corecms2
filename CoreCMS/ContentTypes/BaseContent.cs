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

        [BsonElement("fully_assembly_qualified_type_name")]
        private string _typeName { get; set; }

        [BsonIgnore]
        public string TypeName { get => GetTypeName(); }

        /// <summary>
        /// Gets the type name of the instance of this object.
        /// </summary>
        /// <returns>The type name.</returns>
        private string GetTypeName()
        {
            if (string.IsNullOrEmpty(_typeName))
            {
                var myType = this.GetType();
                _typeName = myType.AssemblyQualifiedName;
            }

            return _typeName;
        }
    }
}
