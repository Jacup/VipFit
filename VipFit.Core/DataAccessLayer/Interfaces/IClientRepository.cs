namespace VipFit.Core.DataAccessLayer.Interfaces
{
    using VipFit.Core.Models;

    /// <summary>
    /// Defines methods to interact with clients backend.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Gets collection of clients from database.
        /// </summary>
        /// <returns>Collection of clients.</returns>
        Task<IEnumerable<Client>> GetAsync();

        /// <summary>
        /// Gets client from database.
        /// </summary>
        /// <param name="id">Cliend ID.</param>
        /// <returns>Client with provided ID, otherwise null.</returns>
        Task<Client> GetAsync(Guid id);

        /// <summary>
        /// Gets clients from database by content. For search purposes.
        /// </summary>
        /// <param name="args">Parameters to look for in database.</param>
        /// <returns>Collection of clients.</returns>
        Task<IEnumerable<Client>> GetAsync(string args);

        /// <summary>
        /// Update or insert client into database.
        /// </summary>
        /// <param name="client">Client to update/insert.</param>
        /// <returns>Current client.</returns>
        Task<Client> UpsertAsync(Client client);

        /// <summary>
        /// Deletes client entry from database with dependencies.
        /// </summary>
        /// <param name="id">Client ID.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}