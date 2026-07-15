using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Shared.Repositories;
using BankingApi.Shared.requests;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly ApplicationDbContext _Db;
        private readonly IAuth auth;
        public AuthController(ApplicationDbContext Db, IAuth auth)
        {
            _Db = Db;
            this.auth = auth;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> LoginFunc([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var CheckIfUserExists = await _Db.CustomersTable.FirstOrDefaultAsync
                                        (a => a.CustomerEmail == loginRequest.UserEmail
                                         && a.CustomerNationalId == loginRequest.UserNId);

                if (CheckIfUserExists != null)
                {
                    // Exists

                    return Ok(new LoginResponse
                    {
                        IsSuccess = true,
                        Message = "User Is Logged in",
                        Token = auth.GenereateToken(loginRequest.UserEmail, loginRequest.UserNId),
                    });
                }
                else
                {
                    return BadRequest(new LoginResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid email or national ID",
                        Token = null
                    });
                }

            }
            else
            {
                return BadRequest(new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid request data",
                    Token = null
                });
            }
        }



        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterationRequest RegRequest)
        {
            if (ModelState.IsValid)
            {
                var response = await auth.RegisterUser(RegRequest);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest(new GeneralResponse
                {
                    IsSuccessful = false,
                    Message = "Invalid request data"
                });
            }
        }


    }
}