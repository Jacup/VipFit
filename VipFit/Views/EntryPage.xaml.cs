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
                ViewModel = new EntryViewModel();
            }
            else
            {
                throw new NotImplementedException();
                //var passId = (Guid)e.Parameter;
                //var pass = App.GetService<PassListViewModel>().Passes.FirstOrDefault(p => p.Model.Id == passId)
                //    ?? throw new NotImplementedException("Exception thrown when user tried to create order and NON-VALID passID was provided.");

                //ViewModel = new EntryViewModel
                //{
                //    Pass = pass.Model,


                //};
            }

            ViewModel.AddNewEntryCanceled += AddNewEntryCanceled;
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

                void ResumeNavigation()
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

                switch (saveDialog.Result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        ResumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        ResumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
                        break;
                }
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
        }

        public async void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
