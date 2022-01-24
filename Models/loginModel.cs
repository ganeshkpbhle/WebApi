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
}