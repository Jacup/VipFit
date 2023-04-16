using Microsoft.EntityFrameworkCore;
using VipFit.Core.Models;

namespace VipFit.Database
{
    public class VipFitContext : DbContext
    {
        /// <summary>
        /// Creates a new VipFit DbContext.
        /// </summary>
        /// <param name="options"></param>
        public VipFitContext(DbContextOptions<VipFitContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets the clients DbSet.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable(nameof(Client));
        }

        internal void Initialize()
        {
            Database.EnsureCreated();

            if (Clients.Any())
                return;

            var clients = new Client[]
            {
                new Client("Janusz", "Kowalski", "12456789", "email@gmail.com"),
                new Client("Adam", "Kowalski", "12456789", "email2@gmail.com"),
            };

            foreach (Client c in clients)
                Clients.Add(c);

            SaveChanges();
        }
    }
}
