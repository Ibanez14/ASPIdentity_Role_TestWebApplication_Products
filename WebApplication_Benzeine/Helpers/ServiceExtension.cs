using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication_Benzeine.Models.RequestDTO;
using WebApplication_Benzeine.Options;
using WebApplication_Benzeine.Swagger_Example_model_Providers;

namespace WebApplication_Benzeine.Helpers
{
    public static class ServiceExtension
    {
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

            // Add tokenParameters to singleton so that to use these paramters
            // in AuthenticationService when creating JWT
            // DRY principle baby

            services.AddSingleton<TokenValidationParameters>(tokenValidParameters);

            #region Local Method

            TokenValidationParameters CreateTokenParameters(JwtOptions options)
            {
                return new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true
                };
            }
            #endregion
        }


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var assemblyName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml");
            var pathToAssembly = Path.Combine(AppContext.BaseDirectory, assemblyName);

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Benzeine. Simple API service for testing Angular",
                    Version = "v1",
                    Description = $"",
                });



                ops.ExampleFilters();
                ops.IncludeXmlComments(pathToAssembly);

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                    ,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                };
                ops.AddSecurityDefinition("Bearer", securitySchema);


                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                ops.AddSecurityRequirement(securityRequirement);




                ops.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

        }



        #region Private Methods
        #endregion
    }
}
