using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApplication_Benzeine.Data;
using Microsoft.EntityFrameworkCore.SqlServer;
using WebApplication_Benzeine.Helpers;
using WebApplication_Benzeine.Options;
using WebApplication_Benzeine.Services;

namespace WebApplication_Benzeine
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)=>
            Configuration = configuration;
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();


            services.ConfigureSwagger();
          
            // No need for IdentityRole
            services.AddIdentityCore<IdentityUser>()
                    .AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(ops => ops.UseSqlServer(GetConnectionString()));

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Options
            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

            services.AddJwtBearer(Configuration);
            services.AddAuthorization(ops =>
                ops.AddPolicy("OnlyBenzeine", policy => policy.RequireRole("Developer")));

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseSwagger();
            app.UseSwaggerUI(ops =>
            {
                ops.SwaggerEndpoint("/swagger/v1/swagger.json", "");
                // to access swagger with root url => ~/
                ops.RoutePrefix = string.Empty;
            });
           
            app.UseCors(ops => ops.AllowAnyOrigin());

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
