namespace VipFit.Core.Models
{
    /// <summary>
    /// Represents a client entity.
    /// </summary>
    internal class Client : DbObject, IEquatable<Client>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";

        public bool Equals(Client other) =>
            other != null &&
            FirstName == other.FirstName &&
            LastName == other.LastName &&
            Phone == other.Phone &&
            Email == other.Email;
    }
}
