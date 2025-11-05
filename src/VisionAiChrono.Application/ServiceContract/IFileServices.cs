using Microsoft.AspNetCore.Http;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IFileServices
    {

        Task<string> CreateFile(IFormFile file);

        Task DeleteFile(string? imageUrl);

        Task<string> UpdateFile(IFormFile newFile, string? currentFileName);
    }
}
