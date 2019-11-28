using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication_Benzeine.Data
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
