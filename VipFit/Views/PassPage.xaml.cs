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
        public PassPage() => InitializeComponent();

        public PassViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewPassCanceled(object sender, EventArgs e) => Frame.GoBack();

        #region INavigation Implementations

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new PassViewModel { IsNewPass = true, Client = (Core.Models.Client)e.Parameter, };

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

                SaveChangesDialog saveDialog = new()
                {
                    XamlRoot = Content.XamlRoot
                };

                await saveDialog.ShowAsync();

                switch (saveDialog.Result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        if (!ViewModel.IsNewPass)
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
                    if (!ViewModel.IsNewPass)
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
