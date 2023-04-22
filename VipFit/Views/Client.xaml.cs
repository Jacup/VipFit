namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using System.Runtime.InteropServices;
    using VipFit.Interfaces;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Client : Page
    {
        /// <summary>
        /// Client ViewModel.
        /// </summary>
        public ClientViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewClientCanceled(object sender, EventArgs e) => Frame.GoBack();

        /// <summary>
        /// Initialize the Client Page.
        /// </summary>
        public Client()
        {
            this.InitializeComponent();
        }

        #region INavigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new ClientViewModel
                {
                    IsNewClient = true,
                    IsInEdit = true
                };
                VisualStateManager.GoToState(this, "NewCustomer", false);
            }
            else
            {

                var vm = App.GetService<ClientListViewModel>();
                var clients = App.GetService<ClientListViewModel>().Clients;
                ViewModel = App.GetService<ClientListViewModel>().Clients.Where(
                    c => c.Model.Id == (Guid)e.Parameter).First();
            }

            ViewModel.AddNewClientCanceled += AddNewClientCanceled;
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel.IsModified)
            {
                // Cancel the navigation immediately, otherwise it will continue at the await call. 
                e.Cancel = true;

                void resumeNavigation()
                {
                    if (e.NavigationMode == NavigationMode.Back)
                        Frame.GoBack();
                    else
                        Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                }

                var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
                saveDialog.XamlRoot = this.Content.XamlRoot;
                await saveDialog.ShowAsync();
                SaveChangesDialogResult result = saveDialog.Result;

                switch (result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        await ViewModel.RevertChangesAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
                        break;
                }
            }

            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Disconnects the AddNewCustomerCanceled event handler from the ViewModel 
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewClientCanceled -= AddNewClientCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        #region Buttons Actions

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
                return;

            var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
            saveDialog.XamlRoot = this.Content.XamlRoot;
            await saveDialog.ShowAsync();
            SaveChangesDialogResult result = saveDialog.Result;

            switch (result)
            {
                case SaveChangesDialogResult.Save:
                    await ViewModel.SaveAsync();
                    break;
                case SaveChangesDialogResult.DontSave:
                    await ViewModel.RevertChangesAsync();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }

        }

        #endregion
    }
}
