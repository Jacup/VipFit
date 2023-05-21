namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;

    /// <summary>
    /// Payment ListViewModel.
    /// </summary>
    public class PaymentListViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private PaymentViewModel? selectedPayment;

        private bool isLoading = false;

        public PaymentListViewModel() => Task.Run(GetPaymentListAsync);

        /// <summary>
        /// Gets Collection of Payments View Models.
        /// </summary>
        public ObservableCollection<PaymentViewModel> Payments { get; } = new();

        public PaymentViewModel? SelectedPayment
        {
            get => selectedPayment;
            set => SetProperty(ref selectedPayment, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the payments list is currently being updated.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Saves any modified payments and reloads the Payments list from the database.
        /// </summary>
        public void Refresh()
        {
            Task.Run(async () =>
            {
                foreach (var modifiedPayment in Payments.Where(p => p.IsModified).Select(p => p.Model))
                    await App.GetService<IPaymentRepository>().UpsertAsync(modifiedPayment);

                IsLoading = true;
                await GetPaymentListAsync();
                isLoading = false;
            });
        }

        /// <summary>
        /// Gets the list of payments from database.
        /// </summary>
        /// <returns>List of payments.</returns>
        public async Task GetPaymentListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var payments = await App.GetService<IPaymentRepository>().GetAsync();

            if (payments == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Payments.Clear();

                foreach (var p in payments)
                    Payments.Add(new PaymentViewModel(p));

                IsLoading = false;
            });
        }
    }
}