using Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
        
    }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<MonthlySchedule> MonthlySchedules { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 37));
        optionsBuilder.UseMySql("Server=localhost;Port=3307;User=root;Password=admin;Database=FinanzasDB", serverVersion);

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        
        builder.Entity<Client>().ToTable("Clients");
        builder.Entity<Client>().HasKey(c => c.Id);
        builder.Entity<Client>().Property(c => c.Dni).IsRequired().HasMaxLength(8);
        builder.Entity<Client>().Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Entity<Client>().Property(c => c.LastName).IsRequired().HasMaxLength(50);
        builder.Entity<Client>().Property(c => c.Phone).IsRequired().HasMaxLength(9);
        builder.Entity<Client>().Property(c => c.Address).IsRequired().HasMaxLength(50);
        builder.Entity<Client>().Property(c => c.CreditLimit).IsRequired().HasColumnType("decimal(8,2)");
        builder.Entity<Client>().Property(c => c.BirthDate).IsRequired().HasColumnType("date");
        builder.Entity<Client>().Property(c => c.DateCreated).HasDefaultValue(DateTime.Now);
        builder.Entity<Client>().Property(c => c.IsActive).HasDefaultValue(true);
        
        builder.Entity<Loan>().ToTable("Loans");
        builder.Entity<Loan>().HasKey(l => l.Id);
        builder.Entity<Loan>().Property(l => l.Amount).IsRequired().HasColumnType("decimal(15,7)");
        builder.Entity<Loan>().Property(l => l.TotalInstallments).IsRequired();
        builder.Entity<Loan>().Property(l => l.DateCreated).HasDefaultValue(DateTime.Now);
        builder.Entity<Loan>().Property(l => l.IsActive).HasDefaultValue(true);
        
        builder.Entity<Schedule>().ToTable("Schedules");
        builder.Entity<Schedule>().HasKey(s => s.Id);
        builder.Entity<Schedule>().Property(s => s.InstallmentNumber).IsRequired();
        builder.Entity<Schedule>().Property(s => s.Amortization).IsRequired().HasColumnType("decimal(8,2)");
        builder.Entity<Schedule>().Property(s => s.Interest).IsRequired().HasColumnType("decimal(8,2)");
        builder.Entity<Schedule>().Property(s => s.InstallmentAmount).IsRequired().HasColumnType("decimal(12,2)");
        builder.Entity<Schedule>().Property(s => s.DueDate).IsRequired().HasColumnType("date");
        builder.Entity<Schedule>().Property(s => s.DateCreated).HasDefaultValue(DateTime.Now).HasColumnType("date");
        builder.Entity<Schedule>().Property(s => s.IsActive).HasDefaultValue(true);
        
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(50);
        builder.Entity<User>().Property(u => u.DateCreated).HasDefaultValue(DateTime.Now);
        builder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
        
        builder.Entity<Contract>().ToTable("Contracts");
        builder.Entity<Contract>().HasKey(ir => ir.Id);
        builder.Entity<Contract>().Property(ir => ir.TypeRate).IsRequired().HasMaxLength(8);
        builder.Entity<Contract>().Property(ir => ir.Rate).IsRequired().HasColumnType("decimal(15,7)");
        builder.Entity<Contract>().Property(ir => ir.Period).IsRequired();
        builder.Entity<Contract>().Property(ir => ir.TypePenaltyRate).IsRequired().HasMaxLength(8);
        builder.Entity<Contract>().Property(ir => ir.PenaltyRate).IsRequired().HasColumnType("decimal(15,7)");
        builder.Entity<Contract>().Property(ir => ir.PenaltyPeriod).IsRequired();
        builder.Entity<Contract>().Property(ir => ir.PaymentDay).IsRequired().HasColumnType("date");
        builder.Entity<Contract>().Property(ir => ir.DateCreated).HasDefaultValue(DateTime.Now);
        builder.Entity<Contract>().Property(ir => ir.IsActive).HasDefaultValue(true);
        
        builder.Entity<MonthlySchedule>().ToTable("MonthlySchedules");
        builder.Entity<MonthlySchedule>().HasKey(ms => ms.Id);
        builder.Entity<MonthlySchedule>().Property(ms => ms.Month).IsRequired();
        builder.Entity<MonthlySchedule>().Property(ms => ms.Year).IsRequired();
        builder.Entity<MonthlySchedule>().Property(ms => ms.Status).IsRequired().HasMaxLength(30);
        builder.Entity<MonthlySchedule>().Property(ms => ms.DateCreated).HasDefaultValue(DateTime.Now);
        builder.Entity<MonthlySchedule>().Property(ms => ms.IsActive).HasDefaultValue(true);
        
    }
}