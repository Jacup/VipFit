namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer;

    /// <summary>
    /// Pass list ViewModel.
    /// </summary>
    public class PassListViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private PassViewModel? selectedPass;

        private bool isLoading = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassListViewModel"/> class.
        /// </summary>
        public PassListViewModel() => Task.Run(GetPassListAsync);

        /// <summary>
        /// Gets observable collection of Passes.
        /// </summary>
        public ObservableCollection<PassViewModel> Passes { get; } = new();

        /// <summary>
        /// Gets or sets the selected pass, or null if no pass is selected.
        /// </summary>
        public PassViewModel? SelectedPass
        {
            get => selectedPass;
            set => SetProperty(ref selectedPass, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the pass list is currently being updated.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets collection of passList from repository.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task GetPassListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var passList = await App.GetService<IPassRepository>().GetAsync();

            if (passList == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Passes.Clear();

                foreach (var p in passList)
                    Passes.Add(new PassViewModel(p));

                IsLoading = false;
            });
        }
    }
}
