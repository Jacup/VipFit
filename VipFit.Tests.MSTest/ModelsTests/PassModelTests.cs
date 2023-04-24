namespace VipFit.Tests.MSTest.ModelsTests
{
    using VipFit.Core.Enums;
    using VipFit.Core.Models;

    [TestClass]
    public class PassModelTests
    {
        #region GetTotalEntries

        [TestMethod]
        public void GetTotalEntries_StandardPass3m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            byte months = 3;

            byte expectedEntries = 12;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        [TestMethod]
        public void GetTotalEntries_StandardPass6m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            byte months = 6;

            byte expectedEntries = 24;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        [TestMethod]
        public void GetTotalEntries_StandardPass12m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            byte months = 12;

            byte expectedEntries = 48;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        [TestMethod]
        public void GetTotalEntries_ProPass1m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            byte months = 1;

            byte expectedEntries = 8;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        [TestMethod]
        public void GetTotalEntries_ProPass3m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            byte months = 3;

            byte expectedEntries = 24;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        [TestMethod]
        public void GetTotalEntries_ProPass10m_ShouldReturnValidValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            byte months = 10;

            byte expectedEntries = 80;

            // Act
            byte actualEntries = PassModel.GetTotalEntries(months, type);

            // Assert
            Assert.AreEqual(expectedEntries, actualEntries);
        }

        #endregion

        #region GetMonths

        [TestMethod]
        public void GetMonth_StandardShort_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            PassDuration duration = PassDuration.Short;

            byte expectedMonths = 3;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        [TestMethod]
        public void GetMonth_StandardMedium_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            PassDuration duration = PassDuration.Medium;

            byte expectedMonths = 6;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        [TestMethod]
        public void GetMonth_StandardLong_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Standard;
            PassDuration duration = PassDuration.Long;

            byte expectedMonths = 12;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        [TestMethod]
        public void GetMonth_ProShort_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            PassDuration duration = PassDuration.Short;

            byte expectedMonths = 1;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        [TestMethod]
        public void GetMonth_ProMedium_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            PassDuration duration = PassDuration.Medium;

            byte expectedMonths = 3;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        [TestMethod]
        public void GetMonth_ProLong_ShouldMatchExpectedValue()
        {
            // Arrange
            PassType type = PassType.Pro;
            PassDuration duration = PassDuration.Long;

            byte expectedMonths = 10;

            // Act
            byte months = PassModel.GetMonths(type, duration);

            // Assert
            Assert.AreEqual(expectedMonths, months);
        }

        #endregion

        #region ToString Overrides

        [TestMethod]
        public void ToString_3mPro_ShouldReturnValidString()
        {
            // Arrange
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);
            string expectedString = "VF-3M-Standard";

            // Act
            var result = model.ToString();

            // Assert
            Assert.AreEqual(expectedString, result);
        }

        [TestMethod]
        public void ToString_10mPro_ShouldReturnValidString()
        {
            // Arrange
            var model = new PassModel(PassType.Pro, PassDuration.Long, 500m);
            string expectedString = "VF-10M-Pro";

            // Act
            var result = model.ToString();

            // Assert
            Assert.AreEqual(expectedString, result);
        }

        #endregion

        #region Model

        [TestMethod]
        public void PassModel_StandardShort_TypeShouldMatch()
        {
            // Arrange
            PassType expectedType = PassType.Standard;
            PassDuration expectedDuration = PassDuration.Short;

            // Act
            var model = new PassModel(expectedType, expectedDuration, 500m);

            // Assert
            Assert.AreEqual(expectedType, model.Type);
        }

        [TestMethod]
        public void PassModel_StandardShort_DurationShouldMatch()
        {
            // Arrange
            PassType expectedType = PassType.Standard;
            PassDuration expectedDuration = PassDuration.Short;

            // Act
            var model = new PassModel(expectedType, expectedDuration, 500m);

            // Assert
            Assert.AreEqual(expectedDuration, model.Duration);
        }

        [TestMethod]
        public void PassModel_StandardShort_StringShouldMatch()
        {
            // Arrange
            string expectedString = "VF-3M-Standard";

            // Act
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);

            // Assert
            Assert.AreEqual(expectedString, model.ToString());
        }

        [TestMethod]
        public void PassModel_StandardShort_PassCodeShouldMatch()
        {
            // Arrange
            string expectedPassCode = "VF-3M-Standard";

            // Act
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);

            // Assert
            Assert.AreEqual(expectedPassCode, model.PassCode);
        }

        [TestMethod]
        public void PassModel_StandardShort_PassCodeShouldBeEqualWithToString()
        {
            // Act
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);

            // Assert
            Assert.AreEqual(model.ToString(), model.PassCode);
        }

        [TestMethod]
        public void PassModel_StandardShort_MonthsShouldMatch()
        {
            // Arrange
            byte expectedMonths = 3;

            // Act
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);

            // Assert
            Assert.AreEqual(expectedMonths, model.MonthsDuration);
        }

        [TestMethod]
        public void PassModel_StandardShort_EntriesShouldMatch()
        {
            // Arrange
            byte expectedEntries = 12;

            // Act
            var model = new PassModel(PassType.Standard, PassDuration.Short, 500m);

            // Assert
            Assert.AreEqual(expectedEntries, model.Entries);
        }

        #endregion
    }
}
