namespace VipFit.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a client entity.
    /// </summary>
    public class Client : DbObject, IEquatable<Client>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
        }

        #region ClientDetails

        /// <summary>
        /// Gets or sets client's first name.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets client's last name.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets clien't phone number.
        /// </summary>
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets client's email.
        /// </summary>
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        #endregion

        #region Agreements

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for marketing purposes.
        /// </summary>
        public bool AgreementMarketing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image for promotional purposes.
        /// </summary>
        public bool AgreementPromoImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image on website.
        /// </summary>
        public bool AgreementWebsiteImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image on socials.
        /// </summary>
        public bool AgreementSocialsImage { get; set; }

        #endregion

        #region Miscellaneous

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

        /// <summary>
        /// Gets or sets comment for client.
        /// </summary>
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether client is removed and is in "bin".
        /// </summary>
        public bool Trash { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets collection of Client's Passes.
        /// </summary>
        [InverseProperty("Client")]
        public ICollection<Pass> Passes { get; set; }

        #endregion

        #region Overrides

        /// <inheritdoc/>
        public override string ToString() => $"{FirstName} {LastName}";

        /// <inheritdoc/>
        public bool Equals(Client other) =>
            other != null &&
            Id == other.Id &&
            FirstName == other.FirstName &&
            LastName == other.LastName &&
            Phone == other.Phone &&
            Email == other.Email;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var c = obj as Client;

            return
                Id == c.Id &&
                FirstName == c.FirstName
                && LastName == c.LastName
                && Phone == c.Phone
                && Email == c.Email;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (!Id.Equals(Guid.Empty) ? Id.GetHashCode() : 0);
                hash = (hash * 23) + (FirstName?.GetHashCode() ?? 0);
                hash = (hash * 23) + (LastName?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Phone?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Email?.GetHashCode() ?? 0);
                return hash;
            }
        }

        #endregion

    }
}
