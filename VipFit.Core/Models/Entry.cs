namespace VipFit.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model representing client's entries to the gym.
    /// </summary>
    public class Entry : DbObject
    {
        /// <summary>
        /// Gets or sets the date of entry.
        /// </summary>
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets a value representing position in pass.
        /// The position indicates which entry was consumed from all entries.
        /// </summary>
        [Required]
        public byte PositionInPass { get; set; }

        #region Foreign Keys

        /// <summary>
        /// Gets or sets the associated PassId.
        /// </summary>
        [Required]
        public Guid PassId { get; set; }

        /// <summary>
        /// Gets or sets the associated Pass.
        /// </summary>
        [ForeignKey(nameof(PassId))]
        public Pass Pass { get; set; }

        #endregion
    }
}