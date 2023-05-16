namespace VipFit.Views
{
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media.Animation;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.ViewModels;

    /// <summary>
    /// Pass list page.
    /// </summary>
    public sealed partial class PassListPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Initializes a new instance of the <see cref="PassListPage"/> class.
        /// </summary>
        public PassListPage()
        {
            ViewModel = App.GetService<PassListViewModel>();
            InitializeComponent();
        }

        /// <summary>
        /// Gets View Model.
        /// </summary>
        public PassListViewModel ViewModel { get; }

        private async Task ResetPassList() => await dispatcherQueue.EnqueueAsync(ViewModel.GetPassListAsync);

        #region DataGrid Implementations

        /// <summary>
        /// Selects right click the tapped Pass.
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedPass = (e.OriginalSource as FrameworkElement).DataContext as PassViewModel;

        #endregion

        #region INavigation Implementations

        /// <summary>
        /// Resets the clients list when leaving the page.
        /// </summary>
        protected override async void OnNavigatedFrom(NavigationEventArgs e) => await ResetPassList();

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion

        #region Buttons

        private void SellPassButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassPage), null, new DrillInNavigationTransitionInfo());
        }

        private void EditPassButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedPass == null)
                return;

            Frame.Navigate(typeof(PassPage), ViewModel.SelectedPass.Model.Id, new DrillInNavigationTransitionInfo());
        }

        #endregion

    }
}
