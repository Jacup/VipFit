namespace VipFit.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using VipFit.Core.Models;

    /// <summary>
    /// Database context.
    /// </summary>
    public class VipFitContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VipFitContext"/> class.
        /// </summary>
        /// <param name="options">Db context options.</param>
        public VipFitContext(DbContextOptions<VipFitContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the clients DbSet.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Initializes base db.
        /// </summary>
        public void Initialize()
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

        /// <summary>
        /// Initializes on creating db model.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable(nameof(Client));
        }
    }
}
