using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MercuryApi.BLL
{
    public interface IUserBusinessLogic
    {
        Task<UserDto?> GetUserById(int userId);
        Task<string?> SignUp(UserUpsert request);
        Task<UserDto?> GetUserByUsername(string username);
        Task<string?> LogIn(UserUpsert request);
    }

    public class UserBusinessLogic : BusinessLogicBase, IUserBusinessLogic
    {
        private readonly IConfiguration _configuration;

        public UserBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, IConfiguration configuration) : base(repositoryManager, mapper)
        {
            _configuration = configuration;
        }

        public async Task<UserDto?> GetUserById(int userId)
        {
            User? user = await _repositoryManager.User.GetUserById(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<string?> SignUp(UserUpsert request)
        {
            // If username is already taken, don't create user and return null.
            if (await _repositoryManager.User.GetUserByUsername(request.Username) != null)
            {
                return null;
            }
            User user = _mapper.Map<User>(request);

            // Hash password.
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);
            user.Password = passwordHash;

            await _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();

            return CreateToken(user);
        }

        public async Task<UserDto?> GetUserByUsername(string username)
        {
            User? user = await _repositoryManager.User.GetUserByUsername(username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<string?> LogIn(UserUpsert request)
        {
            User? user = await _repositoryManager.User.GetUserByUsername(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
            {
                return null;
            }
            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            string signingKey = _configuration.GetSection("SigningKey").Value ?? throw new Exception("Coud not find signing key.");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
