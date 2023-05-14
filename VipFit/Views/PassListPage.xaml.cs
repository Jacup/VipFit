namespace VipFit.Views
{
    using Microsoft.UI.Xaml.Controls;
    using VipFit.ViewModels;

    public sealed partial class PassListPage : Page
    {
        public PassListPage()
        {
            ViewModel = App.GetService<PassListViewModel>();
            InitializeComponent();
        }

        public PassListViewModel ViewModel { get; }
    }
}
