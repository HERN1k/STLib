using System;
using System.Text.Json.Serialization;

namespace STLib.Core.Testing
{
    /// <summary>
    /// Represents an attention message with a specific type and creation timestamp.
    /// </summary>
    public readonly struct Attention
    {
        /// <summary>
        /// Gets the attention text.
        /// </summary>
        public string AttentionText
        {
            get => m_attention;
        }
        /// <summary>
        /// Gets the type of attention.
        /// </summary>
        public AttentionType Type
        {
            get => m_type;
        }
        /// <summary>
        /// Gets the date and time when the attention was created.
        /// </summary>
        public DateTime Created
        {
            get => m_created;
        }

        private readonly string m_attention;
        private readonly AttentionType m_type;
        private readonly DateTime m_created;

        /// <summary>
        /// Initializes a new instance of the <see cref="Attention"/> struct.
        /// </summary>
        /// <param name="attention">The attention text.</param>
        /// <param name="type">The type of attention.</param>
        /// <exception cref="ArgumentNullException">Thrown when the attention text is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the attention text exceeds 100 characters.</exception>
        /// <exception cref="ArgumentException">Thrown when the attention text contains "NULL".</exception>
        public Attention(string attention, AttentionType type)
        {
            if (string.IsNullOrWhiteSpace(attention))
            {
                throw new ArgumentNullException(nameof(attention));
            }

            if (attention.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(attention), "Attention cannot exceed 100 characters.");
            }

            if (attention.Contains("NULL"))
            {
                throw new ArgumentException("Attention cannot contain \"NULL\".", nameof(attention));
            }

            m_attention = attention;
            m_type = type;
            m_created = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attention"/> struct with a specific creation date.
        /// </summary>
        /// <param name="attention">The attention text.</param>
        /// <param name="type">The type of attention.</param>
        /// <param name="created">The creation date and time of the attention.</param>
        /// <exception cref="ArgumentNullException">Thrown when the attention text is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the attention text exceeds 100 characters or the created date is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown when the attention text contains "NULL".</exception>
        [JsonConstructor]
#pragma warning disable IDE0051
        private Attention(string attention, AttentionType type, DateTime created)
#pragma warning restore IDE0051
        {
            if (string.IsNullOrWhiteSpace(attention))
            {
                throw new ArgumentNullException(nameof(attention));
            }

            if (attention.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(attention), "Attention cannot exceed 100 characters.");
            }

            if (attention.Contains("NULL"))
            {
                throw new ArgumentException("Attention cannot contain \"NULL\".", nameof(attention));
            }

            if (created.Equals(DateTime.MinValue) || created.Equals(DateTime.MaxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(created), "Created date is invalid.");
            }

            m_attention = attention;
            m_type = type;
            m_created = created;
        }

        /// <summary>
        /// Returns a string representation of the attention message.
        /// </summary>
        /// <returns>A string in the format "[Created] [Type] AttentionText".</returns>
        public override string ToString() => $"[{m_created}] [{m_type}] {m_attention}";
    }
}