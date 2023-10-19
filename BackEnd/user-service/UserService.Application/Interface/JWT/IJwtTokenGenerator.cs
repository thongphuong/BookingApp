using UserService.Domain;

namespace UserService.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        public string GenerateJwtToken(UserService.Domain.User user);
    }
}
