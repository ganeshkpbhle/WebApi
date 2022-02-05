#nullable disable
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class userController : ControllerBase
    {
        private readonly gisContext _context;
        private readonly IConfiguration _config;
        private readonly SHA256 sHA256 = SHA256.Create();
        private readonly UTF8Encoding objUtf8 = new UTF8Encoding();

        private string CalcHash(string str)
        {
            byte[] hash = sHA256.ComputeHash(objUtf8.GetBytes(str));
            StringBuilder rslt = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                rslt.Append(hash[i].ToString("X2"));
            }
            return rslt.ToString();
        }

        public userController(gisContext context, IConfiguration Config)
        {
            _context = context;
            _config = Config;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpGet("getRes/{id}")]
        public async Task<ActionResult<response>> GetUserSimple(int id)
        {
            var user = await _context.Users.FindAsync(id);
            response res = new response()
            {
                Id = user.Id,
                GId = user.GId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                SnType = user.SnType
            };
            if (user == null)
            {
                return NotFound();
            }
            else if (Mthds.IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, id.ToString()))
            {
                return res;
            }
            else
            {
                return new ForbidResult();
            }
        }
        [HttpGet("getId/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else if (Mthds.IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, id.ToString()))
            {
                return user;
            }
            else
            {
                return new ForbidResult();
            }
        }
        [HttpGet("getEmail/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetUserBymail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Email.Equals(email));
                if (user == null)
                {
                    var result = Ok(new { message = 0 });
                    return result;
                }
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var payload = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sid,user.Id.ToString()),
                    };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                                _config["Jwt:Issuer"],
                                                payload,
                                                expires: DateTime.Now.AddMinutes(45),
                                                signingCredentials: signCredentials
                                                );
                var registeredToken = new JwtSecurityTokenHandler().WriteToken(token);
                var response = Ok(new { token = registeredToken, id = user.Id });
                return response;

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(loginModel model)
        {
            IActionResult response = new ForbidResult();
            User user = await _context.Users.FirstOrDefaultAsync(p => p.Email.Equals(model.Uemail));
            if (user != null)
            {   string Pswd=(user.SnType=="Google")?user.GId:model.Passwd;
                if (CalcHash(Pswd).Equals(user.Passwd))
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var payload = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sid,user.Id.ToString()),
                    };
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                                    _config["Jwt:Issuer"],
                                                    payload,
                                                    expires: DateTime.Now.AddMinutes(45),
                                                    signingCredentials: signCredentials
                                                    );
                    var registeredToken = new JwtSecurityTokenHandler().WriteToken(token);
                    response = Ok(new { token = registeredToken, id = user.Id });
                }
            }
            return response;
        }

        // PUT: api/user/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateModel user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            try
            {
                var usr = await _context.Users.FindAsync(id);
                if (usr != null)
                {
                    usr.FirstName = user.FirstName;
                    usr.LastName = user.LastName;
                    usr.Email = user.Email;
                    usr.Mobile = user.Mobile;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { update = true });
        }

        // POST: api/user
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            User usr;
            if (user.SnType.Equals("Google"))
            {
                usr = new User()
                {
                    GId = user.GId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    EmailVerified = user.EmailVerified,
                    SnType = user.SnType,
                    Passwd = CalcHash(user.GId)
                };
            }
            else
            {
                usr = new User()
                {
                    GId = user.GId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    EmailVerified = user.EmailVerified,
                    SnType = user.SnType,
                    Passwd = CalcHash(user.Passwd)
                };
                var len=CalcHash(user.Passwd).Length;
            }
            try
            {
                _context.Users.Add(usr);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}