using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCMS
{
    /// <summary>
    /// Content class, used to be stored as a document in the Database and 
    /// also to be used as a basic model for views and etc...
    /// The CoreCMS bases itself in the handeling of Contents though Systems.
    /// </summary>
    public class Content : BaseContent
    {
        public TranslatableString Name { get; set; }
        public Guid ParentId { get; set; }
    }
}
