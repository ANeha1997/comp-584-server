using comp_584_server.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using WorldModel;


namespace comp_584_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController (UserManager<WorldModelUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginrequest)
        {
            WorldModelUser? worldUser = await userManager.FindByNameAsync(loginrequest.Username);
            //if (worldUser is null || !await userManager.CheckPasswordAsync(worldUser, loginrequest.Password))
            //{
            //    Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await Response.WriteAsync("Invalid username or password");
            //    return;
            //}
            if (worldUser == null)
            {
                return Unauthorized("Invalid username");
            }
            bool loginstatus = await userManager.CheckPasswordAsync(worldUser, loginrequest.Password);
            if (!loginstatus)
            {
                return Unauthorized("Invalid password");
            }
            JwtSecurityToken jwtToken = await jwtHandler.GenerateTokenAsync(worldUser);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Mom Loves me!",
                Token = stringToken
            });
        }
    }
}
