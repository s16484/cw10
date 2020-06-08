using System;
using System.Collections.Generic;

namespace APIDatabaseFirst.Models
{
    public partial class RefreshToken
    {
        public string Token { get; set; }
        public string IndexNumber { get; set; }
        public DateTime ValidDate { get; set; }

        public virtual Student IndexNumberNavigation { get; set; }
    }
}
