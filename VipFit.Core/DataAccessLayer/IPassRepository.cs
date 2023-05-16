namespace VipFit.Core.DataAccessLayer
{
    using VipFit.Core.Models;

    /// <summary>
    /// Defines methods to interact with Pass backend.
    /// </summary>
    public interface IPassRepository
    {
        /// <summary>
        /// Gets collection of Passes from database.
        /// </summary>
        /// <returns>Collection of Pass.</returns>
        Task<IEnumerable<Pass>> GetAsync();

        /// <summary>
        /// Gets Pass from database.
        /// </summary>
        /// <param name="id">Pass ID.</param>
        /// <returns>Pass with provided ID, otherwise null.</returns>
        Task<Pass> GetAsync(Guid id);

        /// <summary>
        /// Returns all the given client's passes.
        /// </summary>
        /// <param name="clientId">Client id associated with pass.</param>
        /// <returns>Collection of found passes associated with provided client.</returns>
        Task<IEnumerable<Pass>> GetForClientAsync(Guid clientId);

        /// <summary>
        /// Update or insert pass into database.
        /// </summary>
        /// <param name="pass">Pass to update/insert.</param>
        /// <returns>Current Pass.</returns>
        Task<Pass> UpsertAsync(Pass pass);

        /// <summary>
        /// Deletes Pass entry from database with dependencies.
        /// </summary>
        /// <param name="id">Pass ID.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}
