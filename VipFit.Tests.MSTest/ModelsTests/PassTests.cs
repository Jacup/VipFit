namespace VipFit.Tests.MSTest.ModelsTests
{
    using Moq;
    using System;
    using VipFit.Core.Models;

    [TestClass]
    public class PassTests
    {
        [TestMethod]
        public void Pass_Equals_ReturnsTrueTheSameObjects()
        {
            // Arrange
            var pass = GetPass();

            // Act & Assert
            Assert.AreEqual(pass, pass);
        }

        [TestMethod]
        public void Pass_Equals_ReturnsFalseForDifferentObjects()
        {
            // Arrange
            var pass1 = GetPass();
            var pass2 = GetPass();

            // Act & Assert
            Assert.AreNotEqual(pass1, pass2);
        }

        [TestMethod]
        public void Pass_GetHashCode_AreNotEqualForDifferentObjects()
        {
            // Arrange
            var pass1 = GetPass();
            var pass2 = GetPass();

            // Act & Assert
            Assert.AreNotEqual(pass1.GetHashCode(), pass2.GetHashCode());
        }

        [TestMethod]
        public void Pass_ToString_ReturnsFormattedString()
        {
            // Arrange
            var startDate = new DateOnly(2022, 1, 15);
            var endDate = new DateOnly(2022, 3, 15);

            var passTemplate = GetStandardPassTemplate();

            var pass = new Pass
            {
                StartDate = startDate,
                EndDate = endDate,
                PassTemplate = passTemplate,
            };

            // Act
            var result = pass.ToString();

            // Assert
            Assert.AreEqual($"{passTemplate}: {startDate.ToShortDateString()} -> {endDate.ToShortDateString()}", result);
        }

        private static Pass GetPass() => new()
        {
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2022, 2, 1),
            ClientId = Guid.NewGuid(),
            PassTemplateId = Guid.NewGuid(),
        };

        private static PassTemplate GetStandardPassTemplate() => new() { Name = "Standard", EntriesPerMonth = 4, MonthsDuration = 3 };
    }
}
