using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using ProgressApp.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ProgressApp.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly string filePath;
    public MainViewModel()
    {
#if ANDROID
        filePath = Path.Combine(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath, "item.json");
#else
        filePath = Path.Combine(FileSystem.AppDataDirectory, "item.json");
#endif
        Items = new ObservableCollection<DataClass>();
        LoadItems();
        Items.CollectionChanged += Items_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (DataClass item in e.NewItems)
                item.PropertyChanged += Item_PropertyChanged;

        if (e.OldItems != null)
            foreach (DataClass item in e.OldItems)
                item.PropertyChanged -= Item_PropertyChanged;

        SaveItems();
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DataClass.IsEditing)
            && sender is DataClass item
            && !item.IsEditing)
            SaveItems();
    }

    [ObservableProperty]
    ObservableCollection<DataClass> items;

    [ObservableProperty]
    string text;
    [ObservableProperty]
    int currentChapter;
    [ObservableProperty]
    int totalChapter;

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;
        Items.Add(new DataClass(Text, currentChapter, totalChapter));
        Text = string.Empty;
        CurrentChapter = 0;
        TotalChapter = 0;
    }

    [RelayCommand]
    void Delete(DataClass s)
    {
        if(Items.Contains(s))
        { 
            Items.Remove(s);
        }
    }

    private void LoadItems()
    {
        if (!File.Exists(filePath))
            return;

        var json = File.ReadAllText(filePath);
        var loader = JsonSerializer.Deserialize<ObservableCollection<DataClass>>(json);

        if (loader is null)
            return;

        Items = loader;
    }

    private void SaveItems()
    {
        var json = JsonSerializer.Serialize(Items);
        File.WriteAllText(filePath, json);
    }
}
