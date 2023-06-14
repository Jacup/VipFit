namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;
    using VipFit.Managers;

    /// <summary>
    /// Payment ListViewModel.
    /// </summary>
    public class PaymentListViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private PaymentViewModel? selectedPayment;

        private Pass? pass;
        private bool isLoading = false;
        private bool isAbleToBeSuspended;
        private bool isAbleToBeResumed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentListViewModel"/> class.
        /// </summary>
        /// <param name="client">Client object.</param>
        public PaymentListViewModel(Client client) => Task.Run(() => GetPaymentListForClientAsync(client));

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentListViewModel"/> class.
        /// </summary>
        /// <param name="pass">Pass object.</param>
        public PaymentListViewModel(Pass pass)
        {
            this.pass = pass;
            LoadPayments(pass);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentListViewModel"/> class.
        /// </summary>
        public PaymentListViewModel() => Task.Run(GetPaymentsForPassAsync);

        /// <summary>
        /// Gets collection of Payments View Models.
        /// </summary>
        public ObservableCollection<PaymentViewModel> Payments { get; } = new();

        /// <summary>
        /// Gets or sets a VM of currently selected payment.
        /// </summary>
        public PaymentViewModel? SelectedPayment
        {
            get => selectedPayment;
            set
            {
                SetProperty(ref selectedPayment, value);
                IsAbleToBeSuspended = selectedPayment != null && !selectedPayment.IsSuspended;
                IsAbleToBeResumed = selectedPayment != null && selectedPayment.IsSuspended;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model has been modified.
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether selected pass is able to be suspended.
        /// </summary>
        public bool IsAbleToBeSuspended
        {
            get => isAbleToBeSuspended;
            set => SetProperty(ref isAbleToBeSuspended, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether selected pass is able to be resumed.
        /// </summary>
        public bool IsAbleToBeResumed
        {
            get => isAbleToBeResumed;
            set => SetProperty(ref isAbleToBeResumed, value);
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
        /// Gets the list of payments from database.
        /// </summary>
        /// <returns>List of payments.</returns>
        internal async Task GetPaymentsForPassAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var payments = await App.GetService<IPaymentRepository>().GetForPassAsync(pass.Id);

            await AddPaymentsToObservableCollection(payments);
        }

        /// <summary>
        /// Gets the list of payments for specific client from database.
        /// </summary>
        /// <param name="client">Associated Client.</param>
        /// <returns>List of payments.</returns>
        internal async Task GetPaymentListForClientAsync(Client client)
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var payments = await App.GetService<IPaymentRepository>().GetForClientAsync(client.Id);

            await AddPaymentsToObservableCollection(payments);
        }

        /// <summary>
        /// Gets the list of payments for specific pass from database.
        /// </summary>
        /// <param name="pass">Associated Pass.</param>
        /// <returns>List of payments.</returns>
        internal async Task GetPaymentListForPassAsync(Pass pass)
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var payments = await App.GetService<IPaymentRepository>().GetForPassAsync(pass.Id);

            await AddPaymentsToObservableCollection(payments);
        }

        /// <summary>
        /// Saves any modified payments and reloads the Payments list from the database.
        /// </summary>
        internal async Task Refresh()
        {
            await GetPaymentsForPassAsync();
        }

        /// <summary>
        /// Saves modified data.
        /// </summary>
        internal async Task SaveAsync()
        {
            foreach (var modifiedPayment in Payments.Where(p => p.IsModified))
            {
                await App.GetService<IPaymentRepository>().UpsertAsync(modifiedPayment.Model);
                modifiedPayment.IsModified = false;
            }

            foreach (var p in paymentsToRemove)
                await App.GetService<IPaymentRepository>().DeleteAsync(p.Id);

            paymentsToRemove.Clear();
            IsModified = false;
        }

        /// <summary>
        /// Suspends selected payment.
        /// </summary>
        /// <param name="selectedPayment">Payment to be suspended.</param>
        internal void SuspendPayment(PaymentViewModel selectedPayment)
        {
            if (selectedPayment == null)
                return;

            IsModified = true;

            if (pass == null)
                AddNewPayment(selectedPayment.Model.Pass);
            else
                AddNewPayment(pass);

            selectedPayment.IsSuspended = true;
        }

        /// <summary>
        /// Resumes selected payment.
        /// </summary>
        /// <param name="selectedPayment">Payment to be resumed.</param>
        internal void ResumePayment(PaymentViewModel? selectedPayment)
        {
            if (selectedPayment == null)
                return;

            IsModified = true;

            selectedPayment.IsSuspended = false;

            RemoveLastPayment();
        }

        private async Task AddPaymentsToObservableCollection(IEnumerable<Payment> payments)
        {
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

        private void AddNewPayment(Pass pass)
        {
            Payment newPayment = PaymentManager.CreateNextPayment(Payments.Last().Model, pass);
            Payments.Add(new(newPayment) { IsModified = true });
            IsModified = true;
        }

        private void RemoveLastPayment()
        {
            PaymentViewModel paymentVmToRemove = Payments.Last();

            paymentsToRemove.Add(paymentVmToRemove.Model);
            Payments.Remove(paymentVmToRemove);
            IsModified = true;
        }

        private Collection<Payment> paymentsToRemove = new Collection<Payment>();

        private async void LoadPayments(Pass pass) => await GetPaymentListForPassAsync(pass);
    }
}