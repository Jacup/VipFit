namespace VipFit.Tests.MSTest.ModelsTests
{
    using VipFit.Core.Models;

    [TestClass]
    public class PassTemplateTests
    {
        [TestMethod]
        public void PassTemplateTest_AllDataProviden_NameShouldBeEqual() => Assert.AreEqual("Standard", GetStandardPassTemplate().Name);

        [TestMethod]
        public void PassTemplateTest_AllDataProviden_EntriesShouldBeEqual() => Assert.AreEqual(4, GetStandardPassTemplate().EntriesPerMonth);

        [TestMethod]
        public void PassTemplateTest_AllDataProviden_MonthsDurationShouldBeEqual() => Assert.AreEqual(3, GetStandardPassTemplate().MonthsDuration);

        [TestMethod]
        public void PassTemplateTest_PassCodeValidation_ShouldCombineNameAndMonthsDurationInSelectedPattern()
        {
            var name = "Standard";
            byte duration = 3;

            var passTemplate = new PassTemplate() { Name = name, EntriesPerMonth = 4, MonthsDuration = duration };

            Assert.AreEqual($"VF-{name}-{duration}M", passTemplate.PassCode);
        }

        [TestMethod]
        public void PassTemplateTest_ToString_PassCodeShouldBeEqualToToString()
        {
            var passTemplate = GetStandardPassTemplate();

            Assert.AreEqual(passTemplate.PassCode, passTemplate.ToString());
        }

        [TestMethod]
        public void PassTemplateTest_PropertiesChanged_PassCodeShouldBeDynamic()
        {
            var passTemplate = GetStandardPassTemplate();

            passTemplate.Name = "NewName";
            Assert.AreEqual(passTemplate.PassCode, passTemplate.ToString());
        }

        private static PassTemplate GetStandardPassTemplate() => new() { Name = "Standard", EntriesPerMonth = 4, MonthsDuration = 3 };

        private static PassTemplate GetProPassTemplate() => new() { Name = "Pro", EntriesPerMonth = 8, MonthsDuration = 1 };
    }
}
