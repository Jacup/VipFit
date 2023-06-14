namespace VipFit.Managers
{
    using System;
    using System.Collections.Generic;
    using VipFit.Core.Models;

    /// <summary>
    /// Class that implements business logic for payments module.
    /// </summary>
    internal static class PaymentManager
    {
        /// <summary>
        /// Creates collection of Payments based on provided pass object.
        /// </summary>
        /// <param name="pass">Pass object.</param>
        /// <returns>Collection of created payments.</returns>
        internal static IEnumerable<Payment> CreatePaymentList(Pass pass)
        {
            byte payments = pass.PassTemplate.MonthsDuration;
            decimal installmentAmount = pass.PassTemplate.PricePerMonth;
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (byte i = 0; i < payments; i++)
            {
                DateTime? paymentDate = i == 0 ? DateTime.Now : null;
                bool paid = i == 0;
                string comment = paid ? "Paid" : string.Empty;

                yield return new Payment()
                {
                    Amount = installmentAmount,
                    DueDate = today.AddMonths(i),
                    PaymentDate = paymentDate,
                    Paid = paid,
                    Comment = comment,
                    Pass = pass,
                    PassId = pass.Id,
                };
            }
        }

        /// <summary>
        /// Creates single payment added as the next (or last) payment of the provided pass.
        /// </summary>
        /// <param name="lastPayment">Base payment.</param>
        /// <param name="pass">Base pass.</param>
        /// <returns>Created payment.</returns>
        internal static Payment CreateNextPayment(Payment lastPayment, Pass pass)
        {
            return new()
            {
                Amount = pass.PassTemplate.PricePerMonth,
                DueDate = lastPayment.DueDate.AddMonths(1),
                Pass = pass,
                PassId = pass.Id,
            };
        }
    }
}
