using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.DataModels
{
    public class PizzeriaApiDBContext : IdentityDbContext<PizzeriaUser>
    {
        public PizzeriaApiDBContext(DbContextOptions<PizzeriaApiDBContext> options)

        : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

      
            builder.Entity<PizzeriaUser>(entity =>
            {
                entity.ToTable("Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

      
            builder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dishes");

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Description)
                    .HasMaxLength(500);

                entity.Property(d => d.Price)
                    .HasColumnType("decimal(18,2)");


               

                entity.HasOne(d => d.Category)
                    .WithMany(c => c.Dishes)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasIndex(d => d.Name);
            });

    
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);



               

                entity.HasIndex(c => c.Name).IsUnique();

            });

        
                builder.Entity<Ingredient>(entity =>
                {
                    entity.ToTable("Ingredients");

                    entity.Property(i => i.Name)
                        .IsRequired()
                        .HasMaxLength(100);

                    entity.Property(i => i.Price)
                        .HasColumnType("decimal(18,2)")
                        .IsRequired();

                   

                    entity.HasIndex(i => i.Name).IsUnique();
                });

                builder.Entity<DishIngredient>(entity =>
                {
                    entity.ToTable("DishIngredients");

                    entity.HasKey(di => new { di.DishId, di.IngredientId });

                    entity.Property(di => di.Quantity)
                            .HasColumnType("decimal(18,3)")  
                            .IsRequired();

                    entity.Property(di => di.Unit)
                         .HasConversion<string>()
                         .HasMaxLength(20);

                  



                    entity.HasOne(di => di.Dish)
                        .WithMany(d => d.DishIngredients)
                        .HasForeignKey(di => di.DishId);

                    entity.HasOne(di => di.Ingredient)
                        .WithMany(i => i.DishIngredients)
                        .HasForeignKey(di => di.IngredientId);
                });

         
                builder.Entity<Order>(entity =>
                {
                    entity.ToTable("Orders");

                    entity.Property(o => o.TotalPrice)
                        .HasColumnType("decimal(18,2)");

                    entity.Property(o => o.CreatedAt)
                        .HasDefaultValueSql("GETUTCDATE()");

                    entity.Property(o => o.Status)
                        .HasConversion<string>()
                        .HasMaxLength(50)
                        .IsRequired();

                    entity.HasOne(o => o.User)
                        .WithMany(u => u.Orders)
                        .HasForeignKey(o => o.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasIndex(o => o.CreatedAt);
                    entity.HasIndex(o => o.Status);
                });

         
                builder.Entity<OrderItem>(entity =>
                {
                    entity.ToTable("OrderItems");

                    entity.HasOne(oi => oi.Order)
                        .WithMany(o => o.Items)
                        .HasForeignKey(oi => oi.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(oi => oi.Dish)
                        .WithMany(d => d.OrderItems)
                        .HasForeignKey(oi => oi.DishId)
                         .OnDelete(DeleteBehavior.Restrict);



                    entity.Property(oi => oi.Quantity)
                        .IsRequired();
                });

        }
            
    }
}
