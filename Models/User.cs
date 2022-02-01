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
        public string? GId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string Email { get; set; } = null!;
        public int EmailVerified { get; set; }
        public string? SnType { get; set; }
        public string Passwd { get; set; } = null!;

        public virtual ICollection<Url> Urls { get; set; }
    }
}
