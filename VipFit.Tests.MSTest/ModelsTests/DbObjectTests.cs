namespace VipFit.Tests.MSTest.ModelsTests
{
    using VipFit.Core.Models;

    [TestClass]
    public class DbObjectTests
    {
        [TestMethod]
        public void DbObject_Id_ShouldBeUnique()
        {
            DbObject obj1 = new();
            DbObject obj2 = new();

            Assert.AreNotEqual(obj1.Id, obj2.Id);
        }

        [TestMethod]
        public void DbObject_Id_ShoulNotBeEmpty()
        {
            DbObject obj1 = new();

            Assert.AreNotEqual(Guid.Empty, obj1.Id);
        }
    }
}
