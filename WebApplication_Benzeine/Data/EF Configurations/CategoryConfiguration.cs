using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication_Benzeine.Data
{
    /// <summary>
    /// Category entity configurations that is applied in DataContext.OnModelCreating 
    /// </summary>
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Name);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();  
        }
    }
}
