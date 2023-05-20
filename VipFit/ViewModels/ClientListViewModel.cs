namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;

    /// <summary>
    /// ViewModel for ClientList.
    /// </summary>
    public class ClientListViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private ClientViewModel? selectedClient;

        private bool isLoading = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientListViewModel"/> class.
        /// </summary>
        public ClientListViewModel() => Task.Run(GetClientListAsync);

        /// <summary>
        /// Collection of clients in the list.
        /// </summary>
        public ObservableCollection<ClientViewModel> Clients { get; } = new ObservableCollection<ClientViewModel>();

        /// <summary>
        /// Gets or sets the selected client, or null if no client is selected.
        /// </summary>
        public ClientViewModel? SelectedClient
        {
            get => selectedClient;
            set => SetProperty(ref selectedClient, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clients list is currently being updated.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets the list of clients from database.
        /// </summary>
        /// <returns>List of clients.</returns>
        public async Task GetClientListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var clients = await App.GetService<IClientRepository>().GetAsync();

            if (clients == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Clients.Clear();

                foreach (var client in clients)
                    Clients.Add(new ClientViewModel(client));

                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified clients and reloads the client list from the database.
        /// </summary>
        public void Refresh()
        {
            Task.Run(async () =>
            {
                foreach (var modifiedClient in Clients.Where(client => client.IsModified).Select(client => client.Model))
                    await App.GetService<IClientRepository>().UpsertAsync(modifiedClient);

                IsLoading = true;
                await GetClientListAsync();
                await Task.Delay(TimeSpan.FromSeconds(2));
                isLoading = false;
            });
        }
    }
}