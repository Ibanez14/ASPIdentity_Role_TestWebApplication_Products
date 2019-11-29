using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication_Benzeine.Models.RequestDTO;
using WebApplication_Benzeine.Options;

namespace WebApplication_Benzeine.Helpers
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Extension method for configure JWT Authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            var tokenValidParameters = CreateTokenParameters(jwtOptions);

            services.AddAuthentication
                        (ops =>
                        {
                            ops.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(ops =>
                        {
                            ops.TokenValidationParameters = tokenValidParameters;
                            ops.SaveToken = true;
                        });

            // Add tokenParameters to singleton so that to use in AuthenticationService when creating JWT
            // DRY principle baby
            services.AddSingleton<TokenValidationParameters>(tokenValidParameters);

            #region Local Method

            TokenValidationParameters CreateTokenParameters(JwtOptions options)=>
                new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true
                };
            
            #endregion
        }


        /// <summary>
        /// Extension method to to Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var assemblyName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml");
            var pathToAssembly = Path.Combine(AppContext.BaseDirectory, assemblyName);

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "Simple API service for testing Angular",
                    Version = "v1",
                    Description = $"API Service based on JWT authentication" +
                                  $"{Environment.NewLine}" +
                                  $"Where registered user can GET, ADD or DELETE products" +
                                  $"{Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"There is two roles: <b> User, Admin </b>" +
                                  $"{Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"<b>User</b>  => Can GET and ADD products, also may DELETE products that has been created by this User" +
                                  $"{Environment.NewLine}" +
                                  $"<b>Admin</b> => Can do the same, BUT may also delete all products no matter which user added it"
                });

                ops.IncludeXmlComments(pathToAssembly);

                #region Swagger Authorization Configuration

                var securitySchema = new ApiKeyScheme()
                {
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                };

                ops.AddSecurityDefinition("Bearer", securitySchema);

                var securityDictionary = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Array.Empty<string>()}
                };
                ops.AddSecurityRequirement(securityDictionary);

                #endregion
            });
        }
    }
}
