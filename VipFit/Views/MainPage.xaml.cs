namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Animation;
    using VipFit.ViewModels;

    /// <summary>
    /// Main VIP FIT Page with basic controls.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            ViewModel = App.GetService<MainViewModel>();
            InitializeComponent();
        }

        /// <summary>
        /// Gets ViewModel.
        /// </summary>
        public MainViewModel ViewModel
        {
            get;
        }

        private void SellPassButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassPage), null, new DrillInNavigationTransitionInfo());
        }

        private void RegisterEntryButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage), null, new DrillInNavigationTransitionInfo());
        }
    }
}