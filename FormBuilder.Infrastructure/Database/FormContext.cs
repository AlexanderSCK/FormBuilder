﻿using FormBuilder.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Infrastructure.Database;

public class FormContext : DbContext
{
    public DbSet<FormTemplate> FormTemplates { get; set; }
    public DbSet<Field> Fields { get; set; }

    public FormContext(DbContextOptions<FormContext> options) : base(options) { }

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