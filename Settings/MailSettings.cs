using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Settings
{
    public partial class MailSettings
    {
        public string Mail { get; set; }=null!;
        public int Port { get; set; }
        public string Password { get; set; }=null!;
        public string Host { get; set; }=null!;
        public string DisplayName { get; set; }=null!;
        
    }
}