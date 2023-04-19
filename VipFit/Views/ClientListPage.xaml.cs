using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using VipFit.ViewModels;

namespace VipFit.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ClientListPage : Page
{
    private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public ClientListViewModel ViewModel { get; }

    public ClientListPage()
    {
        ViewModel = App.GetService<ClientListViewModel>();
        InitializeComponent();
    }

    private async Task ResetClientList() => await dispatcherQueue.EnqueueAsync(ViewModel.GetClientListAsync);

    /// <summary>
    /// Reverts all changes to the row if the row has changes but a cell is not currently in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Escape &&
            ViewModel.SelectedClient != null &&
            ViewModel.SelectedClient.IsModified &&
            !ViewModel.SelectedClient.IsInEdit)
        {
            (sender as DataGrid).CancelEdit(DataGridEditingUnit.Row);
        }
    }

    /// <summary>
    /// Selects right click the tapped Client. 
    /// </summary>
    private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
        ViewModel.SelectedClient = (e.OriginalSource as FrameworkElement).DataContext as ClientViewModel;




    /// <summary>
    /// Resets the clients list when leaving the page.
    /// </summary>
    protected async override void OnNavigatedFrom(NavigationEventArgs e)
    {
        await ResetClientList();
    }

    /// <summary>
    /// Applies any existing filter when navigating to the page.
    /// </summary>
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
    }

    private void NewClient_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(Client), null, new DrillInNavigationTransitionInfo());
    }

    private void EditClient_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedClient != null)
        {
            Frame.Navigate(typeof(Client), ViewModel.SelectedClient.Model.Id,
                new DrillInNavigationTransitionInfo());
        }
    }

    /// <summary>
    /// Menu flyout click control for selecting a customer and displaying details.
    /// </summary>
    private void ViewClient_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedClient != null)
        {
            Frame.Navigate(typeof(Client), ViewModel.SelectedClient.Model.Id,
                new DrillInNavigationTransitionInfo());
        }
    }

    private void RegisterTraining_Click(object sender, RoutedEventArgs e)
    {

    }

    private void SellPass_Click(object sender, RoutedEventArgs e)
    {

    }
}
