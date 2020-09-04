using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Token
    {
        public int IdToken { get; set; }
        public int FkUser { get; set; }
        public DateTime Expiration { get; set; }
        public bool Active { get; set; }

        public virtual User FkUserNavigation { get; set; }
    }
}
