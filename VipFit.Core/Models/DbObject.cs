using System.ComponentModel.DataAnnotations;

namespace VipFit.Core.Models
{
    /// <summary>
    /// Base class for database entities.
    /// </summary>
    public class DbObject
    {
        /// <summary>
        /// Gets or sets the database id.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
