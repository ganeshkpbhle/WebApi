#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    enum Months
    {
        Jan = 1, Feb = 2, Mar = 3, Apr = 4, May = 5, Jun = 6, Jul = 7, Aug = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12
    };
    enum Weekday
    {
        Mon = 1, Tue = 2, Wed = 3, Thu = 4, Fri = 5, Sat = 6, Sun = 7
    };
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class urlController : ControllerBase
    {
        private readonly gisContext _context;

        public urlController(gisContext context)
        {
            _context = context;
        }

        // GET: api/url
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Url>>> GetUrls()
        {
            return await _context.Urls.ToListAsync();
        }

        // GET: api/url/5
        [HttpGet("getId/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Url>> GetUrl(string id)
        {
            var url = await _context.Urls.FindAsync(id);

            if (url == null)
            {
                return NotFound();
            }

            return url;
        }

        [HttpGet("getlist/{userId}")]
        public async Task<ActionResult<IEnumerable<urlist>>> GetUrlList(int userId)
        {
            var url = await _context.Urls.Where(p => p.UserId == userId).OrderByDescending(item => item.CreatedDate).ToListAsync();
            List<urlist> li = new List<urlist>();
            url.ForEach(e =>
            {
                li.Add(new urlist() { UrlId = e.UrlId, LongUrl = e.LongUrl, CreatedDate = e.CreatedDate });
            });
            if (Mthds.IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, userId.ToString()))
            {
                if (url != null)
                {
                    return li;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPost("date")]
        public async Task<ActionResult<IEnumerable<computed>>> GetMonthwise(UrlFormat user)
        {
            var url = await _context.Urls.Where(p => p.UserId == user.Id).OrderBy(item => item.CreatedDate).ToListAsync();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            DateTime tdy = DateTime.Now;
            switch (user.Opt)
            {
                case 1:
                    foreach (var item in url)
                    {
                        if (item.CreatedDate.Year == tdy.Year)
                        {
                            string key = ((Months)item.CreatedDate.Month).ToString();
                            if (dict.ContainsKey(key))
                            {
                                dict[key] += 1;
                            }
                            else
                            {
                                dict.Add(key, 1);
                            }
                        }
                    }
                    break;
                case 2:
                    double days;
                    foreach (var item in url)
                    {
                        days = (tdy - item.CreatedDate).TotalDays;
                        string key = ((Weekday)item.CreatedDate.DayOfWeek).ToString();
                        if (days > 0 && days <= 7)
                        {
                            if (dict.ContainsKey(key))
                            {
                                dict[key] += 1;
                            }
                            else
                            {
                                dict.Add(key, 1);
                            }
                        }
                    }
                    break;
            }
            List<computed> li = dict.Select(pair => new computed { Name = pair.Key, Active_Count = pair.Value }).ToList();
            return li;
        }

        // PUT: api/url/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrl(string id, Url url)
        {
            if (id != url.UrlId)
            {
                return BadRequest();
            }

            _context.Entry(url).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/url
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Url>> PostUrl(Url url)
        {
            _context.Urls.Add(url);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UrlExists(url.UrlId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUrl", new { id = url.UrlId }, url);
        }

        // DELETE: api/url/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(string id)
        {
            var url = await _context.Urls.FindAsync(id);
            var response = Ok(new { del = 1 });
            if (url == null)
            {
                return NotFound();
            }

            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
            return response;
        }

        private bool UrlExists(string id)
        {
            return _context.Urls.Any(e => e.UrlId == id);
        }
    }
}
