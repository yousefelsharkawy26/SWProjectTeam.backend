using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Services;
public class FileServices: IFileServices
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FileServices> _logger;
    public FileServices(IWebHostEnvironment env, ILogger<FileServices> logger)
    {
        _env = env;
        _logger = logger;
    }

    public string UploudFile(IFormFile File, string oldImage)
    {
        
        try
        {
            string wwwRootPath = _env.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
            string filePath = wwwRootPath + "\\images\\";

            RemoveFile(oldImage);

            using (var fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                File.CopyTo(fs);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error On uploading image {ex.Message}");
        }
    }

    public bool RemoveFile(string FileName)
    {
        try
        {
            if (string.IsNullOrEmpty(FileName) && FileName != "avatar.png")
                return false;

            // delete the old image
            var oldImagePath = _env.WebRootPath + "\\images\\" + FileName;

            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            return true;
        }
        catch { return false; }
    }
}
