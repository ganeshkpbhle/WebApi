using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public partial class loginModel
    {
        public  string Uemail { get; set; }=default!;
        public string Passwd { get; set; }=default!;
    }
    public partial class response
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
    public partial class verify
    {
        public int Id { get; set; }
    }
    public partial class UpdateModel
    {   public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; }=default!;
        public string Mobile { get; set; }=default!;
        public string Email { get; set; } = null!;
    }
}