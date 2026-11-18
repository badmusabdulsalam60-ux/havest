
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace AgroConn.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<FarmerProfile> FarmerProfiles => Set<FarmerProfile>();
    public DbSet<BuyerProfile> BuyerProfiles => Set<BuyerProfile>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<ProduceListing> ProduceListings => Set<ProduceListing>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
        modelBuilder.Entity<Crop>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<FarmerProfile>().HasIndex(x => x.UserId).IsUnique();
        modelBuilder.Entity<BuyerProfile>().HasIndex(x => x.UserId).IsUnique();
        modelBuilder.Entity<Order>().HasIndex(x => x.ReservationId).IsUnique();

        modelBuilder.Entity<User>().HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<FarmerProfile>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<BuyerProfile>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProduceListing>().HasOne(x => x.FarmerProfile).WithMany().HasForeignKey(x => x.FarmerProfileId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProduceListing>().HasOne(x => x.Crop).WithMany().HasForeignKey(x => x.CropId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(x => x.ProduceListing).WithMany().HasForeignKey(x => x.ProduceListingId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(x => x.BuyerProfile).WithMany().HasForeignKey(x => x.BuyerProfileId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>().HasOne(x => x.Reservation).WithOne().HasForeignKey<Order>(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProduceListing>().Property(x => x.PricePerUnit).HasPrecision(18, 2);
        modelBuilder.Entity<ProduceListing>().Property(x => x.TotalQuantity).HasPrecision(18, 2);
        modelBuilder.Entity<ProduceListing>().Property(x => x.ReservedQuantity).HasPrecision(18, 2);
        modelBuilder.Entity<Reservation>().Property(x => x.Quantity).HasPrecision(18, 2);

        // every seeded row needs an Id, otherwise the migration fails
        modelBuilder.Entity<Role>().HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Admin", Description = "Platform administrator" },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Farmer", Description = "Registered Farmer" },
            new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Buyer", Description = "Registered Buyer" }
        );

        modelBuilder.Entity<Crop>().HasData(
            new { Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Maize", Category = "Grain", Description = (string?)"Corn / maize", IsActive = true },
            new { Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Tomato", Category = "Vegetable", Description = (string?)"Fresh tomatoes", IsActive = true },
            new { Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Cassava", Category = "Tuber", Description = (string?)"Cassava tubers", IsActive = true },
            new { Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Yam", Category = "Tuber", Description = (string?)"Yam tubers", IsActive = true },
            new { Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"), CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = (DateTime?)null, IsDeleted = false, Name = "Rice", Category = "Grain", Description = (string?)"Paddy / milled rice", IsActive = true }
        );
    }
}
