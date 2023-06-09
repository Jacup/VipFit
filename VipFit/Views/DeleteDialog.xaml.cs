namespace VipFit.Views
{
    using Microsoft.UI.Xaml.Controls;


    /// <summary>
    /// Creates a dialog that gives asks user for confirmation on deleting object.
    /// </summary>
    public sealed partial class DeleteDialog : ContentDialog
    {
        public DeleteDialog()
        {
            InitializeComponent();

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse("Dialogs");

            Title = resourceLoader.GetString("DeleteDialog/Title");
            Content = resourceLoader.GetString("DeleteDialog/Content");
            PrimaryButtonText = resourceLoader.GetString("DeleteDialog/PrimaryButtonText");
            CloseButtonText = resourceLoader.GetString("DeleteDialog/CloseButtonText");
        }

        public bool DeleteConfirmed { get; private set; } = false;

        private void DeleteDialog_DeleteButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DeleteConfirmed = true;
            Hide();
        }

        private void DeleteDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DeleteConfirmed = false;
            Hide();
        }
    }
}
