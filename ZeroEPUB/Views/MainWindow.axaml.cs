﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Diagnostics;
using System.IO;

namespace ZeroEPUB.Views;

public partial class MainWindow : Window
{
    EpubOpener opener = new();
    public MainWindow()
    {
        InitializeComponent();
    }
    public async void OpenFile(object source, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open EPUB File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // send file to epub opener
            opener.OpenEpub(files[0].TryGetLocalPath());
        }
    }
}
