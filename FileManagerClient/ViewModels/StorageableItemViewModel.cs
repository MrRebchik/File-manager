using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using FileManagerClient.Infrastucture;
using FileManagerLibrary.Models;
using System;

namespace FileManagerClient.ViewModels
{
    public partial class StorageableItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool isDirectory;
        [ObservableProperty]
        private string path;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string extention;
        [ObservableProperty]
        private string touchDate;
        [ObservableProperty]
        private string volume;
        [ObservableProperty]
        public Bitmap? imageSource;

        [ObservableProperty]
        private IStorageable storageable;

        public StorageableItemViewModel()
        {
        }
        public StorageableItemViewModel(IStorageable _storageable)
        {
            Initialize(_storageable);
        }
        public void Initialize(IStorageable _storageable)
        {
            Storageable = _storageable;
            IsDirectory = _storageable.IsDirectory;
            Path = _storageable.Path;
            Name = _storageable.Name;
            if (IsDirectory)
            {
                ImageSource = ImageHelper.LoadFromResource(new Uri("avares://FileManagerClient/Assets/icon.png"));
                Extention = "";
                Volume = "";
                TouchDate = "";
            }
            else
            {
                ImageSource = ImageHelper.LoadFromResource(new Uri("avares://FileManagerClient/Assets/icon2.png"));
                File file = (File)_storageable;
                Extention = file.Extension;
                Volume = ((float)file.VolumeInBits / (8 * 1024)).ToString() + " KB";
                TouchDate = file.TouchDate.ToString();
            }
        }
    }
}
