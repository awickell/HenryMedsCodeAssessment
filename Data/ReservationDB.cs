using Microsoft.EntityFrameworkCore;

namespace HenryMedsCodeAssessment.Data
{
    public class ReservationDb : DbContext
    {
        public ReservationDb(DbContextOptions options) : base(options)
        {
            // Create new database and DbSet tables in this context if they don't already exist
            // TODO: For full EF-based apps, use migrations to track schema changes

            //Database.EnsureDeleted();
            Database.EnsureCreated();
            
            // Seed providert data
            if(!Providers.Any())
            {
                Providers.Add(new Providers
                {
                    display_name = "Test provider 1",
                    id = Guid.NewGuid()
                });

                Providers.Add(new Providers
                {
                    display_name = "Test provider 2",
                    id = Guid.NewGuid()
                });

                SaveChanges();
            }
        }

        public DbSet<Providers> Providers { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<ProviderSchedules> ProviderSchedules { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
    }
}
