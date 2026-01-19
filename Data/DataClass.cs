using CommunityToolkit.Mvvm.ComponentModel;
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

    public DataClass(string fieldName, int currentChapter, int maxChapter)
    {
        FieldName = fieldName;
        CurrentChapter = currentChapter;
        MaxChapter = maxChapter;
    }
}
