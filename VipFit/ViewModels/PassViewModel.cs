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
    /// Pass ViewModel.
    /// </summary>
    public class PassViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private Pass model;
        private Client client;
        private PassTemplate passTemplate;

        private bool isLoading;
        private bool isNewPass;
        private bool isInEdit = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassViewModel"/> class.
        /// </summary>
        /// <param name="model">Pass model.</param>
        public PassViewModel(Pass? model = null)
        {
            Model = model ?? new Pass();

            if (!isNewPass)
            {
                LoadClient(Model.ClientId);
                LoadPass(Model.PassTemplateId);
            }

            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = null;
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the Pass data.
        /// </summary>
        public event EventHandler AddNewPassCanceled;

        /// <summary>
        /// Gets or sets the underlying Pass object.
        /// </summary>
        public Pass Model
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

        #region Model's Properties

        /// <summary>
        /// Gets or sets the selected client.
        /// </summary>
        public Client Client
        {
            get => client;
            set
            {
                if (value != client)
                {
                    client = value;
                    Model.ClientId = value.Id;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected client.
        /// </summary>
        public PassTemplate PassTemplate
        {
            get => passTemplate;
            set
            {
                if (value != passTemplate)
                {
                    passTemplate = value;
                    model.PassTemplate = value;
                    Model.PassTemplateId = value.Id;
                    IsModified = true;
                    OnPropertyChanged();

                    SetDependentOptions();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Pass starting date.
        /// </summary>
        public DateOnly? StartDate
        {
            get => Model.StartDate;
            set
            {
                // TODO: Validation if set in the past. some dialog would be nice.

                if (value is not null && value != Model.StartDate)
                {
                    Model.StartDate = (DateOnly)value;

                    IsModified = true;
                    OnPropertyChanged();

                    var date = CalculatePassDuration();
                    if (date != null)
                        EndDate = (DateOnly)date;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Pass ending date.
        /// </summary>
        public DateOnly? EndDate
        {
            get
            {
                if (PassTemplate == null || StartDate == null)
                    return null;

                return Model.EndDate;
            }

            set
            {
                if (value is not null && value != Model.EndDate)
                {
                    Model.EndDate = (DateOnly)value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public IList<Entry> Entries
        {
            get => Model.Entries;
            set
            {
                if (Model.Entries == value)
                    return;

                IsModified = true;
                Model.Entries = value;
                OnPropertyChanged();
            }
        }

        #region Miscellaneous Model's Properties

        /// <summary>
        /// Gets or sets the Pass sold/created date.
        /// </summary>
        public DateTime CreatedAt
        {
            get => Model.CreatedAt;
            set
            {
                if (value != Model.CreatedAt)
                {
                    Model.CreatedAt = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Pass last modfification date.
        /// </summary>
        public DateTime ModifiedAt
        {
            get => Model.ModifiedAt;
            set
            {
                if (value != Model.ModifiedAt)
                {
                    Model.ModifiedAt = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Payment> Payments { get; set; }

        #endregion

        #endregion

        #region Miscellaneous Properties

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model has been modified.
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model is being loaded. Used to show a progress bar.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is new pass object.
        /// </summary>
        public bool IsNewPass
        {
            get => isNewPass;
            set => SetProperty(ref isNewPass, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the pass data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => isInEdit;
            set => SetProperty(ref isInEdit, value);
        }

        #endregion

        /// <summary>
        /// Gets collection of clients in the list.
        /// </summary>
        public ObservableCollection<Client> AvailableClients { get; } = new();

        /// <summary>
        /// Gets collection of pass templates in the list.
        /// </summary>
        public ObservableCollection<PassTemplate> AvailablePassTemplates { get; } = new();

        /// <summary>
        /// Insert new Pass (if new) and save changes to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveAsync()
        {
            isInEdit = false;
            IsModified = false;
            var dateTime = DateTime.Now;

            if (IsNewPass)
            {
                IsNewPass = false;
                Model.CreatedAt = dateTime;

                Payments = new(PaymentManager.CreatePaymentList(Model));

                App.GetService<PassListViewModel>().Passes.Add(this);
            }

            Model.ModifiedAt = dateTime;
            await App.GetService<IPassRepository>().UpsertAsync(Model);

            await SavePaymentsAsync();
        }

        public async Task SavePaymentsAsync()
        {
            foreach (var payment in Payments)
            {
                await App.GetService<IPaymentRepository>().UpsertAsync(payment);
            }
        }

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CancelEditsAsync()
        {
            if (IsNewPass)
                AddNewPassCanceled?.Invoke(this, EventArgs.Empty);
            else
                await RevertChangesAsync();
        }

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RevertChangesAsync()
        {
            IsInEdit = false;

            if (IsNewPass)
            {
                IsModified = false;
                return;
            }

            if (IsModified)
            {
                await RefreshPassAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Reloads all of the pass data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshPassAsync() => Model = await App.GetService<IPassRepository>().GetAsync(Model.Id);

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        internal async Task GetAvailableClientListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var clients = await App.GetService<IClientRepository>().GetAsync();

            if (clients == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                AvailableClients.Clear();

                foreach (var c in clients)
                    AvailableClients.Add(c);

                IsLoading = false;
            });
        }

        internal async Task GetAvailablePassTemplateListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var passTemplates = await App.GetService<IPassTemplateRepository>().GetAsync();

            if (passTemplates == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                AvailablePassTemplates.Clear();

                foreach (var pt in passTemplates)
                    AvailablePassTemplates.Add(pt);

                IsLoading = false;
            });
        }

        private async void LoadClient(Guid clientId)
        {
            var c = await App.GetService<IClientRepository>().GetAsync(clientId);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Client = c;
            });
        }

        private async void LoadPass(Guid passTemplateId)
        {
            var pt = await App.GetService<IPassTemplateRepository>().GetAsync(passTemplateId);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                PassTemplate = pt;
            });
        }

        private DateOnly? CalculatePassDuration()
        {
            if (PassTemplate == null || StartDate == null)
                return null;

            var date = (DateOnly)StartDate;
            return date.AddMonths(PassTemplate.MonthsDuration);
        }

        private void SetDependentOptions()
        {
            SetPassDuration();
            SetEntriesArray();
        }

        private void SetEntriesArray()
        {
            Entries = new List<Entry>();
        }

        private void SetPassDuration()
        {
            var date = CalculatePassDuration();
            if (date != null)
                EndDate = (DateOnly)date;
        }
    }
}
