using Microsoft.UI.Xaml.Controls;

using VipFit.ViewModels;

namespace VipFit.Views;

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
