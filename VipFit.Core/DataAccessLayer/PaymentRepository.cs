namespace VipFit.Core.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with Payment database using EF Core.
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRepository"/> class.
        /// </summary>
        /// <param name="db">VipFit context.</param>
        public PaymentRepository(VipFitContext db) => this.db = db;

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var current = await db.Payments.FirstOrDefaultAsync(p => p.Id == id);

            if (current == null)
                return;

            db.Payments.Remove(current);
            await db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Payment>> GetAsync() =>
            await db.Payments
            .AsNoTracking()
            .ToListAsync();

        /// <inheritdoc/>
        public async Task<Payment> GetAsync(Guid id) =>
            await db.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        /// <inheritdoc/>
        /// It could be optimized I guess.
        public async Task<IEnumerable<Payment>> GetForClientAsync(Guid clientId)
        {
            var passes = await db.Passes
                .Where(p => p.ClientId == clientId)
                .Select(p => p.Id)
                .ToListAsync();

            List<Payment> payments = new();

            foreach (var pass in passes)
                payments.AddRange(await GetForPassAsync(pass));

            return payments;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Payment>> GetForPassAsync(Guid passId) =>
            await db.Payments
            .AsNoTracking()
            .Where(p => p.PassId == passId)
            .ToListAsync();

        /// <inheritdoc/>
        public async Task<Payment> UpsertAsync(Payment payment)
        {
            payment.Pass = null;

            var current = await db.Payments.FirstOrDefaultAsync(p => p.Id == payment.Id);

            if (current == null)
                db.Add(payment);
            else
                db.Entry(current).CurrentValues.SetValues(payment);

            await db.SaveChangesAsync();

            return payment;
        }
    }
}
