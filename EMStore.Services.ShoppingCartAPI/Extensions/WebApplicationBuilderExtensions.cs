using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EMStore.Services.ShoppingCartAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            var apiSettingsSection = builder.Configuration.GetSection("ApiSettings");

            var jwtSecret = apiSettingsSection.GetValue<string>("SecretKey");
            var jwtIssuer = apiSettingsSection.GetValue<string>("Issuer");
            var jwtAudience = apiSettingsSection.GetValue<string>("Audience");

            var encodedSecretKey = Encoding.ASCII.GetBytes(jwtSecret ?? string.Empty);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedSecretKey),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                };
            });

            return builder;
        }
    }
}
