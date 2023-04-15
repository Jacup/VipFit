namespace VipFit.Core.Models
{
    /// <summary>
    /// Base class for database entities.
    /// </summary>
    internal class DbObject
    {
        /// <summary>
        /// Gets or sets the database id.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
