using FileManagerAPI.Models;
using FileManagerLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetFromPath([FromHeader] string fullPath, [FromHeader] bool isDirectory, StorageContext storageContext)
        {
            string name = fullPath.Split(@"\").Last().ToString();
            string path = fullPath.Substring(0, fullPath.Length - name.Length);
            string extention = "";
            FileManagerLibrary.Models.Directory dir = null;
            FileManagerLibrary.Models.File file = null;
            if (isDirectory)
            {
                dir = storageContext.Directories.Include(d => d.IncludesDirectories).Include(d => d.IncludesFiles).FirstOrDefault(d => d.Name == name && d.Path == path);
                if(dir == null) 
                    return NotFound();
            }
            else
            {
                extention = name.Split(".").Last().ToString();
                name = name.Substring(0, name.Length - (extention.Length + 1));
                file = storageContext.Files.FirstOrDefault(f => f.Name == name && f.Path == path);
                if(file == null) 
                    return NotFound();
            }
            IStorageable result = null;
            if (file != null)
                result = file;
            if (dir != null)
            {
                dir.IncludesFiles.AddRange(storageContext.Files.Where(f => f.Path == fullPath));
                dir.IncludesDirectories.AddRange(storageContext.Directories.Where(d => d.Path == fullPath));
                result = dir;
            }
            if (result.IsDirectory) return Json((FileManagerLibrary.Models.Directory)result);
            else return Json((FileManagerLibrary.Models.File)result);
        }
    }
}
