using System;
using System.Collections.Generic;
using Caleb_Marks_Assignment_11._1.Models;
using Microsoft.EntityFrameworkCore;

// Scaffold-DbContext "Data Source=localhost;Initial Catalog=CCAD23BookInventory;Integrated Security=True;Encrypt=False;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Force

namespace Caleb_Marks_Assignment_11._1.Data;

public partial class Ccad23bookInventoryContext : DbContext
{
    public Ccad23bookInventoryContext()
    {
    }

    public Ccad23bookInventoryContext(DbContextOptions<Ccad23bookInventoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CCAD23BookInventory;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Book__9271CED1A8EC2FF3");

            entity.ToTable("Book");

            entity.Property(e => e.Isbn).HasMaxLength(13);
            entity.Property(e => e.AuthorName).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
