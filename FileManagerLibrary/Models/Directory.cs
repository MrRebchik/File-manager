using System.ComponentModel.DataAnnotations.Schema;

namespace FileManagerLibrary.Models
{
    public class Directory : IStorageable
    {
        public string Path { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public bool IsDirectory { get => true; }
        public List<Directory> IncludesDirectories { get; set; } = new();
        public List<File> IncludesFiles { get; set; } = new();
        public Directory()
        {
        }
        public Directory(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }
}
