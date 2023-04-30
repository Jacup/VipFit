namespace VipFit.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents pass entity.
    /// </summary>
    internal class Pass : DbObject
    {
        /// <summary>
        /// Gets or sets pass template.
        /// </summary>
        public PassTemplate PassTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pass is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a collection of entries to the gym.
        /// </summary>
        [DataType(DataType.Date)]
        public IEnumerable<DateOnly> Entries { get; set; }

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

        // public IEnumerable<Payment> Payments { get; set; } // Module3
    }
}
