using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;

namespace FreeDOW.API.WebHost.Authentication
{
    public class JwtSchemeHandler: AuthenticationHandler<JwtSchemeOptions>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _jwtKey = "";
        private readonly IConfiguration _config;

        public JwtSchemeHandler(
            IOptionsMonitor<JwtSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpContextAccessor contextAccessor,
            IConfiguration config
            ) :base (options, logger, encoder, clock)
        {
            _contextAccessor = contextAccessor;
            _config = config;
            _jwtKey = _config.GetSection("JwtKey").Value;

        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = "BustlerGreen",
                ValidAudience = "FreeDOW",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey))
            };
        }


private bool IsTokenValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token, GetValidationParameters(), out var f);
                return true;
            }
            catch { return false; }
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var tokenPresented = this
                ._contextAccessor
                .HttpContext
                .Request
                .Headers
                .TryGetValue("Authorization", out var token);
            if (!tokenPresented) return Task.FromResult(AuthenticateResult.Fail("no Jwt token presented"));

            #region Deconstruct and check token
            // https://stackoverflow.com/questions/38725038/c-sharp-how-to-verify-signature-on-jwt-token
            
            var handler = new JwtSecurityTokenHandler();
            token = token.ToString().Substring("Bearer ".Length);
            var parts = token.ToString().Split(".".ToCharArray());
            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            var bytesToSign = Encoding.UTF8.GetBytes($"{header}.{payload}");
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            var algorithm = new HMACSHA256(key);
            var hmac = algorithm.ComputeHash(bytesToSign);
            var calculatedSignature = Base64UrlEncode(hmac);
            if (!calculatedSignature.Equals(signature))
                return Task.FromResult(AuthenticateResult.Fail("token isn't valid"));
            #endregion
            
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            var identity = new ClaimsIdentity(claims, this.Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Split('=')[0]
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
