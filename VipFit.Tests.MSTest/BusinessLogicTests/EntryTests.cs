//namespace VipFit.Tests.MSTest.BusinessLogicTests
//{
//    using Microsoft.UI.Dispatching;
//    using Moq;
//    using System;
//    using VipFit.Core.Enums;
//    using VipFit.Core.Models;
//    using VipFit.ViewModels;

//    [TestClass]
//    public class EntryTests
//    {
//        #region Properties Tests

//        [TestMethod]
//        public void PositionInPassTest_PassNotNull_ShouldReturnValidValue()
//        {
//            //// Arrange
//            //var vm = new EntryViewModel();
//            //Pass pass = GetPass(GetClient(), GetPassTemplate());

//            //byte expectedPosition = 0;

//            //// Act
//            //vm.Pass = pass;

//            //// Assert
//            //Assert.AreEqual(expectedPosition, vm.PositionInPass);
//        }

//        #endregion

//        #region Methods Tests

//        [TestMethod]
//        public void CalculatePositionTest_NullPassProvided_ShouldReturnNull()
//        {
//            //// Act
//            //var actualPosition = EntryViewModel.CalculatePosition(null);

//            //// Asssert
//            //Assert.IsNull(actualPosition);
//        }

//        [TestMethod]
//        public void CalculatePositionTest_PassWithoutRecentEntrtiesProvided_ShouldReturnValidValue()
//        {
//            //// Arrange
//            //Pass pass = GetPass(GetClient(), GetPassTemplate());
//            //byte expectedPosition = 0;

//            //// Act
//            //var actualPosition = EntryViewModel.CalculatePosition(pass);

//            //// Asssert
//            //Assert.AreEqual(expectedPosition, actualPosition);
//        }

//        [TestMethod]
//        public void CalculatePositionTest_PassWithOneEntryProvided_ShouldReturnValidValue()
//        {
//            //// Arrange
//            //Pass pass = GetPass(GetClient(), GetPassTemplate());
//            //byte expectedPosition = 2;

//            //// Act
//            //var actualPosition = EntryViewModel.CalculatePosition(pass);

//            //// Asssert
//            //Assert.AreEqual(expectedPosition, actualPosition);
//        }

//        #endregion

//        #region SetUp Methods

//        private static Client GetClient() => new("Andrzej", "Kowalski", "666666666", "email@email.pl");

//        private static PassTemplate GetPassTemplate() => new(PassType.Standard, PassDuration.Short, 500);

//        //private static Pass GetPass(Client client, PassTemplate pt) => new(
//        //    DateOnly.FromDateTime(DateTime.Now),
//        //    DateOnly.FromDateTime(DateTime.Now).AddMonths(pt.MonthsDuration),
//        //    DateTime.Now,
//        //    DateTime.Now,
//        //    client.Id,
//        //    pt.Id);

//        #endregion
//    }
//}
