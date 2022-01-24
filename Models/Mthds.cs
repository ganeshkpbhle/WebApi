using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Mthds
    {

        public static string? values { get; set; }
        public static bool IsCorrectUser(ClaimsIdentity? identity, string id)
        {
            if (identity != null)
            {
                var claims = identity.FindFirst("sid");
            if (claims != null)
            {
                values = claims.Value;
                return values.Equals(id);
            }
            }
            return false;
        }
    }
}