using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public partial class urlist
    {
        [Key]
        public string UrlId { get; set; } = null!;
        public string LongUrl { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
    public partial class computed
    {
        public string Name { get; set; } = default!;
        public int Active_Count { get; set; }
    }
    public partial class UrlFormat{
        public int Id { get; set; }=default!;
        public int Opt { get; set; }=default!;
    }
}