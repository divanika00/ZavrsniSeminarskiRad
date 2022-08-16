using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface IFileSaveService
    {
        Task<FileSaveViewModel> AddFileToStorage(IFormFile file);
        Task<bool> DeleteFile(int id);
        bool DeletePhysicalFile(string filePath);
        Task<FileSaveExpendedViewModel> GetFile(long id);
    }
}