namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml.Media.Animation;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer;
    using VipFit.Core.Models;

    /// <summary>
    /// Pass ViewModel.
    /// </summary>
    public class PassViewModel : ObservableRecipient
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private Pass model;
        private Client client;

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

            //if (Model.Client != null)
            //    return;

            //Task.Run(() => LoadClient(Model.ClientId));
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

                RefreshAvailableClientList();
                RefreshAvailablePassTemplateList();

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
        /// Gets or sets the Pass starting date.
        /// </summary>
        public DateOnly StartDate
        {
            get => Model.StartDate;
            set
            {
                if (value != Model.StartDate)
                {
                    Model.StartDate = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Pass ending date.
        /// </summary>
        public DateOnly EndDate
        {
            get => Model.EndDate;
            set
            {
                if (value != Model.EndDate)
                {
                    Model.EndDate = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pass is active.
        /// </summary>
        public new bool IsActive
        {
            get => Model.IsActive;
            set
            {
                if (value != Model.IsActive)
                {
                    Model.IsActive = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StartDate));
                }
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
        /// Refresh available client list.
        /// </summary>
        public void RefreshAvailableClientList() => Task.Run(GetAvailableClientListAsync);

        /// <summary>
        /// Refresh available pass template list.
        /// </summary>
        public void RefreshAvailablePassTemplateList() => Task.Run(GetPassTemplateListAsync);

        /// <summary>
        /// Insert new Pass (if new) and save changes to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task SaveAsync()
        {
            isInEdit = false;
            IsModified = false;
            var dateTime = DateTime.Now;

            if (IsNewPass)
            {
                IsNewPass = false;
                Model.CreatedAt = dateTime;
                App.GetService<PassListViewModel>().Passes.Add(this);
            }

            Model.ModifiedAt = dateTime;
            await App.GetService<IPassRepository>().UpsertAsync(Model);
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

        private async void LoadClient(Guid clientId)
        {
            var client = await App.GetService<IClientRepository>().GetAsync(clientId);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Client = client;
            });
        }

        private async Task GetAvailableClientListAsync()
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

        private async Task GetPassTemplateListAsync()
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
    }
}
