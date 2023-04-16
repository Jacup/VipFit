namespace VipFit.Core.Models
{
    /// <summary>
    /// Represents a client entity.
    /// </summary>
    public class Client : DbObject, IEquatable<Client>
    {
        public Client(string firstName, string lastName, string phone, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        public Client()
        {
        }

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
