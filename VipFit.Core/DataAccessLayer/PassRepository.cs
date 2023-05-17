namespace VipFit.Core.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with Pass database using EF Core.
    /// </summary>
    public class PassRepository : IPassRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassRepository"/> class.
        /// </summary>
        /// <param name="db">VipFit Context.</param>
        public PassRepository(VipFitContext db) => this.db = db;

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var current = await db.PassTemplates.FirstOrDefaultAsync(p => p.Id == id);

            if (current == null)
                return;

            // Remove dependencies?
            db.PassTemplates.Remove(current);
            await db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Pass>> GetAsync() =>
            await db.Passes
            .AsNoTracking()
            .ToListAsync();

        /// <inheritdoc/>
        public async Task<Pass> GetAsync(Guid id) =>
            await db.Passes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        /// <inheritdoc/>
        public async Task<IEnumerable<Pass>> GetForClientAsync(Guid clientId) =>
            await db.Passes
            .Where(p => p.ClientId == clientId)
            .AsNoTracking()
            .ToListAsync();

        /// <inheritdoc/>
        public async Task<Pass> UpsertAsync(Pass pass)
        {
            var current = await db.Passes.FirstOrDefaultAsync(p => p.Id == pass.Id);

            if (current == null)
                db.Add(pass);
            else
                db.Entry(current).CurrentValues.SetValues(pass);

            await db.SaveChangesAsync();

            return pass;
        }
    }
}
