namespace VipFit.Core.Models
{
    /// <summary>
    /// Represents a client entity.
    /// </summary>
    public class Client : DbObject, IEquatable<Client>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="firstName">Client's first name.</param>
        /// <param name="lastName">Client's last name.</param>
        /// <param name="phone">Client's phone number.</param>
        /// <param name="email">Client's email.</param>
        public Client(string firstName, string lastName, string phone, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
        }

        /// <summary>
        /// Gets or sets client's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets client's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets clien't phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets client's email.
        /// </summary>
        public string Email { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"{FirstName} {LastName}";

        /// <inheritdoc/>
        public bool Equals(Client other) =>
            other != null &&
            FirstName == other.FirstName &&
            LastName == other.LastName &&
            Phone == other.Phone &&
            Email == other.Email;
    }
}
