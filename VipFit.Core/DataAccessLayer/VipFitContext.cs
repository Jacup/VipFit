﻿namespace VipFit.Core.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using VipFit.Core.Enums;
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
        /// Initializes base db.
        /// </summary>
        public void Initialize()
        {
            Database.EnsureCreated();

            SeedClients();
            SeedPassTemplates();
            SeedPasses();
        }

        /// <summary>
        /// Initializes on creating db model.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable(nameof(Client));
            modelBuilder.Entity<PassTemplate>().ToTable(nameof(PassTemplate));
            modelBuilder.Entity<Pass>()
                .HasMany(e => e.Entries)
                .WithOne(e => e.Pass)
                .HasForeignKey(e => e.PassId)
                .IsRequired();
            modelBuilder.Entity<Entry>()
                .HasOne(e => e.Pass)
                .WithMany(e => e.Entries)
                .HasForeignKey(e => e.PassId)
                .IsRequired();
        }

        private void SeedClients()
        {
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

        private void SeedPassTemplates()
        {
            if (PassTemplates.Any())
                return;

            var passTemplates = new PassTemplate[]
            {
                new PassTemplate(PassType.Standard, PassDuration.Short, 1500m),
                new PassTemplate(PassType.Standard, PassDuration.Medium, 2760m),
                new PassTemplate(PassType.Standard, PassDuration.Long, 4560m),
                new PassTemplate(PassType.Pro, PassDuration.Short, 920m),
                new PassTemplate(PassType.Pro, PassDuration.Medium, 2520m),
                new PassTemplate(PassType.Pro, PassDuration.Long, 7200m),
            };

            foreach (var p in passTemplates)
                PassTemplates.Add(p);

            SaveChanges();
        }

        private void SeedPasses()
        {
            if (Passes.Any() || !Clients.Any())
                return;

            var startingDate = DateOnly.Parse("01.06.2023");
            var now = DateTime.Now;

            var clientId = Clients.FirstOrDefault().Id;
            var pt = PassTemplates.FirstOrDefault();

            var passes = new Pass[]
            {
                new Pass(startingDate, startingDate.AddMonths(pt.MonthsDuration), now, now, clientId,pt.Id, new Entry[pt.Entries]),
            };

            foreach (var p in passes)
                Passes.Add(p);

            SaveChanges();
        }
    }
}
