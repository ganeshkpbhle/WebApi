#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
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
            var url = await _context.Urls.Where(p => p.UserId == userId).ToListAsync();
            List<urlist> li=new List<urlist>();
            url.ForEach(e =>{
                li.Add(new urlist(){UrlId=e.UrlId,LongUrl=e.LongUrl,CreatedDate=e.CreatedDate});
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
            var response= Ok(new { del=1 });
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
