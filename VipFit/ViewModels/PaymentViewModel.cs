namespace VipFit.ViewModels
{
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;
    using Windows.ApplicationModel.Resources;

    /// <summary>
    /// Payment ViewModel.
    /// </summary>
    public class PaymentViewModel : BaseViewModel
    {
        private Payment model;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentViewModel"/> class.
        /// </summary>
        /// <param name="model">Payment model.</param>
        public PaymentViewModel(Payment model) => Model = model ?? new();

        /// <summary>
        /// Gets or sets the underlying Payment object.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Payment DueDate.
        /// </summary>
        public DateOnly DueDate
        {
            get => Model.DueDate;
            set
            {
                if (Model.DueDate == value)
                    return;

                IsModified = true;
                Model.DueDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current Payment date.
        /// </summary>
        public DateTime? PaymentDate
        {
            get => Model.PaymentDate;
            set
            {
                if (Model.PaymentDate == value)
                    return;

                IsModified = true;
                Model.PaymentDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the payment is done. Affects Comment and Payment date.
        /// </summary>
        public bool Paid
        {
            get => Model.Paid;
            set
            {
                if (Model.Paid == value)
                    return;

                IsModified = true;
                Model.Paid = value;
                OnPropertyChanged();

                if (!value)
                {
                    PaymentDate = null;
                    Comment = string.Empty;
                    return;
                }

                PaymentDate = DateTime.Now;

                var resourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
                Comment = resourceLoader == null ? "Paid" : resourceLoader.GetString("Paid");
            }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment
        {
            get => Model.Comment;
            set
            {
                if (Model.Comment == value)
                    return;

                IsModified = true;
                Model.Comment = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of payment.
        /// </summary>
        public decimal Amount
        {
            get => Model.Amount;
            set
            {
                if (Model.Amount == value)
                    return;

                IsModified = true;
                Model.Amount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the pass is suspended.
        /// </summary>
        public bool IsSuspended
        {
            get => Model.IsSuspended;
            set
            {
                if (Model.IsSuspended == value)
                    return;

                IsModified = true;
                Model.IsSuspended = value;
                OnPropertyChanged();

                if (value)
                    SuspendPayment();
                else
                    ResumePass();
            }
        }

        /// <inheritdoc/>
        public async override Task SaveAsync()
        {
            IsModified = false;

            App.GetService<PaymentListViewModel>().Payments.Add(this);
            await App.GetService<IPaymentRepository>().UpsertAsync(Model);
        }

        /// <inheritdoc/>
        public override Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async override Task CancelEditsAsync()
        {
            await RevertChangesAsync();
        }

        /// <inheritdoc/>
        public async override Task RevertChangesAsync()
        {
            if (!IsModified)
                return;

            await RefreshPaymentAsync();
            IsModified = false;
        }

        /// <summary>
        /// Called when a bound DataGrid control causes the payment to enter edit mode.
        /// </summary>
        internal void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a payment.
        /// </summary>
        internal async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a payment.
        /// </summary>
        internal async void EndEdit() => await SaveAsync();

        private void SuspendPayment()
        {
            PaymentDate = null;
            Paid = false;
            Amount = 0;
            var resourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            Comment = resourceLoader == null ? "Suspended" : resourceLoader.GetString("Suspended");
        }

        private async void ResumePass()
        {
            if (Model.Pass == null)
                Model.Pass = await GetPassForPaymentAsync(Model);

            Amount = Model.Pass.PassTemplate.PricePerMonth;

            var resourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            string suspendString = resourceLoader == null ? "Suspended" : resourceLoader.GetString("Suspended");
            Comment = Comment.Replace(suspendString, string.Empty);
        }

        private async Task<Pass> GetPassForPaymentAsync(Payment model) => await App.GetService<IPassRepository>().GetAsync(Model.PassId);

        private async Task RefreshPaymentAsync() =>
            Model = await App.GetService<IPaymentRepository>().GetAsync(Model.Id);
    }
}
