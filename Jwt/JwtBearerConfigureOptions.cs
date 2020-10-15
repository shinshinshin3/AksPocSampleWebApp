using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


// 未実装。
namespace Jwt
{
    public class JwtBearerConfigureOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        public void Configure(string name, JwtBearerOptions options)
        {

            if (name != JwtBearerDefaults.AuthenticationScheme)
            {
                return;
            }


            options.TokenValidationParameters = new TokenValidationParameters
            {
                AudienceValidator = this.AudienceValidatorDelegate,
                ValidIssuer = JwtSecurityConfiguration.Issuer,
                IssuerSigningKey = JwtSecurityConfiguration.SecurityKey,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
            };
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }

        public bool AudienceValidatorDelegate(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            // 実際は、データベースを見たりする。
            return true;
        }
    }

    public class JwtSecurityConfiguration
    {
        // 署名用鍵。実際には設定等から取得する    
        private readonly static byte[] secret
            = Encoding.UTF8.GetBytes(new String('s', 128));

        public static SymmetricSecurityKey SecurityKey { get; }
            = new SymmetricSecurityKey(secret);

        public static string Issuer { get; } = "ExJwtAuth";
    }
}
