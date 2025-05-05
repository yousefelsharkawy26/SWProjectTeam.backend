namespace DentalManagementSystem.Services.Interfaces;
public interface IFileServices
{
    string UploudFile(IFormFile File, string oldImage);
    bool RemoveFile(string FileName);
}
