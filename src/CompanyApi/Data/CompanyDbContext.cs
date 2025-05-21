using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

public partial class CompanyDbContext : DbContext
{
    public CompanyDbContext()
    {
    }

    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceEmployee> DeviceEmployees { get; set; }

    public virtual DbSet<DeviceType> DeviceTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=APBD;User ID=sa;Password=11223344Aa;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.Property(e => e.AdditionalProperties).HasDefaultValue("");
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);

            entity.HasOne(d => d.DeviceType).WithMany(p => p.Devices).HasConstraintName("FK_Device_DeviceType");
        });

        modelBuilder.Entity<DeviceEmployee>(entity =>
        {
            entity.Property(e => e.IssueDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeviceEmployee_Device");

            entity.HasOne(d => d.Employee).WithMany(p => p.DeviceEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeviceEmployee_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.HireDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Person).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Person");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Position");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
