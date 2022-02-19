using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Services;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class verifyController : Controller
    {
        private readonly IMail _mailService;
        private readonly gisContext _context;
        public verifyController(IMail mailService, gisContext Context)
        {
            _mailService = mailService;
            _context = Context;
        }
        [HttpPost("Email")]
        public async Task<IActionResult> Send(MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok("sent");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("post")]
        public async Task<ActionResult<response>> postEmailVerification(verify vf)
        {
            try
            {
                var user = await _context.Users.FindAsync(vf.Id);
                if(user!=null && Mthds.IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, vf.Id.ToString()))
                {
                    user.EmailVerified=1;
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }
                return Unauthorized();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(vf.Id))
                {
                    return Unauthorized();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}