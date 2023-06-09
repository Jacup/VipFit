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
        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Gets or sets the pass's end date.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Gets or sets DateTime of creation.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets last modification date.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Gets a value indicating whether pass is active. Depends on date.
        /// </summary>
        public bool IsActive
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                return today >= StartDate && today < EndDate;
            }
        }

        // PRZETESTOWAC
        public bool IsCurrentMonthPaid
        {
            get
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                for (int i = 0; i < Payments.Count; i++)
                {
                    if (today >= Payments.ElementAt(i).DueDate && today < Payments.ElementAt(i + 1).DueDate)
                        return Payments.ElementAt(i + 1).Paid;
                }

                return false;
            }
        }

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

        /// <summary>
        /// Gets or sets the collection of associated entries.
        /// </summary>
        [InverseProperty("Pass")]
        public IList<Entry> Entries { get; set; }

        /// <summary>
        /// Gets or sets the collection of associated payments.
        /// </summary>
        [InverseProperty("Pass")]
        public IList<Payment> Payments { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"{PassTemplate.PassCode}: {StartDate.ToShortDateString()} -> {EndDate.ToShortDateString()}";
    }
}
