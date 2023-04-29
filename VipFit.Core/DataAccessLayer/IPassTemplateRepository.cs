namespace VipFit.Core.DataAccessLayer
{
    using VipFit.Core.Models;

    /// <summary>
    /// Defines methods to interact with PassTemplates backend.
    /// </summary>
    public interface IPassTemplateRepository
    {
        /// <summary>
        /// Gets collection of PassTemplates from database.
        /// </summary>
        /// <returns>Collection of PassTemplates.</returns>
        Task<IEnumerable<PassTemplate>> GetAsync();

        /// <summary>
        /// Gets PassTemplate from database.
        /// </summary>
        /// <param name="id">PassTemplate ID.</param>
        /// <returns>PassTemplate with provided ID, otherwise null.</returns>
        Task<PassTemplate> GetAsync(Guid id);

        /// <summary>
        /// Update or insert client into database.
        /// </summary>
        /// <param name="PassTemplate">PassTemplate to update/insert.</param>
        /// <returns>Current PassTemplate.</returns>
        Task<PassTemplate> UpsertAsync(PassTemplate PassTemplate);

        /// <summary>
        /// Deletes PassTemplate entry from database with dependencies.
        /// </summary>
        /// <param name="id">PassTemplate ID.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}
