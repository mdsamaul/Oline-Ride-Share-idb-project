using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.PhoneVerification;
using Oline_Ride_Share_idb_project.Server.Services;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService; 
        private readonly IUserService _userService;  
        private static string? storedVerificationCode;
        private static string? storedPhoneNumber;
        private readonly DatabaseDbContext _context;        

        public AuthController(IUserService userService, ITokenService tokenService, DatabaseDbContext context)
        {
            _userService = userService;
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("register")]
        public IActionResult RegisterPhoneNumber([FromBody] PhoneRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }
            storedVerificationCode = new Random().Next(1000, 9999).ToString();
            storedPhoneNumber = request.PhoneNumber;
            Console.WriteLine($"Verification code for {request.PhoneNumber}: {storedVerificationCode}");
            return Ok(new { Message = "Verification code generated. Check console for the code." });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] CodeVerificationRequest request)
        {
            if (request.PhoneNumber != storedPhoneNumber || request.Code != storedVerificationCode)
            {
                return BadRequest("Invalid phone number or verification code.");
            }
            bool exUser = _context.UserModels.Any(u => u.PhoneNumber == request.PhoneNumber);
            UserModel user;
            if (exUser)
            {
                user = _context.UserModels.FirstOrDefault(u => u.PhoneNumber == request.PhoneNumber);
                user.PhoneNumber = request.PhoneNumber;
                user.Role = request.Role;
                user.SetUpdateInfo();
                _context.UserModels.Update(user); 
            }
            else
            {
                user = new UserModel
                {
                    PhoneNumber = request.PhoneNumber,
                    Role = request.Role,
                };
                user.SetCreateInfo();
                _context.UserModels.Add(user);
            }
            await _context.SaveChangesAsync();
            var userId = user.UserModelId.ToString();
            var token = _tokenService.GenerateJwtToken(request.PhoneNumber, userId);
            return Ok(new JwtTokenResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(1) 
            });
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] string phoneNumber)
        {        
            storedVerificationCode = VerificationCode(phoneNumber);
            Console.WriteLine(storedVerificationCode);
            return Ok("Verification code sent to your phone.");
        }
        [HttpPost("verify-login")]
        public IActionResult VerifyLoginCode([FromBody] CodeVerificationRequest request)
        {
            var isVerified = VerifyVerificationCode(request.Code);

            if (isVerified)
            {
                var userId = GetUserIdByPhoneNumber(request.PhoneNumber);
                var token = _tokenService.GenerateJwtToken(request.PhoneNumber, userId);
                return Ok(new { Token = token }); 
            }
            return BadRequest("Verification failed.");
        }
        private string VerificationCode(string phoneNumber)
        {
            storedVerificationCode = new Random().Next(1000, 9999).ToString();
            return storedVerificationCode;
        }
        private bool VerifyVerificationCode(string code)
        {
            return code == storedVerificationCode; 
        }
        private string GetUserIdByPhoneNumber(string phoneNumber)
        {
            var user = _userService.GetUserByPhoneNumber(phoneNumber); 
            return user?.UserModelId.ToString() ?? throw new Exception("User not found");
        }
    }
}
