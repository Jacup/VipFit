namespace VipFit.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Base class for database entities.
    /// </summary>
    public class DbObject
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
