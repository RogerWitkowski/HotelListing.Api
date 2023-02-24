using HotelListing.API.Contracts;
using HotelListing.API.DtoModels.UserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        // api/Account/Register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] ApiLoginUserDto loginUserDto)
        {
            var authenticationResponse = await _authManager.Login(loginUserDto);
            if (authenticationResponse is null)
            {
                return Unauthorized();
            }

            return Ok(authenticationResponse);
        }

        // POST: api/Account/refresh-token
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthenticationResponseDto authenticationResponseDto)
        {
           
            var authResponse = await _authManager.VerifyRefreshToken(authenticationResponseDto);

            if (authenticationResponseDto.RefreshToken != authResponse.RefreshToken)
            {
                return BadRequest();
            }
            if (authResponse is null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
    }
}