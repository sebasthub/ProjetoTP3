using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stock2u.Auth;
using Stock2u.Models;

namespace Stock2u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
         private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthenticateController(
        IConfiguration configuration,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;

    }

        [HttpGet]
        [Route("workers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetWorkers()
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users.ToListAsync();
            var workers = new List<IdentityUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "worker"))
                {
                    workers.Add(user);
                }
            }

            return workers;
        }
        
        [HttpGet]
        [Route("clients")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetClientes()
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users.ToListAsync();
            var clients = new List<IdentityUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "client"))
                {
                    clients.Add(user);
                }
            }

            return clients;
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,worker")]
        public async Task<ActionResult<IdentityUser>> GetUser(string id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.UserName);

            if (userExists is not null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel { Success = false, Message = "Usuário já existe!" }
                );

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel { Success = false, Message = "Erro ao criar usuário" }
                );

            var role = model.IsAdmin ? UserRoles.Admin : UserRoles.worker;

            await AddToRoleAsync(user, role);

            return Ok(new ResponseModel { Message = "Usuário criado com sucesso!" });
        }

        [HttpPost]
        [Route("register-client")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateClientAsync([FromBody] CreateUserModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.UserName);

            if (userExists is not null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel { Success = false, Message = "Usuário já existe!" }
                );

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel { Success = false, Message = "Erro ao criar usuário" }
                );

            var role = UserRoles.client;

            await AddToRoleAsync(user, role);

            return Ok(new ResponseModel { Message = "Usuário criado com sucesso!" });
        }

        [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
        {

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
                authClaims.Add(new(ClaimTypes.Role, userRole));

            return Ok(new ResponseModel { Data = GetToken(authClaims), Message = userRoles[0]});
        }

        return Unauthorized();
    }

    private TokenModel GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ValidTo = token.ValidTo
        };

    }

    private async Task AddToRoleAsync(IdentityUser user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new(role));

        await _userManager.AddToRoleAsync(user, role);
    }
    }
}
