namespace VipFit.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a model for the payment.
    /// </summary>
    public class Payment : DbObject
    {
        /// <summary>
        /// Gets or sets the payment amount.
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the payment DueDate.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DueDate { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the payment is already paid.
        /// </summary>
        [Required]
        public bool Paid { get; set; }

        /// <summary>
        /// Gets or sets the payment comment.
        /// </summary>
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether payment(pass) is suspended.
        /// </summary>
        [Required]
        public bool IsSuspended { get; set; }

        #region Foreign Keys

        /// <summary>
        /// Gets or sets the associated PassId.
        /// </summary>
        [Required]
        public Guid PassId { get; set; }

        /// <summary>
        /// Gets or sets the associated PassId.
        /// </summary>
        [ForeignKey(nameof(PassId))]
        public Pass Pass { get; set; }

        #endregion
    }
}
