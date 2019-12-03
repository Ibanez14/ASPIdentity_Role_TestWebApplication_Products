using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication_Benzeine.Data
{
    /// <summary>
    /// Product entity configurations that is applied in DataContext.OnModelCreating
    /// </summary>
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(c => c.CategoryId);
            
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Products)
                   .HasForeignKey(p => p.UserId);
        }
    }
}
