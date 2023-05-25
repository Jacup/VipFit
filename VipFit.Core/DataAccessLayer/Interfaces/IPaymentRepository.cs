namespace VipFit.Core.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VipFit.Core.Models;

    /// <summary>
    /// Defines methods to interact with Payments backend.
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Gets collection of Payments from database.
        /// </summary>
        /// <returns>Collection of payments.</returns>
        Task<IEnumerable<Payment>> GetAsync();

        /// <summary>
        /// Gets single Payment from database.
        /// </summary>
        /// <param name="id">Payment ID.</param>
        /// <returns>Payment with provided ID, otherwise null.</returns>
        Task<Payment> GetAsync(Guid id);

        /// <summary>
        /// Gets payments associated with given Client ID from database.
        /// </summary>
        /// <param name="clientId">Associated Client ID.</param>
        /// <returns>Collection of payments.</returns>
        Task<IEnumerable<Payment>> GetForClientAsync(Guid clientId);

        /// <summary>
        /// Gets payments associated with given Pass ID from database.
        /// </summary>
        /// <param name="passId">Associated Pass ID.</param>
        /// <returns>Collection of payments.</returns>
        Task<IEnumerable<Payment>> GetForPassAsync(Guid passId);

        /// <summary>
        /// Update or insert entry into database.
        /// </summary>
        /// <param name="entry">Payment to update/insert.</param>
        /// <returns>Current entry.</returns>
        Task<Payment> UpsertAsync(Payment entry);

        /// <summary>
        /// Deletes entry entry from database with dependencies.
        /// </summary>
        /// <param name="id">Payment ID.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}
