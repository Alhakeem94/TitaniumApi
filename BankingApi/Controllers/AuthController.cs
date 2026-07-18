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
                var AuthServiceResponse = await auth.UserLogin(loginRequest);
                if (AuthServiceResponse.IsSuccess == true)
                {
                    return Ok(AuthServiceResponse);
                }
                else
                {
                    return BadRequest(AuthServiceResponse);
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