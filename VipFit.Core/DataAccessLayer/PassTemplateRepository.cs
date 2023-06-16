namespace VipFit.Core.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with PassTemplates database using EF Core.
    /// </summary>
    public class PassTemplateRepository : IPassTemplateRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassTemplateRepository"/> class.
        /// </summary>
        /// <param name="db">VipFit Context.</param>
        public PassTemplateRepository(VipFitContext db) => this.db = db;

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var current = await db.PassTemplates.FirstOrDefaultAsync(p => p.Id == id);

            if (current == null)
                return;

            db.PassTemplates.Remove(current);
            await db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PassTemplate>> GetAsync() =>
            await db.PassTemplates
            .AsNoTracking()
            .ToListAsync();

        /// <inheritdoc/>
        public async Task<PassTemplate> GetAsync(Guid id) =>
            await db.PassTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        /// <inheritdoc/>
        public async Task<PassTemplate> UpsertAsync(PassTemplate passTemplate)
        {
            var current = await db.PassTemplates.FirstOrDefaultAsync(p => p.Id == passTemplate.Id);

            if (current == null)
                db.Add(passTemplate);
            else
                db.Entry(current).CurrentValues.SetValues(passTemplate);

            await db.SaveChangesAsync();

            return passTemplate;
        }
    }
}
