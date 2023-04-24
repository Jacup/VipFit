namespace VipFit.Views
{
    using Microsoft.UI.Xaml.Controls;
    using System.ComponentModel;
    using VipFit.Helpers;
    using VipFit.Interfaces;
    using VipFit.ViewModels;

    public sealed partial class MainPage : Page, INotifyPropertyChanged, IHeaderChanger
    {
        public MainViewModel ViewModel
        {
            get;
        }

        public HeaderHelper Header { get; private set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;


        public MainPage()
        {
            ViewModel = App.GetService<MainViewModel>();
            InitializeComponent();

            Header.Text = "test Header";
        }
    }
}