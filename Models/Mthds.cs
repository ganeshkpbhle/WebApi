using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public enum Months
    {
        Jan = 1, Feb = 2, Mar = 3, Apr = 4, May = 5, Jun = 6, Jul = 7, Aug = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12
    };
    public enum Weekday
    {
        Mon = 1, Tue = 2, Wed = 3, Thu = 4, Fri = 5, Sat = 6, Sun = 0
    };
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