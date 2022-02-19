using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class User
    {
        public User()
        {
            Urls = new HashSet<Url>();
        }

        public int Id { get; set; }
        public string GId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Mobile { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int EmailVerified { get; set; }
        public string SnType { get; set; } = null!;
        public string Passwd { get; set; } = null!;

        public virtual UserSession? UserSession { get; set; }
        public virtual ICollection<Url>? Urls { get; set; }
    }
}
