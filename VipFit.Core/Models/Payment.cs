namespace VipFit.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Payment : DbObject
    {
        [Required]
        public double Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DueDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PaymentDate { get; set; }

        [Required]
        public bool Paid { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Comment { get; set; }

        #region Foreign Keys

        [Required]
        public Guid PassId { get; set; }

        [ForeignKey(nameof(PassId))]
        public Pass Pass { get; set; }

        #endregion
    }
}
