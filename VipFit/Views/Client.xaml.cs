namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Animation;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.Helpers;
    using VipFit.Interfaces;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Client : Page, IHeaderChanger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client() => InitializeComponent();

        /// <summary>
        /// Gets or sets the Client ViewModel.
        /// </summary>
        public ClientViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets header.
        /// </summary>
        public HeaderHelper Header { get; private set; } = new();

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewClientCanceled(object sender, EventArgs e) => Frame.GoBack();

        #region INavigation Implementations

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new ClientViewModel
                {
                    IsNewClient = true,
                    IsInEdit = true,
                };
                VisualStateManager.GoToState(this, "NewCustomer", false);
            }
            else
            {
                ViewModel =
                    App.GetService<ClientListViewModel>()
                    .Clients
                    .Where(c => c.Model.Id == (Guid)e.Parameter)
                    .First();
            }
            Header.Text = ViewModel.Name;
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
                        SaveAndUpdateHeader();
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

        public void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
        {
            // If the SelectAll box is checked (all options are selected),
            // clicking the box will change it to its indeterminate state.
            // Instead, we want to uncheck all the boxes,
            // so we do this programatically. The indeterminate state should
            // only be set programatically, not by the user.

            if (ViewModel.AgreementMarketing && ViewModel.AgreementPromoImage && ViewModel.AgreementWebsiteImage && ViewModel.AgreementSocialsImage)
            {
                // This will cause SelectAll_Unchecked to be executed, so
                // we don't need to uncheck the other boxes here.
                ViewModel.AgreementsAll = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                // todo: dialog: not made any changes.
            }

            SaveAndUpdateHeader();
        }

        private async void SaveAndUpdateHeader()
        {
            await ViewModel.SaveAsync();
            Header.Text = ViewModel.Name;
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                await ViewModel.RevertChangesAsync();
                return;
            }

            var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
            saveDialog.XamlRoot = this.Content.XamlRoot;
            await saveDialog.ShowAsync();
            SaveChangesDialogResult result = saveDialog.Result;

            switch (result)
            {
                case SaveChangesDialogResult.Save:
                    SaveAndUpdateHeader();
                    break;
                case SaveChangesDialogResult.DontSave:
                    await ViewModel.RevertChangesAsync();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteDialog deleteDialog = new();
            deleteDialog.XamlRoot = Content.XamlRoot;
            await deleteDialog.ShowAsync();

            bool result = deleteDialog.DeleteConfirmed;

            if (!result)
                return;

            if (ViewModel != null)
            {
                await ViewModel.DeleteAsync();
                Frame.GoBack();
            }
        }

        private void SellPassButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassPage), ViewModel.Model, new DrillInNavigationTransitionInfo());
        }

        private async void RegisterEntryButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage), ViewModel.SelectedPass, new DrillInNavigationTransitionInfo());
        }

        #endregion
    }
}
