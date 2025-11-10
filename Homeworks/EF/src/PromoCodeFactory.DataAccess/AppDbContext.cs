using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>()
                .HasOne<Role>(e => e.Role)
                .WithMany(r => r.Employees);

            modelBuilder.Entity<PromoCode>()
                .HasOne<Customer>(p => p.Customer)
                .WithMany(c => c.Promocodes);

            modelBuilder.Entity<PromoCode>()
                .HasOne<Preference>(p => p.Preference)
                .WithMany(c => c.PromoCodes);

            modelBuilder.Entity<PromoCode>()
                .HasOne<Employee>(p => p.PartnerManager)
                .WithMany(e => e.PromoCodes)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CustomerPreference>().HasKey(sc => new { sc.CustomerId, sc.PreferenceId });

            //modelBuilder.Entity<CustomerPreference>()
            //    .HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            //modelBuilder.Entity<CustomerPreference>()
            //    .HasOne<Customer>(cp => cp.Customer)
            //    .WithMany(c => c.CustomerPreferences);

            //modelBuilder.Entity<CustomerPreference>()
            //    .HasOne<Preference>(cp => cp.Preference)
            //    .WithMany(c => c.CustomerPreferences);

            modelBuilder.Entity<Employee>().Property(c => c.FirstName).HasMaxLength(150);
            modelBuilder.Entity<Employee>().Property(c => c.LastName).HasMaxLength(200);
            modelBuilder.Entity<Employee>().Property(c => c.Email).HasMaxLength(250);

            modelBuilder.Entity<Role>().Property(c => c.Name).HasMaxLength(150);
            modelBuilder.Entity<Role>().Property(c => c.Description).HasMaxLength(300);

            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(150);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(200);
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(250);

            modelBuilder.Entity<Preference>().Property(c => c.Name).HasMaxLength(150);

            modelBuilder.Entity<PromoCode>().Property(c => c.Code).HasMaxLength(100);
            modelBuilder.Entity<PromoCode>().Property(c => c.ServiceInfo).HasMaxLength(300);
            modelBuilder.Entity<PromoCode>().Property(c => c.PartnerName).HasMaxLength(300);

        }

    }
}