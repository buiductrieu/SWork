
using Microsoft.AspNetCore.Http;

namespace SWork.ServiceContract.ICloudinaryService
{
    public interface ICloudinaryImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
        string ExtractPublicIdFromUrl(string url); // find image in cloudinary by url
        Task DeleteImageAsync(string publicId); // delete image in cloudinary

    }
}
