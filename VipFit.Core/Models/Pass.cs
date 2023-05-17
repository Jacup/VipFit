namespace VipFit.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

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
        /// Initializes a new instance of the <see cref="Pass"/> class.
        /// </summary>
        /// <param name="startDate">StartDate.</param>
        /// <param name="endDate">EndDate.</param>
        /// <param name="createdAt">Created date.</param>
        /// <param name="modifiedAt">Last modification date.</param>
        /// <param name="clientId">Associated client ID.</param>
        /// <param name="passTemplateId">Associated Pass ID</param>
        /// <param name="entries">Array of associated entries.</param>
        public Pass(
            DateOnly startDate,
            DateOnly endDate,
            DateTime createdAt,
            DateTime modifiedAt,
            Guid clientId,
            Guid passTemplateId,
            Entry[] entries)
        {
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            ClientId = clientId;
            PassTemplateId = passTemplateId;
            Entries = entries;
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

        #region Foreign keys

        /// <summary>
        /// Gets or sets the associated client's ID.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Gets or sets the associated client.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the associated Pass Template's ID.
        /// </summary>
        public Guid PassTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the associated PassTemplate.
        /// </summary>
        public PassTemplate PassTemplate { get; set; }

        /// <summary>
        /// Gets or sets the array of entries.
        /// </summary>
        public Entry[] Entries { get; set; } // Collection navigation containing dependents

        #endregion

        // public IEnumerable<Payment> Payments { get; set; } // Module 3
    }
}
