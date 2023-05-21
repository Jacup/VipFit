namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Dispatching;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Payment ViewModel.
    /// </summary>
    public class PaymentViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private bool isLoading;

        private Payment model;

        public PaymentViewModel(Payment? model = null)
        {
            Model = model ?? new Payment();
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the payment data.
        /// </summary>
        public event EventHandler AddNewPaymentCanceled;

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model has been modified.
        /// </summary>
        /// <remarks>
        /// Used when syncing with the server to reduce load and only upload the models that have changed.
        /// </remarks>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model is being loaded. Used to show a progress bar.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public Payment Model
        {
            get => model;
            set
            {
                if (model == value)
                    return;

                model = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public DateOnly DueDate
        {
            get => Model.DueDate;
            set
            {
                if (Model.DueDate == value)
                    return;

                Model.DueDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? PaymentDate
        {
            get => Model.PaymentDate;
            set
            {
                if (Model.PaymentDate == value)
                    return;

                Model.PaymentDate = value;
                OnPropertyChanged();
            }
        }

        public bool Paid
        {
            get => Model.Paid;
            set
            {
                if (Model.Paid == value)
                    return;

                Model.Paid = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => Model.Comment;
            set
            {
                if (Model.Comment == value)
                    return;

                Model.Comment = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Saves modified data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveAsync()
        {
            IsModified = false;

            App.GetService<PaymentListViewModel>().Payments.Add(this);
            await App.GetService<IPaymentRepository>().UpsertAsync(Model);
        }

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CancelEditsAsync() => await RevertChangesAsync();

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RevertChangesAsync()
        {
            if (!IsModified)
                return;

            await RefreshPaymentAsync();
            IsModified = false;
        }

        /// <summary>
        /// Reloads all of the payment data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshPaymentAsync() =>
            Model = await App.GetService<IPaymentRepository>().GetAsync(Model.Id);

        /// <summary>
        /// Called when a bound DataGrid control causes the payment to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a payment.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a payment.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
    }
}
