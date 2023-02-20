using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Entity;

public partial class TestInvoiceContext : DbContext
{
    public TestInvoiceContext()
    {
    }

    public TestInvoiceContext(DbContextOptions<TestInvoiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-AURP706;Database=Test_Invoice; User Id=edwinoDB; Password=123456789; Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Customer");

            entity.Property(e => e.Adress).HasMaxLength(120);
            entity.Property(e => e.CustName).HasMaxLength(70);
            entity.Property(e => e.CustomerTypeId).HasDefaultValueSql("((1))");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.CustomerType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_CustomerTypes");
        });

        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerType");

            entity.Property(e => e.Description).HasMaxLength(70);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.TotalItbis).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Invoice_Customers");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.TotalItbis).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_InvoiceDetail_Invoice");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
