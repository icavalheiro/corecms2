using System;
using System.Linq;
using CoreCMS.Tools;
using MongoDB.Driver;

namespace CoreCMS.Systems
{
    /// <summary>
    /// System to be used as a interface to the database when dealing with base contents.
    /// </summary>
    public class ContentSystem : BaseSystem<Content>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="collectionName">The name of the collection to be used in the database.</param>
        public ContentSystem(string collectionName) : base(collectionName) {}


        /// <summary>
        /// Can delete will consider if the content has no chieldren in it.
        /// If id does it cannot be deleted.
        /// </summary>
        /// <typeparam name="Y">Type of the content to check.</typeparam>
        /// <param name="content">The content to be checked.</param>
        /// <returns>True if it can be delete, False otherwise.</returns>
        public override bool CanDelete<Y>(Y content)
        {
            //before anything lets see if the base reproves it
            if (!base.CanDelete(content))
            {
                //if id does just reprove right away
                return false;
            }

            //now lets check if it has any children associated with it
            var chields = Collection.AsQueryable().Where(x => x.ParentId == content.Id).Count();
            if(chields > 0)
            {
                //if it does, reprove it.
                Debug.Log("Unable to delete contents that have children associated to them.");
                return false;
            }

            //no issues found
            return true;
        }
    }
}
