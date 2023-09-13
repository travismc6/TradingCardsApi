using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingCards.Models.Domain;

namespace TradingCards.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Brand>().HasData(new List<Brand>()
            {
                new Brand()
                {
                    Name = "Topps",
                    Id = 1
                },
                new Brand()
                {
                    Name = "Bowman",
                    Id = 2
                },
                new Brand()
                {
                    Name = "UpperDeck",
                    Id = 3
                }
            });
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CollectionCard> CollectionCards { get; set; }
        //public DbSet<Photo> Photos { get; set; }
    }
}
