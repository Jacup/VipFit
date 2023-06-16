namespace VipFit.Core.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with Entry database using EF Core.
    /// </summary>
    public class EntryRepository : IEntryRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryRepository"/> class.
        /// </summary>
        /// <param name="db">VipFit context.</param>
        public EntryRepository(VipFitContext db) => this.db = db;

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var current = await db.Entries.FirstOrDefaultAsync(p => p.Id == id);

            if (current == null)
                return;

            db.Entries.Remove(current);
            await db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Entry>> GetAsync() => await db.Entries.AsNoTracking().ToListAsync();

        /// <inheritdoc/>
        public async Task<Entry> GetAsync(Guid id) => await db.Entries.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        /// <inheritdoc/>
        /// It could be optimized I guess.
        public async Task<IEnumerable<Entry>> GetForClientAsync(Guid clientId)
        {
            var passes = await db.Passes
                .Where(p => p.ClientId == clientId)
                .Select(p => p.Id)
                .ToListAsync();

            List<Entry> entries = new();

            foreach (var pass in passes)
                entries.AddRange(await GetForPassAsync(pass));

            return entries;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Entry>> GetForPassAsync(Guid passId) => await db.Entries .Where(e => e.PassId == passId).ToListAsync();

        /// <inheritdoc/>
        public async Task<Entry> UpsertAsync(Entry entry)
        {
            entry.Pass = null;
            var current = await db.Entries.FirstOrDefaultAsync(p => p.Id == entry.Id);

            if (current == null)
                db.Add(entry);
            else
                db.Entry(current).CurrentValues.SetValues(entry);

            await db.SaveChangesAsync();

            return entry;
        }
    }
}
