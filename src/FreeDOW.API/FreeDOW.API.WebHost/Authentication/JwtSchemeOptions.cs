using Microsoft.AspNetCore.Authentication;
namespace FreeDOW.API.WebHost.Authentication
{
    public class JwtSchemeOptions : AuthenticationSchemeOptions
    {
        public bool IsActive { get; set; }
    }
}
