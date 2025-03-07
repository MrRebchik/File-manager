using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerClient.Services;
using FileManagerLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerClient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IStorageProviderService storageProviderService;
        [ObservableProperty]
        private string currentPath = "C:";
        [ObservableProperty]
        public StorageableItemViewModel selectedStorageable;
        [ObservableProperty]
        public IStorageable storageable;
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private bool isDirectory;
        private bool CanGoBack { get { return Storageable == null ? false : SelectedStorageable.Path.Length > 1; } }
        public ObservableCollection<StorageableItemViewModel> includes {  get; } = new ObservableCollection<StorageableItemViewModel>();

        public MainWindowViewModel(IStorageProviderService storageProviderService)
        {
            Username = AuthService.Username;
            this.storageProviderService = storageProviderService;
            GetFromRoot();
        }
        [RelayCommand (CanExecute = nameof(CanGoBack))]
        public async Task GoBack()
        {
            string previousDir = SelectedStorageable.Path.
                Substring(0, SelectedStorageable.Path.Length - SelectedStorageable.Path.Split(@"\").LastOrDefault().Count() - 1);
            IStorageable storageable = await storageProviderService.GetStorageablesAsync(
                true, previousDir);
            includes.Clear();
            SelectedStorageable = new StorageableItemViewModel(storageable);
            Storageable = storageable;
            IsDirectory = SelectedStorageable.IsDirectory;
            if (SelectedStorageable.IsDirectory)
            {
                foreach (var d in ((Directory)SelectedStorageable.Storageable).IncludesFiles)
                {
                    includes.Add(new StorageableItemViewModel(d));
                }
                foreach (var d in ((Directory)SelectedStorageable.Storageable).IncludesDirectories)
                {
                    includes.Add(new StorageableItemViewModel(d));
                }
            }
            CurrentPath = storageable.Path + storageable.Name;
            GoBackCommand.NotifyCanExecuteChanged();
        }
        public async Task GetFromPath()
        {
            IStorageable storageable = await storageProviderService.GetStorageablesAsync(
                SelectedStorageable.IsDirectory,
                SelectedStorageable.Path + 
                (SelectedStorageable.IsDirectory ? SelectedStorageable.Name : ((File)SelectedStorageable.Storageable).Name + ((File)SelectedStorageable.Storageable).Extension));
            includes.Clear();
            SelectedStorageable = new StorageableItemViewModel(storageable);
            Storageable = storageable;
            IsDirectory = SelectedStorageable.IsDirectory;
            if ( SelectedStorageable.IsDirectory )
            {
                foreach( var d in ((Directory)SelectedStorageable.Storageable).IncludesFiles)
                {
                    includes.Add(new StorageableItemViewModel(d));
                }
                foreach(var d in ((Directory)SelectedStorageable.Storageable).IncludesDirectories)
                {
                    includes.Add(new StorageableItemViewModel(d));
                } 
            }
            CurrentPath = storageable.Path + storageable.Name;
            GoBackCommand.NotifyCanExecuteChanged();
        }
        private async void GetFromRoot()
        {
            IStorageable storageable = await storageProviderService.GetStorageablesAsync(true, @"C:");
            SelectedStorageable = new StorageableItemViewModel(storageable);
            Storageable = storageable;
            CurrentPath = storageable.Path;
            IsDirectory = SelectedStorageable.IsDirectory;
            List<IStorageable> includesToAdd = ((Directory)storageable).IncludesFiles.Select(i => (IStorageable)i).ToList() ;
            includesToAdd.AddRange(((Directory)storageable).IncludesDirectories);
            foreach (var include in includesToAdd)
            {
                includes.Add(new StorageableItemViewModel(include));
            }
        }
        [RelayCommand]
        public void SortItemsAlphabetically()
        {
            var sortedItems = includes.OrderByDescending(item => item.Name).ToList();
            includes.Clear();
            foreach (var item in sortedItems)
            {
                includes.Add(item);
            }
        }
    }
}
