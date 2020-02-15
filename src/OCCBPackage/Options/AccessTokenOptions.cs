using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace OCCBPackage.Options
{
    public class AccessTokenOptions
    {
        public const string TokenName = "access_token";

        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public DateTime IssuedAt => DateTime.UtcNow;

        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime Expires => IssuedAt.AddMinutes(ExpiresInMinutes);

        public double ExpiresInMinutes { get; set; } = 120.0;

        public bool IsTransferTokenOverHttps { get; set; } = true;

        public bool IsStoreTokenAfterAuthentication { get; set; }

        public TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true) =>
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),

                ValidateIssuer = true,
                ValidIssuer = Issuer,

                ValidateAudience = true,
                ValidAudience = Audience,

                ValidateLifetime = validateLifetime,

                ClockSkew = TimeSpan.FromSeconds(5)
            };
    }
}
