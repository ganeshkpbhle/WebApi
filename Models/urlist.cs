using System.ComponentModel.DataAnnotations;
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
    public partial class UrlFormat
    {
        public int Id { get; set; } = default!;
        public int Opt { get; set; } = default!;
    }
    public partial class yearGroup
    {
        public int Yr { get; set; }
        public List<urlist>? YrData { get; set; }
    }
    public partial class MonthGroup
    {
        public string? month { get; set; }
        public List<urlist>? urls { get; set; }
    }
}