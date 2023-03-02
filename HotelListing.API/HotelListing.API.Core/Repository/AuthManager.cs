using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.UserDto;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _apiUser;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _apiUser = _mapper.Map<ApiUser>(userDto);
            _apiUser.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_apiUser, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_apiUser, "User");
            }

            return result.Errors;
        }

        public async Task<AuthenticationResponseDto> Login(ApiLoginUserDto loginUserDto)
        {
            _logger.LogInformation($"Looking for user with email {loginUserDto.Email}");
            _apiUser = await _userManager.FindByEmailAsync(loginUserDto.Email);
            var isValidUser = await _userManager.CheckPasswordAsync(_apiUser, loginUserDto.Password);

            if (_apiUser is null || isValidUser is false)
            {
                _logger.LogWarning($"User with email {loginUserDto.Email} was not found.");
                return default;
            }
            var token = await GenerateToken();
            _logger.LogInformation($"Token generated for user with email {loginUserDto.Email} | Token: {token}");

            return new AuthenticationResponseDto
            {
                Token = token,
                UserId = _apiUser.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_apiUser, _loginProvider, _refreshToken);

            var newRefreshToken =
                await _userManager.GenerateUserTokenAsync(_apiUser, _loginProvider, _refreshToken);

            var result = await _userManager.SetAuthenticationTokenAsync(_apiUser, _loginProvider, _refreshToken,
                    newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthenticationResponseDto> VerifyRefreshToken(AuthenticationResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            var userName = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            _apiUser = await _userManager.FindByNameAsync(userName);

            if (_apiUser is null || _apiUser.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken =
                await _userManager.VerifyUserTokenAsync(_apiUser, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthenticationResponseDto
                {
                    Token = token,
                    UserId = _apiUser.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_apiUser);
            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var role = await _userManager.GetRolesAsync(_apiUser);

            var roleClaims = role.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(_apiUser);
            var expire = Convert.ToInt32(_configuration["JWTSettings:DurationInMinutes"]);

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _apiUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, _apiUser.Email),
                    new Claim("UID", _apiUser.Id),
                }
                .Union(userClaims)
                .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expire),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}