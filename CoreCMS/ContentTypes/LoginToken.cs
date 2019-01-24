using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreCMS
{
    /// <summary>
    /// Holds some data related to a user login session.
    /// </summary>
    public class LoginToken : BaseContent
    {
        public DateTime ExpireAt { get; set; }
        public Guid UserId { get; set; }
        public IPAddress AccessIp { get; set; }
    }
}
