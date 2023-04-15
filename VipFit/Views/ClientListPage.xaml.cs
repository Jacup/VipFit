using Microsoft.UI.Xaml.Controls;

using VipFit.ViewModels;

namespace VipFit.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ClientListPage : Page
{
    public ClientListViewModel ViewModel
    {
        get;
    }

    public ClientListPage()
    {
        ViewModel = App.GetService<ClientListViewModel>();
        InitializeComponent();
    }
}
