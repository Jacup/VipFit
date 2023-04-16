using VipFit.Core.Models;

namespace VipFit.Tests.MSTest.ModelsTests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void Equals_TwoObjectsWithSameData_ShouldReturnTrue()
        {
            // Arrange
            Client client1 = GetBasicClient();
            Client client2 = GetBasicClient();

            // Act & Assert
            Assert.IsTrue(client1.Equals(client2));
        }

        [TestMethod]
        public void IdTest_TwoObjectsWithSameData_IdShouldBeDifferent()
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
            Client client = new(fname, lname, "123456789", "example@example.pl");

            // Act & Assert
            Assert.AreEqual(expectedValue, client.ToString());
        }

        [TestMethod]
        public void Properties_TheSameFirstNameProvided_ShouldBeTheSame()
        {
            // Arrange
            string fname = "Andrzej";
            string lname = "Kowalski";
            string phone = "123456789";
            string email = "example@example.pl";

            Client client1 = new(fname, lname, phone, email);

            // Act & Assert
            Assert.AreEqual(fname, client1.FirstName);
        }

        [TestMethod]
        public void Properties_TheSameLastNameProvided_ShouldBeTheSame()
        {
            // Arrange
            string fname = "Andrzej";
            string lname = "Kowalski";
            string phone = "123456789";
            string email = "example@example.pl";

            Client client1 = new(fname, lname, phone, email);

            // Act & Assert
            Assert.AreEqual(lname, client1.LastName);
        }
        [TestMethod]
        public void Properties_TheSamePhoneProvided_ShouldBeTheSame()
        {
            // Arrange
            string fname = "Andrzej";
            string lname = "Kowalski";
            string phone = "123456789";
            string email = "example@example.pl";

            Client client1 = new(fname, lname, phone, email);

            // Act & Assert
            Assert.AreEqual(phone, client1.Phone);
        }
        [TestMethod]
        public void Properties_TheSameEmailProvided_ShouldBeTheSame()
        {
            // Arrange
            string fname = "Andrzej";
            string lname = "Kowalski";
            string phone = "123456789";
            string email = "example@example.pl";

            Client client1 = new(fname, lname, phone, email);

            // Act & Assert
            Assert.AreEqual(email, client1.Email);
        }


        private static Client GetBasicClient() => new("Andrzej", "Kowalski", "123456789", "example@example.pl");
    }
}
