namespace CoreCMS.Systems
{
    /// <summary>
    /// System to be used as a interface to the database when dealing with base contents.
    /// </summary>
    public class ContentSystem : BaseSystem<BaseContent>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="collectionName">The name of the collection to be used in the database.</param>
        public ContentSystem(string collectionName) : base(collectionName) {}
    }
}
