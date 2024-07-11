using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace ZeroEPUB.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ExitApp(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("It would exit here");
    }
}
