namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;

    public class PassTemplateListViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private PassTemplateViewModel? selectedPassTemplate;

        private bool isLoading;

        public PassTemplateListViewModel() => Task.Run(GetPassTemplateListAsync);

        /// <summary>
        /// Gets the collection of PassTemplates in the list.
        /// </summary>
        public ObservableCollection<PassTemplateViewModel> PassTemplates { get; } = new ObservableCollection<PassTemplateViewModel>();

        /// <summary>
        /// Gets or sets the selected PassTemplate, or null if no PassTemplate is selected.
        /// </summary>
        public PassTemplateViewModel? SelectedPassTemplate
        {
            get => selectedPassTemplate;
            set => SetProperty(ref selectedPassTemplate, value);
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
        /// Gets the list of PassTemplates from database.
        /// </summary>
        /// <returns>List of PassTemplates.</returns>
        public async Task GetPassTemplateListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var passTemplates = await App.GetService<IPassTemplateRepository>().GetAsync();

            if (passTemplates == null)
                return;

            await dispatcherQueue.EnqueueAsync(() =>
            {
                PassTemplates.Clear();

                foreach (var p in passTemplates)
                    PassTemplates.Add(new PassTemplateViewModel(p));

                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified PassTemplates and reloads the pass template list from the database.
        /// </summary>
        public void Refresh()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                SelectedPassTemplate = null;
                await GetPassTemplateListAsync();
                isLoading = false;
            });
        }
    }
}
