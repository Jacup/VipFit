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

        public Pass(bool isActive, DateOnly startDate, DateOnly endDate, DateTime createdAt, DateTime modifiedAt, Guid clientId, Client client)
        {
            IsActive = isActive;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            ClientId = clientId;
            Client = client;
        }

        /// <summary>
        /// Gets or sets a value indicating whether pass is active.
        /// </summary>
        public bool IsActive { get; set; }

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

        #region Foreign keys

        /// <summary>
        /// Gets or sets the associated client's ID.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Gets or sets the associated Client.
        /// </summary>
        public Client Client { get; set; } = null!;

        #endregion

        /// <summary>
        /// Gets or sets pass template.
        /// </summary>
        //public PassTemplate PassTemplate { get; set; }

        ///// <summary>
        ///// Gets or sets a collection of entries to the gym.
        ///// </summary>
        //[InverseProperty("Pass")]
        //public virtual ICollection<PassEntry> Entries { get; set; }

        // public IEnumerable<Payment> Payments { get; set; } // Module 3
    }
}
