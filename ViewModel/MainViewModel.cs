using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ProgressApp.Data;

namespace ProgressApp.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly string filePath;
    public MainViewModel()
    {
        filePath = Path.Combine(FileSystem.AppDataDirectory, "item.json");
        Items = new ObservableCollection<DataClass>();
        //LoadItems();
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
        SaveItems();
    }

    [RelayCommand]
    void Delete(DataClass s)
    {
        if(Items.Contains(s))
        { 
            Items.Remove(s);
            SaveItems();
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
