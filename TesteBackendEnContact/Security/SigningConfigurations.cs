using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TesteBackendEnContact.Security
{
    public class SigningConfigurations
    {
        private const string SECRET_KEY = "ase1s9e6-9173-b6b7-a6d6fe6se6a6";

        public SigningCredentials signingCredentials { get; }
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));

        public SigningConfigurations()
        {
            signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
