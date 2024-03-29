using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Helpers;
using WebApplication_Benzeine.Options;
using WebApplication_Benzeine.Services;
using WebApplication_Benzeine.Data.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.Encodings.Web;

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

            services.Configure<IdentityOptions>(ops =>
            {
                ops.Password.RequireDigit = true;
                ops.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                ops.Lockout.MaxFailedAccessAttempts = 5;
                ops.User.RequireUniqueEmail = true;
            });

           



            services.AddCors();
            services.AddHttpContextAccessor();

            services.AddJwtBearer(Configuration);
            services.ConfigureSwagger();

            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<Data.DataContext>();

            services.AddDbContext<Data.DataContext>(ops => ops.UseSqlServer(GetConnectionString())
                                                             .UseLazyLoadingProxies());

            HtmlEncoder.Default.Encode("String");

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEmailSender, EmailSender>();

            // Options
            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseAuthentication();
            

            app.UseSwagger();
            app.UseSwaggerUI(ops =>
            {
                ops.SwaggerEndpoint("/swagger/v1/swagger.json", "");
                // to access swagger with root url => ~/
                ops.RoutePrefix = string.Empty;
            });

            app.UseCors(c=>c.AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
