namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using VipFit.Helpers;
    using VipFit.Interfaces;
    using VipFit.ViewModels;
    using VipFit.Views.Dialogs;

    /// <summary>
    /// A page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassTemplatePage : Page, IHeaderChanger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassTemplatePage"/> class.
        /// </summary>
        public PassTemplatePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the PassTemplate ViewModel.
        /// </summary>
        public PassTemplateViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets Header Helper.
        /// </summary>
        public HeaderHelper Header { get; private set; } = new();

        #region INavigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new PassTemplateViewModel
                {
                    IsNew = true,
                    IsInEdit = true,
                };
            }
            else
            {
                ViewModel = App.GetService<PassTemplateListViewModel>().PassTemplates
                               .First(c => c.Model.Id == (Guid)e.Parameter);
            }

            Header.Text = ViewModel.PassCode;
            ViewModel.IsInEdit = true;
            ViewModel.AddNewPassTemplateCanceled += AddNewCanceled;
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
        /// Disconnects the AddNewPassTemplateCanceled event handler from the ViewModel 
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewPassTemplateCanceled -= AddNewCanceled;
            base.OnNavigatedFrom(e);
        }

        #endregion

        #region Buttons Actions

        private void AddNewCanceled(object sender, EventArgs e) => Frame.GoBack();

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();
            //ShowSuccessDialog();

            Frame.GoBack();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsModified)
            {
                await ViewModel.RevertChangesAsync();
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
                    //ShowSuccessDialog();
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.DontSave:
                    if (!ViewModel.IsNew)
                        await ViewModel.RevertChangesAsync();
                    ViewModel.IsModified = false;
                    Frame.GoBack();
                    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteDialog deleteDialog = new()
            {
                XamlRoot = Content.XamlRoot,
            };
            await deleteDialog.ShowAsync();

            if (!deleteDialog.DeleteConfirmed)
                return;

            if (ViewModel != null)
            {
                await ViewModel.DeleteAsync();
                Frame.GoBack();
            }
        }

        #endregion

        private void NumberBox_Loaded(object sender, RoutedEventArgs e)
        {
            var box = sender as NumberBox;
            var textBox = (TextBox)box.VisualTreeFindName("InputBox");
            textBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text;
            bool isNumber = !text.Any(t => !char.IsDigit(t));

            if (isNumber)
            {
                decimal.TryParse(text, out decimal value);

                if (value != ViewModel.PricePerMonth)
                    ViewModel.PricePerMonth = value;
            }
        }

        private async void ShowSuccessDialog()
        {
            SuccessDialog successDialog = new();
            XamlRoot = Content.XamlRoot;
            await successDialog.ShowAsync();
        }
    }
}
