using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class UserSession
    {
        public int Id { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public int TokenValid { get; set; }
        public string Token { get; set; } = null!;

        public virtual User IdNavigation { get; set; } = null!;
    }
}
