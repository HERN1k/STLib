namespace STLib.Core.Testing
{
    /// <summary>
    /// Represents the type of attention or alert associated with a message or event.
    /// </summary>
    public enum AttentionType
    {
        /// <summary>
        /// Indicates a warning that requires attention but is not critical.
        /// </summary>
        Warning = 0,
        /// <summary>
        /// Indicates a critical issue that requires immediate attention.
        /// </summary>
        Critical = 2
    }
}