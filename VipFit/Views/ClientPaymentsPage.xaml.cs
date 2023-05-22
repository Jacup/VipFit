namespace VipFit.Views
{
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientPaymentsPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public ClientPaymentsPage()
        {
            ViewModel = App.GetService<PaymentListViewModel>();

            this.InitializeComponent();
        }

        public PaymentListViewModel ViewModel { get; }

        private async Task ResetPaymentList() => await dispatcherQueue.EnqueueAsync(ViewModel.GetPaymentListAsync);

        /// <summary>
        /// Resets the clients list when leaving the page.
        /// </summary>
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ResetPaymentList();
        }

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ResetPaymentList();
        }
    }
}
