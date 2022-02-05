using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Url
    {
        public string UrlId { get; set; } = null!;
        public string LongUrl { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User? User { get; set; } = null!;
    }
}
