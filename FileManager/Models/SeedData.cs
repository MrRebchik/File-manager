using FileManagerLibrary.Models;

namespace FileManagerAPI.Models
{
    public static class SeedData
    {
        public static void SeedPeopleDatabase(PeopleContext peopleContext)
        {
            if (peopleContext.People.Count() != 0)
                return;
            List<Person> people = new List<Person>()
            {
                new Person("user", "123"),
                new Person("admin", "admin"),
            };
            peopleContext.People.AddRange(people);
            peopleContext.SaveChanges();
        }
        public static void SeedStorageDatabase(StorageContext storageContext)
        {
            if (storageContext.Directories.Count() != 0 || storageContext.Files.Count() != 0)
                return;
            List<FileManagerLibrary.Models.Directory> directories = new List<FileManagerLibrary.Models.Directory>()
            {
                new FileManagerLibrary.Models.Directory("","C:"),
                new FileManagerLibrary.Models.Directory("C:\\","Users"),
                new FileManagerLibrary.Models.Directory("C:\\Users\\","user"),
                new FileManagerLibrary.Models.Directory("C:\\Users\\user\\","Media"),
                new FileManagerLibrary.Models.Directory("C:\\Users\\","admin"),
                new FileManagerLibrary.Models.Directory("C:\\Users\\admin\\","Desktop"),
                new FileManagerLibrary.Models.Directory("C:\\Users\\admin\\","Downloads"),
            };
            List<FileManagerLibrary.Models.File> files = new List<FileManagerLibrary.Models.File>()
            {
                new FileManagerLibrary.Models.File()
                {Path = "C:\\Users\\user\\Media\\", Name = "MyVideo", Extension = ".mp4", TouchDate = new DateOnly(2024, 12, 15), VolumeInBits = 167772160 },
                new FileManagerLibrary.Models.File()
                {Path = "C:\\Users\\user\\Media\\", Name = "Task", Extension = ".txt", TouchDate = new DateOnly(2025, 3, 2), VolumeInBits = 1977 },
                new FileManagerLibrary.Models.File()
                {Path = "C:\\Users\\admin\\", Name = "", Extension = ".gitignore", TouchDate = new DateOnly(2024, 6, 23), VolumeInBits = 9216 },
                new FileManagerLibrary.Models.File()
                {Path = "C:\\Users\\admin\\Downloads\\", Name = "code", Extension = ".cs", TouchDate = new DateOnly(2025, 1, 21), VolumeInBits = 1560 },
            };
            directories[0].IncludesDirectories.Add(directories[1]);
            directories[1].IncludesDirectories.Add(directories[2]);
            directories[1].IncludesDirectories.Add(directories[4]);
            directories[2].IncludesDirectories.Add(directories[3]);
            directories[4].IncludesDirectories.Add(directories[5]);
            directories[4].IncludesDirectories.Add(directories[6]);
            directories[3].IncludesFiles.Add(files[0]);
            directories[3].IncludesFiles.Add(files[1]);
            directories[4].IncludesFiles.Add(files[2]);
            directories[6].IncludesFiles.Add(files[3]);
            storageContext.Directories.AddRange(directories);
            storageContext.Files.AddRange(files);
            storageContext.SaveChanges();
        }
    }
}
