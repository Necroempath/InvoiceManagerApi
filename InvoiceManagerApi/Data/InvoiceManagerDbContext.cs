using InvoiceManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Data;

public class InvoiceManagerDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    public InvoiceManagerDbContext(DbContextOptions<InvoiceManagerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.Id);

            customer.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

            customer.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

            customer.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

            customer.Property(c => c.CreatedAt)
            .IsRequired();

        });

        modelBuilder.Entity<Invoice>(invoice =>
        {
            invoice.HasKey(i => i.Id);

            invoice.Property(i => i.StartDate)
            .IsRequired();

            invoice.Property(i => i.EndDate)
            .IsRequired();

            invoice.Property(i => i.CreatedAt)
            .IsRequired();

            invoice.ToTable(t => t.HasCheckConstraint(
                "CK_Invoice_StartDate_Less_Than_EndDate", "[StartDate] < [EndDate]"
                ));

            invoice.HasOne(i => i.Customer)
            .WithOne(c => c.Invoice)
            .HasForeignKey<Invoice>(i => i.CustomerId)
            .IsRequired(false);
        });

        modelBuilder.Entity<InvoiceRow>(invoiceRow =>
        {
            invoiceRow.HasKey(i => i.Id);

            invoiceRow.Property(i => i.Quantity)
            .IsRequired();

            invoiceRow.Property(i => i.Rate)
            .IsRequired();

            invoiceRow.Property(i => i.Sum)
            .IsRequired();

            invoiceRow.Property(i => i.Service)
            .HasMaxLength(100)
            .IsRequired();

            invoiceRow.ToTable(t => t.HasCheckConstraint(
                "CK_InvoiceRow_Quantity_Positive", "[Quantity] > 0"
                ));

            invoiceRow.ToTable(t => t.HasCheckConstraint(
                "CK_InvoiceRow_Rate_Positive", "[Rate] > 0"
                ));

            invoiceRow.ToTable(t => t.HasCheckConstraint(
                "CK_InvoiceRow_Sum_Positive", "[Sum] > 0"
                ));

            invoiceRow.HasOne(i => i.Invoice)
            .WithMany(i => i.Rows)
            .HasForeignKey(i => i.InvoiceId);
        });
    }

}
