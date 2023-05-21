namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EntryPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryPage"/> class.
        /// </summary>
        public EntryPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets Entry Veiw Model.
        /// </summary>
        public EntryViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewEntryCanceled(object sender, EventArgs e) => Frame.GoBack();

        #region INavigation Implementations

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
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

            ViewModel.AddNewEntryCanceled += AddNewEntryCanceled;
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                base.OnNavigatingFrom(e);
                return;
            }

            // Cancel the navigation immediately, otherwise it will continue at the await call.
            e.Cancel = true;

            void ResumeNavigation()
            {
                if (e.NavigationMode == NavigationMode.Back)
                    Frame.GoBack();
                else
                    Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
            }

            SaveChangesDialog saveDialog = new()
            {
                XamlRoot = Content.XamlRoot,
            };
            await saveDialog.ShowAsync();

            switch (saveDialog.Result)
            {
                case SaveChangesDialogResult.Save:
                    await ViewModel.SaveAsync();
                    ResumeNavigation();
                    break;
                case SaveChangesDialogResult.DontSave:
                    ViewModel.IsModified = false;
                    ResumeNavigation();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }

            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Disconnects the AddNewPassCanceled event handler from the ViewModel
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewEntryCanceled -= AddNewEntryCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        public async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();
            Frame.GoBack();
        }

        public async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                Frame.GoBack();
                return;
            }

            SaveChangesDialog saveDialog = new()
            {
                XamlRoot = Content.XamlRoot,
            };

            await saveDialog.ShowAsync();

            switch (saveDialog.Result)
            {
                case SaveChangesDialogResult.Save:
                    await ViewModel.SaveAsync();
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.DontSave:
                    ViewModel.IsModified = false;
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }
        }
    }
}
