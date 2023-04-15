using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VipFit.Database;
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

        Output.ItemsSource = DataAccess.GetData();
    }

    public void AddData(object sender, RoutedEventArgs e)
    {
        DataAccess.AddData(Input_Box.Text);

        Output.ItemsSource = DataAccess.GetData();
    }
}
