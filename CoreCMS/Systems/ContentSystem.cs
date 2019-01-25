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

        /// <summary>
        /// Get a page of contents from the collection taking into consideration it's parents.
        /// If parent == null than it will return only root contents.
        /// </summary>
        /// <param name="page">Page number, starts from 0.</param>
        /// <param name="pageSize">Size of the pages (number of contents to retrieve), must be greater or equal to 1.</param>
        /// <param name="parent">If different than null will return only chield of the given content.</param>
        /// <returns>An array with the paginated contetns.</returns>
        public Content[] GetPage(Content parent, int page = 0, int pageSize = 25)
        {
            if (page < 0)
                page = 0;

            if (pageSize < 1)
                pageSize = 1;

            IQueryable<Content> query;

            if(parent == null)
            {
                query = Collection.AsQueryable().Where(x => x.ParentId == Guid.Empty);
            }
            else
            {
                query = Collection.AsQueryable().Where(x => x.ParentId == parent.Id);
            }

            return query.Skip(page * pageSize).Take(pageSize).ToArray();
        }
    }
}
