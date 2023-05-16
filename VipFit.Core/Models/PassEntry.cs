namespace VipFit.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model representing client's entries to the gym.
    /// </summary>
    public class PassEntry : DbObject
    {
        /// <summary>
        /// Gets or sets EntryId - foreign key.
        /// </summary>
        [ForeignKey("Entry")]
        [Required]
        public Guid EntryId { get; set; }

        /// <summary>
        /// Gets or sets the date of entry.
        /// </summary>
        [DataType(DataType.Date)]
        [Required]
        public DateOnly Date { get; set; }

        [InverseProperty("Entries")]
        public virtual Pass Pass { get; set; }
    }
}