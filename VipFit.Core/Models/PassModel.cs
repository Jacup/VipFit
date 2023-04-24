namespace VipFit.Core.Models
{
    using System;
    using VipFit.Core.Enums;

    /// <summary>
    /// Represents a model for a pass.
    /// </summary>
    public class PassModel : DbObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassModel"/> class.
        /// </summary>
        /// <param name="type">Pass type.</param>
        /// <param name="duration">Pass duration.</param>
        /// <param name="price">Price of pass.</param>
        public PassModel(PassType type, PassDuration duration, decimal price)
        {
            Type = type;
            Duration = duration;
            Price = price;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PassModel"/> class.
        /// </summary>
        public PassModel()
        {
        }

        /// <summary>
        /// Gets or sets the type of pass.
        /// </summary>
        public PassType Type { get; set; }

        /// <summary>
        /// Gets or sets the duration of pass. Affects <see cref="EndDate"/>.
        /// </summary>
        public PassDuration Duration { get; set; }

        /// <summary>
        /// Gets or sets pass price.
        /// </summary>
        public decimal Price { get; set; }

        #region ReadOnly Values

        /// <summary>
        /// Gets pass duration in months based on Duration and Type of pass.
        /// </summary>
        public byte MonthsDuration => GetMonths(Type, Duration);

        /// <summary>
        /// Gets amount of entries to the gym.
        /// </summary>
        public byte Entries => GetTotalEntries(MonthsDuration, Type);

        /// <summary>
        /// Gets code represantation of pass.
        /// </summary>
        public string PassCode => $"VF-{MonthsDuration}M-{Type}";

        #endregion

        /// <inheritdoc/>
        public override string ToString() => PassCode;

        internal static byte GetMonths(PassType type, PassDuration duration) => duration switch
        {
            PassDuration.Short => (byte)(type == PassType.Standard ? 3 : 1),
            PassDuration.Medium => (byte)(type == PassType.Standard ? 6 : 3),
            PassDuration.Long => (byte)(type == PassType.Standard ? 12 : 10),
            _ => throw new NotImplementedException(),
        };

        internal static byte GetTotalEntries(byte months, PassType type) => type switch
        {
            PassType.Standard => (byte)(months * 4),
            PassType.Pro => (byte)(months * 8),
            _ => throw new NotImplementedException(),
        };
    }
}