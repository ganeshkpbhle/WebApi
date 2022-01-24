using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public partial class MailRequest
    {
        public string ToMail { get; set; }=null!;
        public string? Subject { get; set; }
        public string Body { get; set; }=default!;
        public List<IFormFile>? Attachments {get ;set;}
    }
}