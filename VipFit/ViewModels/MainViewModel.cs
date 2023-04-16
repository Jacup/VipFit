﻿using CommunityToolkit.Mvvm.ComponentModel;
using VipFit.Database;

namespace VipFit.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public MainViewModel()
    {
        var dbcontext = App.GetService<VipFitContext>();
        var entities = dbcontext.Clients.ToList();
    }
}
