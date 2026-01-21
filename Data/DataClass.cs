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
    [ObservableProperty]
    bool isCompleted;

    public DataClass(string fieldName, int currentChapter, int maxChapter)
    {
        FieldName = fieldName;
        CurrentChapter = currentChapter;
        MaxChapter = maxChapter;
        if (CurrentChapter == MaxChapter)
            IsCompleted = true;
    }

    [RelayCommand]
    void Edit()
    {
        IsEditing = true;
    }

    [RelayCommand]
    void EndEdit()
    {
        ChangeCompletedStatus();
        IsEditing = false; 
    }

    internal void ChangeCompletedStatus()
    {
        if(CurrentChapter == MaxChapter)
            IsCompleted = true;
        else
            IsCompleted = false;
    }

    [RelayCommand]
    void AddCompletedChapter()
    {
        if (CurrentChapter + 1 > MaxChapter)
            return;
        IsEditing = true;
        CurrentChapter++;
        ChangeCompletedStatus();
        IsEditing = false;
    }

    [RelayCommand]
    void RemoveCompletedChapter()
    {
        if (CurrentChapter - 1 < 0)
            return;
        IsEditing = true;
        CurrentChapter--;
        ChangeCompletedStatus();
        IsEditing = false;
    }

    public double CompPercentage =>
        MaxChapter == 0 ? 0 : (double)CurrentChapter / MaxChapter;
}
