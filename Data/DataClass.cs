using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgressApp.Data;

public partial class DataClass : ObservableObject
{
    [ObservableProperty]
    string fieldName;
    [ObservableProperty]
    int currentChapter;
    [ObservableProperty]
    int maxChapter;
    [ObservableProperty]
    bool isEditing;

    public DataClass(string fieldName, int currentChapter, int maxChapter)
    {
        FieldName = fieldName;
        CurrentChapter = currentChapter;
        MaxChapter = maxChapter;
    }

    [RelayCommand]
    void Edit()
    {
        IsEditing = true;
    }

    [RelayCommand]
    void EndEdit()
    {
        IsEditing = false; 
    }
}
