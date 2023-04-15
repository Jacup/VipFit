using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using VipFit.Contracts.ViewModels;
using VipFit.Core.Contracts.Services;
using VipFit.Core.Models;

namespace VipFit.ViewModels;

public class ClientListViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public ClientListViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
