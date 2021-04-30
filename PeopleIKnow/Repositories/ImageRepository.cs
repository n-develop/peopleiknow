using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PeopleIKnow.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageRepository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> WriteFileToDiskAsync(IFormFile formFile, int contactId)
        {
            var filename = contactId + formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "contacts", filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return "images/contacts/" + filename;
        }

        public string GetPathToImage(int contactId)
        {
            throw new System.NotImplementedException();
        }
    }
}