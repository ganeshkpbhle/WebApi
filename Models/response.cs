using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class response
    {
        public int Id { get; set; }
        public string? GId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string Email { get; set; }=null!;
        public int EmailVerified { get; set; }
        public string? SnType { get; set; }
    }
}