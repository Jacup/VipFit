namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using System;
    using VipFit.Helpers;
    using VipFit.Interfaces;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassPage : Page, IHeaderChanger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassPage"/> class.
        /// </summary>
        public PassPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        /// <summary>
        /// Gets or sets the Pass ViewModel.
        /// </summary>
        public PassViewModel ViewModel { get; set; }

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
                ViewModel = new(isNewPass: true)
                {
                    IsInEdit = true,
                };
            }
            else
            {
                ViewModel = e.Parameter switch
                {
                    Core.Models.Client client => new(client),
                    _ => throw new NotImplementedException(),
                };
                ViewModel.IsNew = true;
                ViewModel.IsInEdit = true;
            }

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse("Resources");
            Header.Text = resourceLoader.GetString("Page_SellPass");

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
                        await ViewModel.RevertChangesAsync();
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
            ViewModel.AddNewPassCanceled -= AddNewPassCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new customer record.
        /// </summary>
        private void AddNewPassCanceled(object sender, EventArgs e) => Frame.GoBack();

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
