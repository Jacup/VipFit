namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.Extensions.Options;
    using Microsoft.UI.Dispatching;
    using System;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Entry ViewModel.
    /// </summary>
    public class EntryViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private bool isLoading;
        private Entry model;
        private Pass? pass;
        private Client client;
        private ObservableCollection<Pass> passList = new();

        private readonly bool isClientReadOnly;
        private readonly bool isPassReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryViewModel"/> class.
        /// </summary>
        /// <param name="model">Entry model.</param>
        public EntryViewModel()
        {
            Model = new Entry();
            LoadClients();
            Date = DateTime.Now;
        }

        public EntryViewModel(Client client)
        {
            Model = new Entry();
            Date = DateTime.Now;

            LoadClients();

            var matchedClientFromList = AvailableClients.FirstOrDefault(c => c.Id == client.Id);

            if (matchedClientFromList == null)
                return;

            Client = matchedClientFromList;
            isClientReadOnly = true;
        }


        private async void LoadClients()
        {
            await GetAvailableClientListAsync();
        }

        private async void LoadPassesForClient(Client client)
        {
            await GetPassesForClientAsync(client);
        }

        public EntryViewModel(Pass pass)
        {
            Model = new Entry();
            Date = DateTime.Now;

            if (pass == null || pass.Client == null)
                return;

            LoadClients();
            var matchedClientFromList = AvailableClients.FirstOrDefault(c => c.Id == pass.Client.Id);

            LoadPassesForClient(pass.Client);
            var matchedPassFromList = AvailablePasses.FirstOrDefault(p => p.Id == pass.Id);

            if (matchedClientFromList == null || matchedPassFromList == null)
                return;

            isClientReadOnly = true;
            isPassReadOnly = true;

            Client = matchedClientFromList;
            Pass = matchedPassFromList;
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the Entry data.
        /// </summary>
        public event EventHandler AddNewEntryCanceled;

        /// <summary>
        /// Gets or sets the underlying Entry object.
        /// </summary>
        public Entry Model
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
        /// Gets or sets selected pass.
        /// </summary>
        public Pass? Pass
        {
            get => pass;
            set
            {
                if (pass == value)
                    return;

                IsModified = true;
                pass = value;
                OnPropertyChanged();

                RefreshCounters();
                PositionInPass = Convert.ToByte(ThisEntryCounter);
            }
        }

        /// <summary>
        /// Gets or sets the Date of entry.
        /// </summary>
        public DateTime Date
        {
            get => Model.Date;
            set
            {
                if (Model.Date == value)
                    return;

                Model.Date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the position in pass. Indicates how many entries in pass left.
        /// </summary>
        public byte? PositionInPass
        {
            get => Pass == null ? null : Model.PositionInPass;
            private set
            {
                if (value is not null && value != Model.PositionInPass)
                {
                    IsModified = true;
                    Model.PositionInPass = (byte)value;
                    OnPropertyChanged();
                }
            }
        }

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
        /// Gets collection of clients in the list.
        /// </summary>
        public ObservableCollection<Client> AvailableClients { get; } = new();

        /// <summary>
        /// Gets or sets the selected client.
        /// </summary>
        public Client Client
        {
            get => client;
            set
            {
                if (client == value)
                    return;

                IsModified = true;
                client = value;
                OnPropertyChanged();

                if (!IsPassReadOnly)
                    LoadPassesForClient(value);
            }
        }

        /// <summary>
        /// Gets or sets collection of pass in the list.
        /// </summary>
        public ObservableCollection<Pass> AvailablePasses
        {
            get => passList;
            set
            {
                IsModified = true;
                passList = value;
                OnPropertyChanged();
            }
        }

        public int? UsedEntriesCounter => Pass?.Entries.Count;

        public int? LeftEntriesCounter => Pass == null ? null : Pass.PassTemplate.Entries - UsedEntriesCounter;

        public int? ThisEntryCounter => UsedEntriesCounter == null ? null : UsedEntriesCounter + 1;

        public bool IsClientReadOnly => isClientReadOnly;

        public bool IsPassReadOnly => isPassReadOnly;

        /// <summary>
        /// Insert new Entry (if new) and save changes to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task TryToSaveAsync()
        {
            if (!IsModified)
                return;

            try
            {
                Validate();
            }
            catch (ArgumentException e)
            {
                throw e;
            }

            IsModified = false;

            App.GetService<EntryListViewModel>().Entries.Add(this);
            await App.GetService<IEntryRepository>().UpsertAsync(Model);
        }

        private void Validate()
        {
            if (Pass == null)
                throw new ArgumentNullException(nameof(Pass));
            Model.PassId = Pass.Id;

            if (LeftEntriesCounter < 1)
                throw new ArgumentException("All entries has been used.");
        }

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public void CancelEdits() => AddNewEntryCanceled?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Gets all clients.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Gets pass list for selected client.
        /// </summary>
        /// <param name="client">Client with passes.</param>
        /// <returns>Client's passes.</returns>
        internal async Task GetPassesForClientAsync(Client client)
        {
            await dispatcherQueue.EnqueueAsync(() => isLoading = true);

            var passList = await App.GetService<IPassRepository>().GetForClientAsync(client.Id);

            if (passList == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                AvailablePasses.Clear();

                foreach (var p in passList)
                    AvailablePasses.Add(p);

                IsLoading = false;
            });
        }

        private void RefreshCounters()
        {
            OnPropertyChanged(nameof(UsedEntriesCounter));
            OnPropertyChanged(nameof(LeftEntriesCounter));
            OnPropertyChanged(nameof(ThisEntryCounter));
        }
    }
}
