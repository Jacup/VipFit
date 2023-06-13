namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Animation;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.Helpers;
    using VipFit.Interfaces;
    using VipFit.ViewModels;
    using VipFit.Views.Dialogs;

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

        #region INavigation Implementations

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new ClientViewModel
                {
                    IsNew = true,
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

        /// <inheritdoc/>
        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel.IsModified)
            {
                // Cancel the navigation immediately, otherwise it will continue at the await call.
                e.Cancel = true;

                void ResumeNavigation()
                {
                    if (e.NavigationMode == NavigationMode.Back)
                        Frame.GoBack();
                    else
                        Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                }

                var saveDialog = new SaveChangesDialog();
                saveDialog.XamlRoot = Content.XamlRoot;
                await saveDialog.ShowAsync();
                SaveChangesDialogResult result = saveDialog.Result;

                switch (result)
                {
                    case SaveChangesDialogResult.Save:
                        SaveAndUpdate();
                        ResumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        await ViewModel.RevertChangesAsync();
                        ResumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
                        break;
                }
            }

            base.OnNavigatingFrom(e);
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewClientCanceled -= AddNewClientCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        private static SlideNavigationTransitionInfo SlideInNavigation() => new() { Effect = SlideNavigationTransitionEffect.FromRight };

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewClientCanceled(object sender, EventArgs e) => Frame.GoBack();

        #region Buttons Actions

        private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AgreementMarketing && ViewModel.AgreementPromoImage && ViewModel.AgreementWebsiteImage && ViewModel.AgreementSocialsImage)
                ViewModel.AgreementsAll = false;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                await ViewModel.RevertChangesAsync();
                return;
            }

            SaveAndUpdate();
        }

        private async void SaveAndUpdate()
        {
            await ViewModel.SaveAsync();
            Header.Text = ViewModel.Name;
            //ShowSuccessDialog();
        }

        private async void ShowSuccessDialog()
        {
            var successDialog = new SuccessDialog();
            XamlRoot = Content.XamlRoot;
            await successDialog.ShowAsync();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                await ViewModel.RevertChangesAsync();
                return;
            }

            var saveDialog = new SaveChangesDialog();
            saveDialog.XamlRoot = Content.XamlRoot;
            await saveDialog.ShowAsync();
            SaveChangesDialogResult result = saveDialog.Result;

            switch (result)
            {
                case SaveChangesDialogResult.Save:
                    SaveAndUpdate();
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

        private async void SellPassButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassPage), ViewModel.Model, SlideInNavigation());
        }

        private async void RegisterEntryButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage), ViewModel.SelectedPass, SlideInNavigation());
        }

        private async void ShowPaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ClientPaymentsPage), ViewModel.SelectedPass, SlideInNavigation());
        }

        #endregion
    }
}
