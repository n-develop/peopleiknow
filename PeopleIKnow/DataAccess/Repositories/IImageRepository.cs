using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PeopleIKnow.DataAccess.Repositories
{
    public interface IImageRepository
    {
        Task<string> WriteFileToDiskAsync(IFormFile file, int contactId);
        string GetPathToImage(int contactId);
    }
}