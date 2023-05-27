namespace VipFit.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents pass entity.
    /// </summary>
    public class Pass : DbObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pass"/> class.
        /// </summary>
        public Pass()
        {
        }

        /// <summary>
        /// Gets or sets the pass's start date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; } // default = today, possible in future, but not past.

        /// <summary>
        /// Gets or sets the pass's end date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; } // automatically, depends on Pass duration.

        /// <summary>
        /// Gets or sets DateTime of creation.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets last modification date.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }

        #region Relationships

        /// <summary>
        /// Gets or sets the associated client's ID.
        /// </summary>
        [Required]
        public Guid ClientId { get; set; }

        /// <summary>
        /// Gets or sets the associated client.
        /// </summary>
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the associated Pass Template's ID.
        /// </summary>
        [Required]
        public Guid PassTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the associated PassTemplate.
        /// </summary>
        [ForeignKey(nameof(PassTemplateId))]
        public PassTemplate PassTemplate { get; set; }

        [InverseProperty("Pass")]
        public IList<Entry> Entries { get; set; }

        [InverseProperty("Pass")]
        public IList<Payment> Payments { get; set; }

        #endregion

        #region Overrides

        public override string ToString() => $"{PassTemplate.PassCode}: {StartDate.ToShortDateString()} -> {EndDate.ToShortDateString()}";

        #endregion

        // public IEnumerable<Payment> Payments { get; set; } // Module 3
    }
}
