using Avalonia.Controls;
using Avalonia.Input;
using FileManagerClient.ViewModels;
using FileManagerLibrary.Models;

namespace FileManagerClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void ListBox_DoubleTapped(object sender, TappedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null)
                {
                    viewModel.selectedStorageable = listBox.SelectedItem as StorageableItemViewModel;
                    await viewModel.GetFromPath();
                }
            }
        }
    }
}