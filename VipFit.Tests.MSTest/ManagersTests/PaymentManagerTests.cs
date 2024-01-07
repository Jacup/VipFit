namespace VipFit.Tests.MSTest.ManagersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VipFit.Core.Models;
    using VipFit.Managers;

    [TestClass]
    public class PaymentManagerTests
    {
        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_InstallmentAmountShouldBeEqual()
        {
            // Arrange
            Pass pass = GetPass();

            // Act
            var payments = PaymentManager.CreatePaymentList(pass);
            List<decimal> installmentsAmounts = payments.Select(p => p.Amount).ToList();

            // Assert
            Assert.IsTrue(installmentsAmounts.All(x => x == installmentsAmounts[0]));
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_SumOfInstallmentsAmountShouldBeEqualToTotalPriceFromPassTemplate()
        {
            // Arrange
            Pass pass = GetPass();
            var expectedPrice = pass.PassTemplate.Price;

            // Act
            var payments = PaymentManager.CreatePaymentList(pass);
            var actualTotalPrice = payments.Select(x => x.Amount).Sum();

            // Assert
            Assert.AreEqual(expectedPrice, actualTotalPrice);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_InstallmentsCountShouldBeEqualToPassTemplateDuration()
        {
            // Arrange
            Pass pass = GetPass();
            var expectedInstallmentsCount = pass.PassTemplate.MonthsDuration;

            // Act
            var installments = PaymentManager.CreatePaymentList(pass).Count();

            // Assert
            Assert.AreEqual(expectedInstallmentsCount, installments);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_FirstDueDateShouldBeEqualToTodayDate()
        {
            // Arrange
            Pass pass = GetPass();
            DateOnly expectedDueDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var actualDueDate = PaymentManager.CreatePaymentList(pass).ElementAt(0).DueDate;

            // Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_SecondDueDateShouldBeEqualToTodayPlusOneMonthDate()
        {
            // Arrange
            Pass pass = GetPass();
            DateOnly expectedDueDate = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);

            // Act
            var actualDueDate = PaymentManager.CreatePaymentList(pass).ElementAt(1).DueDate;

            // Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_LastDueDateShouldBeEqualToLastMonthOfPass()
        {
            // Arrange
            Pass pass = GetPass();
            DateOnly expectedDueDate = DateOnly.FromDateTime(DateTime.Now).AddMonths(pass.PassTemplate.MonthsDuration - 1);

            // Act
            var actualDueDate = PaymentManager.CreatePaymentList(pass).Last().DueDate;

            // Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_FirstPaymentShouldNotBeNull()
        {
            // Arrange
            Pass pass = GetPass();

            // Act & Assert
            Assert.IsNotNull(PaymentManager.CreatePaymentList(pass).ElementAt(0).PaymentDate);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_AllPaymentDatesAreNullExceptFirst()
        {
            // Arrange
            Pass pass = GetPass();

            var paymentDates = PaymentManager.CreatePaymentList(pass).Select(x => x.PaymentDate).ToList();
            paymentDates.RemoveAt(0);

            // Act & Assert
            Assert.IsTrue(paymentDates.All(x => x == null));
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_FirstPaymentBePaid()
        {
            // Arrange
            Pass pass = GetPass();

            // Act & Assert
            Assert.IsTrue(PaymentManager.CreatePaymentList(pass).ElementAt(0).Paid);
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_AllPaymentsExceptFirstAreNotPaid()
        {
            // Arrange
            Pass pass = GetPass();

            var paymentPaid = PaymentManager.CreatePaymentList(pass).Select(x => x.Paid).ToList();
            paymentPaid.RemoveAt(0);

            // Act & Assert
            Assert.IsTrue(paymentPaid.All(x => x == false));
        }

        [TestMethod]
        public void CreatePaymentListTest_ValidPassProvided_PassPropertyIsNotNull()
        {
            // Arrange
            Pass pass = GetPass();

            var passes = PaymentManager.CreatePaymentList(pass).Select(x => x.Pass).ToList();

            // Act & Assert
            Assert.IsTrue(passes.All(x => x != null));
        }

        #region SetUp Methods

        private static PassTemplate GetPassTemplate() => new(PassType.Standard, PassDuration.Short, 1500m);

        private static Pass GetPass() => new()
        {
            PassTemplate = GetPassTemplate(),
        };

        #endregion
    }
}
