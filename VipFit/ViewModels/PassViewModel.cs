namespace VipFit.ViewModels
{
    using CommunityToolkit.WinUI;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;
    using VipFit.Managers;

    /// <summary>
    /// Pass ViewModel.
    /// </summary>
    public class PassViewModel : BaseViewModel
    {
        private readonly bool isClientReadOnly;

        private Pass model;
        private Client client;
        private PassTemplate passTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassViewModel"/> class.
        /// </summary>
        /// <param name="model">Pass model.</param>
        /// <param name="isNewPass">Indicates whether that is new pass.</param>
        public PassViewModel(Pass? model = null, bool isNewPass = false)
        {
            Model = model ?? new Pass();
            IsNew = isNewPass;
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = null;

            LoadClients();

            if (!isNewPass)
            {
                LoadClient(Model.ClientId);
                LoadPass(Model.PassTemplateId);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PassViewModel"/> class.
        /// </summary>
        /// <param name="client">Client model.</param>
        public PassViewModel(Client client)
        {
            Model = new();

            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = null;

            LoadClients();
            var matchedClientFromList = AvailableClients.FirstOrDefault(c => c.Id == client.Id);

            if (matchedClientFromList == null)
                return;

            Client = matchedClientFromList;
            isClientReadOnly = true;
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
                if (value == client)
                    return;

                client = value;
                Model.ClientId = value.Id;
                IsModified = true;
                OnPropertyChanged();
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
                if (value == passTemplate)
                    return;

                passTemplate = value;
                Model.PassTemplate = value;
                Model.PassTemplateId = value.Id;
                IsModified = true;
                OnPropertyChanged();

                SetDependentOptions();
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

        /// <summary>
        /// Gets or sets a list of entries.
        /// </summary>
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
                if (value == Model.CreatedAt)
                    return;

                Model.CreatedAt = value;
                IsModified = true;
                OnPropertyChanged();
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
                if (value == Model.ModifiedAt)
                    return;

                Model.ModifiedAt = value;
                IsModified = true;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets a value indicating whether client is already set and can't be changed.
        /// </summary>
        public bool IsClientReadOnly => isClientReadOnly;

        /// <summary>
        /// Gets or sets an observable collection of payments.
        /// </summary>
        public ObservableCollection<Payment> Payments { get; set; }

        /// <summary>
        /// Gets collection of clients in the list.
        /// </summary>
        public ObservableCollection<Client> AvailableClients { get; } = new();

        /// <summary>
        /// Gets collection of pass templates in the list.
        /// </summary>
        public ObservableCollection<PassTemplate> AvailablePassTemplates { get; } = new();

        /// <inheritdoc/>
        public override async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            var dateTime = DateTime.Now;

            if (IsNew)
            {
                IsNew = false;
                Model.CreatedAt = dateTime;

                Payments = new(PaymentManager.CreatePaymentList(Model));

                App.GetService<PassListViewModel>().Passes.Add(this);
            }

            Model.ModifiedAt = dateTime;
            await App.GetService<IPassRepository>().UpsertAsync(Model);

            await SavePaymentsAsync();
        }

        /// <inheritdoc/>
        public override async Task CancelEditsAsync()
        {
            if (IsNew)
                AddNewPassCanceled?.Invoke(this, EventArgs.Empty);
            else
                await RevertChangesAsync();
        }

        /// <inheritdoc/>
        public override async Task DeleteAsync()
        {
            if (Model != null)
            {
                IsModified = false;
                App.GetService<PassListViewModel>().Passes.Remove(this);
                await App.GetService<IPassRepository>().DeleteAsync(Model.Id);
            }
        }

        /// <inheritdoc/>
        public override async Task RevertChangesAsync()
        {
            IsInEdit = false;

            if (IsNew)
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
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Obtains list of available pass templates from database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task GetAvailablePassTemplateListAsync()
        {
            await DispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var passTemplates = await App.GetService<IPassTemplateRepository>().GetAsync();

            if (passTemplates == null)
                return;

            await DispatcherQueue.EnqueueAsync(() =>
            {
                AvailablePassTemplates.Clear();

                foreach (var pt in passTemplates)
                    AvailablePassTemplates.Add(pt);

                IsLoading = false;
            });
        }

        private async Task GetAvailableClientListAsync()
        {
            await DispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var clients = await App.GetService<IClientRepository>().GetAsync();

            if (clients == null)
                return;

            await DispatcherQueue.EnqueueAsync(() =>
            {
                AvailableClients.Clear();

                foreach (var c in clients)
                    AvailableClients.Add(c);

                IsLoading = false;
            });
        }

        private async Task RefreshPassAsync() => Model = await App.GetService<IPassRepository>().GetAsync(Model.Id);

        private async Task SavePaymentsAsync()
        {
            foreach (var payment in Payments)
            {
                await App.GetService<IPaymentRepository>().UpsertAsync(payment);
            }
        }

        private async void LoadClients()
        {
            await GetAvailableClientListAsync();
        }

        private async void LoadClient(Guid clientId)
        {
            var c = await App.GetService<IClientRepository>().GetAsync(clientId);

            await DispatcherQueue.EnqueueAsync(() =>
            {
                Client = c;
            });
        }

        private async void LoadPass(Guid passTemplateId)
        {
            var pt = await App.GetService<IPassTemplateRepository>().GetAsync(passTemplateId);

            await DispatcherQueue.EnqueueAsync(() =>
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
