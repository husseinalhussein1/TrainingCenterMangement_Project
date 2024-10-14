using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IAccountRepository accountRepository;

        public AccountsController(IConfiguration configuration, IAccountRepository accountRepository)
        {
            this.configuration = configuration;
            this.accountRepository = accountRepository;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(AuthRequest request)
        {
            // جلب المستخدم بناءً على اسم المستخدم وكلمة المرور
            var userAccount = accountRepository.GetAccount(request.Username, request.Password);

            if (userAccount == null)
            {
                return Unauthorized();
            }

            // تحديد الدور بناءً على نوع الحساب
            string role = GetRoleForAccount(userAccount);

            // إعدادات المفتاح السري والتوقيع
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // إعداد الـ Claims بناءً على الأدوار
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.UserName),
                new Claim(ClaimTypes.Role, role) // هنا نحدد الدور
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: configuration["Authentication:Issuer"],
                audience: configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = token });
        }

        // دالة لتحديد الدور بناءً على نوع الحساب
        private string GetRoleForAccount(Account userAccount)
        {
            if (userAccount.Administrator != null)
            {
                return "Administrator";
            }
            else if (userAccount.TrainingOfficer != null)
            {
                return "TrainingOfficer";
            }
            else if (userAccount.Receptionist != null)
            {
                return "Receptionist";
            }
            else if (userAccount.Trainer != null)
            {
                return "Trainer";
            }

            else if (userAccount.Trainee != null)
            {
                return "Trainee";
            }
            return "UnUser"; // إذا لم يكن له أي دور محدد
        }
    }
}

// Administrator,TrainingOfficer,Receptionist,Trainer,Trainee

//[AllowAnonymous]
//[Authorize(Roles = "TrainingOfficer")]
//[Authorize(Roles = "Receptionist")]
//[Authorize(Roles = "Trainer")]
//[Authorize(Roles = "Trainee")]
//[AllowAnonymous]

