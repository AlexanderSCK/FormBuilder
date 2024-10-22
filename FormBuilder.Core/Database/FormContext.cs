using FormBuilder.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Core.Database
{
    public class FormContext : DbContext
    {
        public DbSet<FormTemplate> FormTemplates { get; set; }
        public DbSet<Field> Fields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Field>()
                .HasDiscriminator<FieldType>("FieldType")
                .HasValue<UserField>(FieldType.UserField)
                .HasValue<CalculatedField>(FieldType.CalculatedField);

            modelBuilder.Entity<FormTemplate>()
                .HasMany(f => f.Fields)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
