namespace VipFit.ViewModels
{
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Entry List ViewModel.
    /// </summary>
    internal class EntryListViewModel
    {
        /// <summary>
        /// Gets observable collection of Entries.
        /// </summary>
        public ObservableCollection<EntryViewModel> Entries { get; } = new();
    }
}
