namespace VipFit.Core.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        public async Task<IEnumerable<Pass>> GetAsync()
        {
            return await db.Passes
            .AsNoTracking()
            .Include(p => p.PassTemplate)
            .Include(p => p.Client)
            .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Pass> GetAsync(Guid id) =>
            await db.Passes
            .AsNoTracking()
            .Include(p => p.PassTemplate)
            .Include(p => p.Client)
            .FirstOrDefaultAsync(p => p.Id == id);

        /// <inheritdoc/>
        public async Task<IEnumerable<Pass>> GetForClientAsync(Guid clientId) =>
            await db.Passes
            .Where(p => p.ClientId == clientId)
            .AsNoTracking()
            .Include(p => p.PassTemplate)
            .Include(p => p.Client)
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
