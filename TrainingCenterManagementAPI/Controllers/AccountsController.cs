using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
/*        private readonly IConfiguration configuration;

        //Post api/accounts/login

        public AccountsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate (AuthRequest request)
        {
            // token
            *//* var user = ValidateUserInformation(request.Username, request.Password);
             if(user == null)
                 return Unauthorized();


             var secretKey = new SymmetricSecurityKey(Encoding.ASCII
                                                         .GetBytes(configuration["Authentication:SecretKey"]));

             var signingCred= new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);

             var calims = new List<Claim>();

             //calims.Add(new Claim("giver_name","ahmad"));
             calims.Add(new Claim("giver_familyName", "shoriqee"));
             calims.Add(new Claim("course", "midad_11"));
             calims.Add(new Claim("sub", "125"));
             calims.Add(new Claim(ClaimTypes.GivenName,"ahmad"));
             calims.Add(new Claim(ClaimTypes.Role,"Admin")) ;




             var securityToken = new JwtSecurityToken(
                 configuration["Authentication:Issuer"],
                 configuration["Authentication:Audience"],
                 calims,
                 DateTime.UtcNow,
                 DateTime.UtcNow.AddHours(10),
                 signingCred
                 );

             var token= new JwtSecurityTokenHandler().WriteToken(securityToken);
             return Ok(token);*//*






            //cookies
            var user = ValidateUserInformation(request.Username, request.Password);
            if (user == null)
                return Unauthorized();


            var calims = new List<Claim>();

            //calims.Add(new Claim("giver_name","ahmad"));
            calims.Add(new Claim("giver_familyName", "shoriqee"));
            calims.Add(new Claim("course", "midad_11"));
            calims.Add(new Claim("sub", "125"));
            calims.Add(new Claim(ClaimTypes.GivenName, "ahmad"));
            calims.Add(new Claim(ClaimTypes.Role, "Admin"));


            var calimsIdentity= new ClaimsIdentity(calims,CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(calimsIdentity));



          
            return Ok("HelloWorld");
        }
        // هذا التابع بالوظيفة يجب ان يجلب من appsetting  معلومات المستخدم
        private object ValidateUserInformation(string username, string password)
        {
            return new AmazonUser() { FirstName = "Ahmad", LastName = "AboHumid",Username="Ahmad2024Super",UserId=1, Role="admin" };
        }*/
    }
}
