using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PeopleIKnow.Repositories
{
    public interface IImageRepository
    {
        Task<string> WriteFileToDiskAsync(IFormFile file, int contactId);
        string GetPathToImage(int contactId);
    }
}