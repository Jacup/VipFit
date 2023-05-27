namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using System;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassPage"/> class.
        /// </summary>
        public PassPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public PassViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewPassCanceled(object sender, EventArgs e) => Frame.GoBack();

        #region INavigation Implementations

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new PassViewModel
                {
                    IsNewPass = true,
                    IsInEdit = true,
                };
            }
            else
            {
                var guid = (Guid)e.Parameter;
                var client = App.GetService<ClientListViewModel>().Clients.FirstOrDefault(c => c.Model.Id == guid) ?? throw new NotImplementedException("Exception thrown when user tried to create order and NON-VALID clientID was provided.");

                ViewModel = new PassViewModel
                {
                    Client = client.Model,
                    IsNewPass = true,
                    IsInEdit = true,
                };
            }

            ViewModel.AddNewPassCanceled += AddNewPassCanceled;
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

                switch (saveDialog.Result)
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
        /// Disconnects the AddNewPassCanceled event handler from the ViewModel
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewPassCanceled -= AddNewPassCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        #region CommandBar Button_Click

        public async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();
            Frame.GoBack();
        }

        public async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                await ViewModel.RevertChangesAsync();
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

                    // TODO: Show Save confirmation dialog
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.DontSave:
                    await ViewModel.RevertChangesAsync();
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }
        }

        #endregion

    }
}
