namespace VipFit.Views
{
    using Microsoft.UI.Xaml.Controls;
    using VipFit.ViewModels;

    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel
        {
            get;
        }

        public MainPage()
        {
            ViewModel = App.GetService<MainViewModel>();
            InitializeComponent();
        }
    }
}