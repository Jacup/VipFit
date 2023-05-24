namespace VipFit.Views
{
    using CommunityToolkit.WinUI;
    using CommunityToolkit.WinUI.UI.Controls;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using Newtonsoft.Json.Linq;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientPaymentsPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public ClientPaymentsPage() => InitializeComponent();

        public PaymentListViewModel ViewModel { get; set; }

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
            if (e.Parameter == null)
            {
                ViewModel = new();
            }
            else
            {
                ViewModel = e.Parameter switch
                {
                    Core.Models.Client client => new(client),
                    Core.Models.Pass pass => new(pass),
                    _ => throw new NotImplementedException(),
                };
            }

            base.OnNavigatedTo(e);
        }

        private void CalendarDatePicker_Opened(object sender, object e)
        {
            CalendarDatePicker calendarDatePicker = (CalendarDatePicker)sender;

            var dateTimeOffset = (DateTimeOffset)calendarDatePicker.Date;

            if (dateTimeOffset.Year < 1950)
                calendarDatePicker.Date = DateTime.Today;
        }

        private void RefreshButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void SuspendButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

            ViewModel.SuspendPayment(ViewModel.SelectedPayment);
            ViewModel.SelectedPayment = null;
        }

        private void ResumeButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ResumePayment(ViewModel.SelectedPayment);
            ViewModel.SelectedPayment = null;
        }

    }
}
