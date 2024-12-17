using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;


namespace PromoCodeFactory.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Employee имеет ссылку на Role
            modelBuilder.Entity<Employee>()
                .HasOne<Role>(e => e.Role)
                .WithMany(r => r.Employees);

                //.HasForeignKey(e  => e.RoleId);
            //modelBuilder.Entity<Role>()
            //    .HasMany<Employee>(u => u.Employees);
            //    //.WithOne<Role>(q => q.Role);
            //    .IsRequired();
            //Связь Customer и Promocode реализовать через One-To-Many, промокод может быть выдан только одному клиенту.
            modelBuilder.Entity<PromoCode>()
                .HasOne<Customer>(p => p.Customer)
                .WithMany(c => c.Promocodes);

            modelBuilder.Entity<PromoCode>()
                .HasOne<Preference>(p => p.Preference)
                .WithMany(c => c.PromoCodes);

            modelBuilder.Entity<CustomerPreference>()
                .HasKey(cp => new { cp.CustomerId, cp.PreferenceId });
            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Customer>(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Preference>(cp => cp.Preference);

            //modelBuilder.Entity<Role>().HasData(
            //        new Role { Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"), Name = "PartnerManager", Description = "Партнерский менеджер" },
            //        new Role()
            //        {
            //            Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
            //            Name = "Admin",
            //            Description = "Администратор",
            //        }
            // );
            modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);
            modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees);
            modelBuilder.Entity<Preference>().HasData(FakeDataFactory.Preferences);

            //modelBuilder.Entity<PromoCode>().HasData(FakeDataFactory.PromoCodes);
            modelBuilder.Entity<CustomerPreference>().HasData(FakeDataFactory.CustomerPreferences);
            modelBuilder.Entity<Customer>().HasData(FakeDataFactory.Customers);
            //modelBuilder.Entity<Role>().HasData(
            //        new Role { Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"), Name = "PartnerManager", Description = "Партнерский менеджер" });
            //modelBuilder.Entity<Employee>().HasData(
            //        new Employee
            //        {
            //            Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
            //            Email = "owner@somemail.ru",
            //            FirstName = "Иван",
            //            LastName = "Сергеев",
            //            //Role = Roles.FirstOrDefault(x => x.Name == "Admin"),
            //            AppliedPromocodesCount = 5,
            //            RoleId = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665")
            //        });
        }
    }
}
