using Microsoft.EntityFrameworkCore;
using VipFit.Core.Models;
using Windows.Storage;

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
        }
    }
}
