namespace VipFit.Tests.MSTest.ModelsTests
{
    using VipFit.Core.Models;

    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void Client_ToString_ReturnsFullName()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "Andrzej",
                LastName = "Kowalski",
            };

            // Act
            var result = client.ToString();

            // Assert
            Assert.AreEqual("Andrzej Kowalski", result);
        }

        [TestMethod]
        public void Client_Equals_ReturnsTrueForEqualObjects()
        {
            // Arrange
            Client client1 = GetBasicClient();

            // Act & Assert
            Assert.IsTrue(client1.Equals(client1));
        }

        [TestMethod]
        public void Client_Equals_ReturnsFalseForDifferentObjectsWithTheSameProperties()
        {
            // Arrange
            Client client1 = GetBasicClient("Andrzej", "Kowalski");
            Client client2 = GetBasicClient("Andrzej", "Kowalski");

            // Act & Assert
            Assert.IsFalse(client1.Equals(client2));
        }

        [TestMethod]
        public void Client_Equals_ReturnsFalseForDifferentObjectsWithDifferentProperties()
        {
            // Arrange
            Client client1 = GetBasicClient("Andrzej", "Kowalski");
            Client client2 = GetBasicClient("Marian", "Kowal");

            // Act & Assert
            Assert.IsFalse(client1.Equals(client2));
        }

        [TestMethod]
        public void Client_EqualsObj_ReturnsTrueForEqualObjects()
        {
            // Arrange
            Client client1 = GetBasicClient();

            // Act & Assert
            Assert.IsTrue(client1.Equals((object)client1));
        }

        [TestMethod]
        public void Client_EqualsObj_ReturnsFalseForDifferentObjectsWithTheSameProperties()
        {
            // Arrange
            Client client1 = GetBasicClient("Andrzej", "Kowalski");
            Client client2 = GetBasicClient("Andrzej", "Kowalski");

            // Act & Assert
            Assert.IsFalse(client1.Equals((object)client2));
        }

        [TestMethod]
        public void Client_EqualsObj_ReturnsFalseForDifferentObjectsWithDifferentProperties()
        {
            // Arrange
            Client client1 = GetBasicClient("Andrzej", "Kowalski");
            Client client2 = GetBasicClient("Marian", "Kowal");

            // Act & Assert
            Assert.IsFalse(client1.Equals((object)client2));
        }

        [TestMethod]
        public void Client_GetHashCode_ReturnsDifferentValueForDifferentObjects()
        {
            // Arrange
            var client1 = GetBasicClient();
            var client2 = GetBasicClient();

            // Act & Assert
            Assert.AreNotEqual(client1.GetHashCode(), client2.GetHashCode());
        }

        [TestMethod]
        public void Client_TwoObjectsWithSameData_IdShouldBeDifferent()
        {
            // Arrange
            Client client1 = GetBasicClient();
            Client client2 = GetBasicClient();

            // Act & Assert
            Assert.IsTrue(client1.Id != client2.Id);
        }

        [TestMethod]
        public void IdTest_ObjectCreated_ShouldNotBeNull()
        {
            // Arrange
            Client client = GetBasicClient();

            // Act & Assert
            Assert.IsNotNull(client.Id);
        }

        [TestMethod]
        public void ToString_ValidData_ShouldReturnStringContainingFirstAndLastName()
        {
            // Arrange
            string fname = "Andrzej";
            string lname = "Kowalski";

            string expectedValue = $"{fname} {lname}";
            Client client = GetBasicClient(fname, lname);

            // Act & Assert
            Assert.AreEqual(expectedValue, client.ToString());
        }

        [TestMethod]
        public void Properties_TheSameFirstNameProvided_ShouldBeTheSame()
        {
            // Arrange
            string fname = "Andrzej";
            Client client1 = new() { FirstName = fname, };

            // Act & Assert
            Assert.AreEqual(fname, client1.FirstName);
        }

        [TestMethod]
        public void Properties_TheSameLastNameProvided_ShouldBeTheSame()
        {
            // Arrange
            string lname = "Kowalski";
            Client client1 = new() { LastName = lname, };

            // Act & Assert
            Assert.AreEqual(lname, client1.LastName);
        }

        [TestMethod]
        public void Properties_TheSamePhoneProvided_ShouldBeTheSame()
        {
            // Arrange
            string phone = "123456789";
            Client client1 = new() { Phone = phone, };

            // Act & Assert
            Assert.AreEqual(phone, client1.Phone);
        }

        [TestMethod]
        public void Properties_TheSameEmailProvided_ShouldBeTheSame()
        {
            // Arrange
            string email = "example@example.pl";
            Client client1 = new() { Email = email };

            // Act & Assert
            Assert.AreEqual(email, client1.Email);
        }

        private static Client GetBasicClient() => new() { FirstName = "Andrzej", LastName = "Kowalski", Phone = "123456789", Email = "example@mail.com" };

        private static Client GetBasicClient(string fname, string lname) => new() { FirstName = fname, LastName = lname, Phone = "123456789", Email = "example@mail.com" };
    }
}
