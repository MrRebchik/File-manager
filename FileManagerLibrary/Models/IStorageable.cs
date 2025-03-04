namespace FileManagerLibrary.Models
{
    public interface IStorageable
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsDirectory { get; }
    }
}
