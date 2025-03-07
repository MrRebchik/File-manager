using System.ComponentModel.DataAnnotations.Schema;

namespace FileManagerLibrary.Models
{
    public class File : IStorageable
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int VolumeInBits {  get; set; }
        public DateOnly TouchDate { get; set; }
        [NotMapped]
        public bool IsDirectory { get => false; }
        public File() { }
    }
}
