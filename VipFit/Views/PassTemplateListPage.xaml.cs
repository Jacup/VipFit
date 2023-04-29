namespace VipFit.Views
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Animation;
    using VipFit.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassTemplateListPage : Page
    {
        public PassTemplateListViewModel ViewModel { get; }

        public PassTemplateListPage()
        {
            ViewModel = App.GetService<PassTemplateListViewModel>();
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedPassTemplate == null)
                return;

            Frame.Navigate(typeof(PassTemplatePage), ViewModel.SelectedPassTemplate.Model.Id, new DrillInNavigationTransitionInfo());
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassTemplatePage), null, new DrillInNavigationTransitionInfo());

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedPassTemplate == null)
                return;

            DeleteDialog deleteDialog = new()
            {
                XamlRoot = Content.XamlRoot
            };

            await deleteDialog.ShowAsync();

            if (!deleteDialog.DeleteConfirmed)
                return;

            await ViewModel.SelectedPassTemplate.DeleteAsync();

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Refresh();

        }


    }
}
