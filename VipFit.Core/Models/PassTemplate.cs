namespace VipFit.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a model for the pass template.
    /// </summary>
    public class PassTemplate : DbObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassTemplate"/> class.
        /// </summary>
        public PassTemplate()
        {
        }

        /// <summary>
        /// Gets or sets the name of pass.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the duration of pass.
        /// </summary>
        [Required]
        public byte MonthsDuration { get; set; }

        /// <summary>
        /// Gets or sets pass price.
        /// </summary>
        [Required]
        public decimal PricePerMonth { get; set; }

        /// <summary>
        /// Gets or sets the amount of entries per month.
        /// </summary>
        [Required]
        public byte EntriesPerMonth { get; set; }

        /// <summary>
        /// Gets total entries. Based on duration and amount of entries per month.
        /// </summary>
        public int TotalEntries => EntriesPerMonth * MonthsDuration;

        /// <summary>
        /// Gets total entries. Based on duration and amount of entries per month.
        /// </summary>
        public decimal TotalPrice => PricePerMonth * MonthsDuration;

        /// <summary>
        /// Gets code represantation of pass.
        /// </summary>
        public string PassCode => $"VF-{Name}-{MonthsDuration}M";

        /// <inheritdoc/>
        public override string ToString() => PassCode;
    }
}