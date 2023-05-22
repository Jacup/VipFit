namespace VipFit.Managers
{
    using System;
    using System.Collections.Generic;
    using VipFit.Core.Models;

    internal static class PaymentManager
    {
        internal static IEnumerable<Payment> CreatePaymentList(Pass pass)
        {
            byte installments = pass.PassTemplate.MonthsDuration;
            decimal amount = pass.PassTemplate.Price;
            decimal installmentAmount = amount / installments;
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (byte i = 0; i < installments; i++)
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


    }
}
