using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public partial class Url
    {
        [Key]
        public string UrlId { get; set; } = null!;
        public string LongUrl { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
