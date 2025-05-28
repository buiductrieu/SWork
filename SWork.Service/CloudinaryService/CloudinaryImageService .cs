using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SWork.ServiceContract.ICloudinaryService;

namespace SWork.Service.CloudinaryService
{
    public class CloudinaryImageService : ICloudinaryImageService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryImageService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;
            var account = new Account(
                settings.Cloudname,
                settings.ApiKey,
                settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Cloudinary upload failed: {result.Error?.Message}");
            }

            return result.SecureUrl.ToString();
        }

        public string ExtractPublicIdFromUrl(string url)
        {

            if (string.IsNullOrEmpty(url)) return null;

            var uri = new Uri(url);
            var segments = uri.Segments
                              .Select(s => s.Trim('/'))
                              .ToList();

            // find location of "upload"
            int uploadIndex = segments.FindIndex(s => s == "upload");
            if (uploadIndex == -1) return null;

            // Skip "upload" + "v..."
            var publicIdSegments = segments.Skip(uploadIndex + 2); // skip "upload" and "v123456..."

            // Combined to publicId
            var publicIdWithExt = string.Join("/", publicIdSegments);

            // skip extend (.png, .jpg,...)
            int dotIndex = publicIdWithExt.LastIndexOf('.');
            return dotIndex >= 0 ? publicIdWithExt.Substring(0, dotIndex) : publicIdWithExt;
        }

        public async Task DeleteImageAsync(string publicId)
        {

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult == null)
                throw new Exception("Không nhận được phản hồi từ Cloudinary khi xóa ảnh.");

            if (deletionResult.Result != "ok")
                throw new Exception($"Xóa ảnh thất bại (PublicId: {publicId}): {deletionResult.Error?.Message}");

            if (deletionResult.Result != "ok")
                throw new System.Exception("Xóa ảnh thất bại: " + deletionResult.Error?.Message);
        }
    
    }
}
