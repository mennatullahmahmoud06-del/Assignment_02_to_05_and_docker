using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Assignment.Constants;
using Assignment.Dtos.AccountDtos;
using Assignment.Entities;
using Assignment.Results;
using Assignment.Services;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountController(UserManager<ApplicationUser> userManager, IJwtService jwtService) : ApiControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, Roles.Player);

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized(Result.Failure("Invalid email or password."));

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtService.GenerateToken(user, roles);

            return Ok(GenericResult<string>.Success(token));
        }
    }
}
