using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Serialize this content to JSON format that is easy to use in generic views.
        /// </summary>
        /// <returns>A Class containing the serialized JSON.</returns>
        public SerializableContent GetSerializable()
        {
            return new SerializableContent(this);
        }

        /// <summary>
        /// Get an array of the properties of this object serialized into JSON.
        /// </summary>
        /// <returns>The JSON serialized properties.</returns>
        public (string name, string value)[] GetSerializedProperties()
        {
            var props = this.GetType().GetProperties();
            var array = new (string name, string value)[props.Length];
            for(int i =0; i < array.Length; i++)
            {
                var prop = props[i];
                var propName = prop.Name;
                var propValue = JsonConvert.SerializeObject(prop.GetValue(this));
                array[i] = (propName, propValue);
            }

            return array;
        }
    }
}
