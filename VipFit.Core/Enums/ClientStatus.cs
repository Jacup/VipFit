namespace VipFit.Core.Enums
{
    /// <summary>
    /// Represents client's status.
    /// </summary>
    [Flags]
    public enum ClientStatus
    {
        /// <summary>
        /// Unknown state.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Inactive state.
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// Active state.
        /// </summary>
        Active = 2,
    }
}
