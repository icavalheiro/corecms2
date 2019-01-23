using System;
using Newtonsoft.Json;

namespace CoreCMS
{
    public sealed class SerializableContent
    {
        public string ContentTypeFullName;
        public string Json;

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
            Json = JsonConvert.SerializeObject(content);
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
            var deserializedObject = (BaseContent)JsonConvert.DeserializeObject(Json, contentType);

            return deserializedObject;
        }
    }
}
