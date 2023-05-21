namespace VipFit.Core.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VipFit.Core.Models;

    /// <summary>
    /// Defines methods to interact with Entry backend.
    /// </summary>
    public interface IEntryRepository
    {
        /// <summary>
        /// Gets collection of Entries from database.
        /// </summary>
        /// <returns>Collection of entries.</returns>
        Task<IEnumerable<Entry>> GetAsync();

        /// <summary>
        /// Gets Entry from database.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        /// <returns>Entry with provided ID, otherwise null.</returns>
        Task<Entry> GetAsync(Guid id);

        /// <summary>
        /// Gets entries associated with given Client ID from database.
        /// </summary>
        /// <param name="clientId">Associated Client ID.</param>
        /// <returns>Collection of entries.</returns>
        Task<IEnumerable<Entry>> GetForClientAsync(Guid clientId);

        /// <summary>
        /// Gets entries associated with given Pass ID from database.
        /// </summary>
        /// <param name="passId">Associated Pass ID.</param>
        /// <returns>Collection of entries.</returns>
        Task<IEnumerable<Entry>> GetForPassAsync(Guid passId);

        /// <summary>
        /// Update or insert entry into database.
        /// </summary>
        /// <param name="entry">Entry to update/insert.</param>
        /// <returns>Current entry.</returns>
        Task<Entry> UpsertAsync(Entry entry);

        /// <summary>
        /// Deletes entry entry from database with dependencies.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}
