
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data.Models.Domain;

namespace WebApplication_Benzeine.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            #region Identity and Role configuration

            // IdentityConfiguration
            modelBuilder.Entity<AppUser>(u =>
            {
                u.HasMany(user => user.Claims)
                 .WithOne() // no lambda because entity has no NavigationProperty, only UserId, no User
                 .HasForeignKey(clm => clm.UserId)
                 .IsRequired();

                u.HasMany(user => user.Logins)
                 .WithOne()
                 .HasForeignKey(lgn => lgn.UserId)
                 .IsRequired();

                u.HasMany(user => user.Tokens)
                 .WithOne()
                 .HasForeignKey(tkn => tkn.UserId)
                 .IsRequired();

                u.HasMany(user => user.UserRoles)
                 .WithOne(ur => ur.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();

            });

            modelBuilder.Entity<AppRole>(r =>
            {
                r.HasMany(rl => rl.UserRoles)
                 .WithOne(ur => ur.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
            });


            #endregion


            base.OnModelCreating(modelBuilder);
        }

    }
}
