using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreCMS
{
    /// <summary>
    /// This class is to be inherited by the systems that will handle custom contents on the CMS.
    /// </summary>
    /// <typeparam name="T">The type of the contents this system will handle.</typeparam>
    public class BaseSystem<T> where T : BaseContent
    {
        private IMongoCollection<T> _colection;
        public IMongoCollection<T> Collection { get { return GetCollection(); } }

        /// <summary>
        /// Get the collection that stores the contents of this type.
        /// Please note that the name of the collection in the MongoDB instance will be
        /// the fully assembly qualified name of the type.
        /// </summary>
        /// <returns>The collection.</returns>
        protected virtual IMongoCollection<T> GetCollection()
        {
            //set up collection if it was not setup yet
            if(_colection == null)
            {
                var tType = typeof(T);
                _colection = DatabaseContext.Database.GetCollection<T>(tType.AssemblyQualifiedName);
            }

            //return collection
            return _colection;
        }

        /// <summary>
        /// Retrieves a type of content using its ID.
        /// </summary>
        /// <typeparam name="Y">The type of the content to be retrieved.</typeparam>
        /// <param name="id">The ID of the content to be searched for.</param>
        /// <returns>The content.</returns>
        public virtual Y GetById<Y>(ObjectId id) where Y : T
        {
            return (Y)GetById(id);
        }

        /// <summary>
        /// Retrieves a type of content using its ID.
        /// </summary>
        /// <param name="id">The ID of the content to be searched for.</param>
        /// <returns>The content.</returns>
        public virtual T GetById(ObjectId id)
        {
            if (id == ObjectId.Empty)
                return null;

            return Collection.AsQueryable().Where(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Checks if a content existis in the collection to know if it can be deleted or not.
        /// It does not assure if the process can be successfully done or not, since the deletion
        /// process may fail by network reasons.
        /// </summary>
        /// <typeparam name="Y">The type of the content to be tested.</typeparam>
        /// <param name="content">The content to be tested.</param>
        /// <returns>If it can be deleted.</returns>
        public virtual bool CanDelete<Y>(Y content) where Y : T
        {
            if(content == null || GetById(content.Id) == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tryes to delete a given content asyncronously.
        /// </summary>
        /// <typeparam name="Y">Type of the content to be deleted.</typeparam>
        /// <param name="content">Content to be deleted.</param>
        /// <returns>If the content was successfully deleted.</returns>
        public virtual async Task<bool> TryDeleteAsync<Y>(Y content) where Y : T
        {
            //make the first basic check of this content can be deleted or not
            if (!CanDelete(content))
            {
                return false;
            }

            //ask the mongodb server to delete it
            var result = await Collection.DeleteOneAsync((x) => x.Id == content.Id);

            //check if the deletion command was acknowledge
            if (result.IsAcknowledged)
            {
                //if it was, return if it deleted something
                return result.DeletedCount > 0;
            }

            //if the command was not acknowledged
            return false;
        }

        /// <summary>
        /// Tryes to save the current content into the database.
        /// </summary>
        /// <typeparam name="Y">Type of the content to be saved.</typeparam>
        /// <param name="content">The content to be saved.</param>
        /// <returns>If it was able to successfully save the content into the database.</returns>
        public virtual async Task<bool> TrySaveAsync<Y>(Y content) where Y : T
        {
            if (content == null)
            {
                return false;
            }

            //we will verify if the username is unique before saving it
            var id = content.Id;
            if (id == ObjectId.Empty)
            {
               await Collection.InsertOneAsync(content);
            }
            else
            {
                //lets check if the username already existis, and if it does, lets comapre if the IDs are the same
                //just to make sure it is editing the same username that is already in the database
                await Collection.ReplaceOneAsync((x) => x.Id == id, content);
            }

            return true;
        }
    }
}
