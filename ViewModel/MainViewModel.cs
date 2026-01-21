using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using ProgressApp.Data;
using ProgressApp.Data.Enums;
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
        ItemFontSize = Preferences.Get(nameof(ItemFontSize), 14.0);
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
    [ObservableProperty]
    double itemFontSize;

    bool isNameSortAsc = true;
    bool isCompSortAsc = true; 
    SortDirection nameSortDirection = SortDirection.Ascending;
    SortDirection completionSortDirection = SortDirection.Ascending;


    [RelayCommand]
    void IncreaseFont()
    {
        ItemFontSize += 1;
    }

    [RelayCommand]
    void DecreaseFont()
    {
        if (ItemFontSize - 1 < 6)
            return;

        ItemFontSize -= 1;
    }


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

    [RelayCommand]
    void SortByName()
    {
        var sorted = isNameSortAsc
            ? Items.OrderBy(i => i.FieldName)
            : Items.OrderByDescending(i => i.FieldName);

        ApplySort(sorted);
        isNameSortAsc = !isNameSortAsc;

        nameSortDirection =
        nameSortDirection == SortDirection.Ascending
            ? SortDirection.Descending
            : SortDirection.Ascending;

        OnPropertyChanged(nameof(SortByNameText));
    }

    [RelayCommand]
    void SortByComp()
    {
        var sorted = isCompSortAsc
            ? Items.OrderBy(i => i.CompPercentage)
            : Items.OrderByDescending(i => i.CompPercentage);

        ApplySort(sorted);
        isCompSortAsc = !isCompSortAsc;

        completionSortDirection =
            completionSortDirection == SortDirection.Ascending
                ? SortDirection.Descending
                : SortDirection.Ascending;

        OnPropertyChanged(nameof(SortByCompletionText));
    }

    private void ApplySort(IOrderedEnumerable<DataClass> sorted)
    {
        Items.CollectionChanged -= Items_CollectionChanged;
        var list = sorted.ToList();

        Items.Clear();

        foreach (var item in list)
        {
            Items.Add(item);
        }

        Items.CollectionChanged += Items_CollectionChanged;
        SaveItems();
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
        foreach (var item in Items)
        {
            item.PropertyChanged += Item_PropertyChanged;
        }
    }

    private void SaveItems()
    {
        var json = JsonSerializer.Serialize(Items);
        File.WriteAllText(filePath, json);
    }

    public string SortByNameText =>
    nameSortDirection == SortDirection.Ascending
        ? "Name ↓"
        : "Name ↑";

    public string SortByCompletionText =>
        completionSortDirection == SortDirection.Ascending
            ? "% ↓"
            : "% ↑";

    partial void OnItemFontSizeChanged(double value)
    {
        Preferences.Set(nameof(ItemFontSize), value);
    }


}
