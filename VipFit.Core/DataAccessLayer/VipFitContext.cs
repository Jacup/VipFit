namespace VipFit.Core.DataAccessLayer
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
        /// Gets or sets the PassTemplates DbSet.
        /// </summary>
        public DbSet<PassTemplate> PassTemplates { get; set; }

        /// <summary>
        /// Gets or sets the Pass DbSet.
        /// </summary>
        public DbSet<Pass> Passes { get; set; }

        /// <summary>
        /// Gets or sets the Entry DbSet.
        /// </summary>
        public DbSet<Entry> Entries { get; set; }

        /// <summary>
        /// Gets or sets the Payments DbSet.
        /// </summary>
        public DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Initializes base db.
        /// </summary>
        public void Initialize()
        {
            CreateApplicationFolder(GetApplicationDirectory());

            Database.EnsureCreated();
        }

        /// <summary>
        /// Gets application folder.
        /// </summary>
        /// <returns>Path.</returns>
        public static string GetApplicationDirectory() =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "VipFit App");

        /// <summary>
        /// Initializes on creating db model.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().Navigation(c => c.Passes).AutoInclude();
            modelBuilder.Entity<Pass>().Navigation(p => p.PassTemplate).AutoInclude();
            modelBuilder.Entity<Pass>().Navigation(p => p.Entries).AutoInclude();
            modelBuilder.Entity<Pass>().Navigation(p => p.Client).AutoInclude();
        }

        private static void CreateApplicationFolder(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}
