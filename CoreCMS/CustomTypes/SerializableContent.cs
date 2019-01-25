using System;
using Newtonsoft.Json;

namespace CoreCMS
{
    public sealed class SerializableContent
    {
        public string ContentTypeFullName;
        public string JsonString;

        /// <summary>
        /// Constructor.
        /// Avoid using the empty constructor, use this only for deserialization.
        /// </summary>
        public SerializableContent() { }

        /// <summary>
        /// Constructor.
        /// Try always to use this constructor when building new objects.
        /// </summary>
        /// <param name="content">Content to be serialized.</param>
        public SerializableContent(BaseContent content)
        {
            ContentTypeFullName = content.TypeName;
            JsonString = JsonConvert.SerializeObject(content);
        }

        /// <summary>
        /// Constructor.
        /// Build this object using a serialized JSON string that contains a instance of this class.
        /// </summary>
        /// <param name="json">JSON serialized string.</param>
        public SerializableContent(string json)
        {
            var deserializedContent = JsonConvert.DeserializeObject<SerializableContent>(json);
            ContentTypeFullName = deserializedContent.ContentTypeFullName;
            JsonString = deserializedContent.JsonString;
        }

        /// <summary>
        /// Deserialize this instance into the original object.
        /// </summary>
        /// <returns>The original object (you can cast it to the proper type)</returns>
        public BaseContent Deserialize()
        {
            if(Cms.AvailableSubTypes.ContainsKey(ContentTypeFullName) == false)
            {
                throw new Exception("Cannot deserialize a content that is unknown to the CMS.");
            }

            //retrieve the type from the known list of types
            var contentType = Cms.AvailableSubTypes[ContentTypeFullName];

            //asks json.net to deserialize it into the type we known
            var deserializedObject = (BaseContent)JsonConvert.DeserializeObject(JsonString, contentType);

            return deserializedObject;
        }

        /// <summary>
        /// Converts this instance to a JSON string.
        /// </summary>
        /// <returns>A JSON string containing this object.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
