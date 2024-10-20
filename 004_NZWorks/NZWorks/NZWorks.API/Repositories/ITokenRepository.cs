using Microsoft.AspNetCore.Identity;

namespace NZWorks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwTToken(IdentityUser user, List<string> roles);
    }
}
