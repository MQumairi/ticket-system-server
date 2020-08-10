using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Images
{
    public class PhotoAccessor
    {
        private readonly Cloudinary cloudinary;

        public PhotoAccessor(IOptions<CloudinarySettings> config)
        {
            var acc = new Account (
                config.Value.cloudName,
                config.Value.apiKey,
                config.Value.apiSecret
            );

            this.cloudinary = new Cloudinary(acc);
        }

        PhotoUploadResult AddPhoto(IFormFile file) {

            var uploadResult = new ImageUploadResult();
            
            if(file.Length > 0) {
                using(var stream = file.OpenReadStream()) { 

                    var uploadParams = new ImageUploadParams{
                        File = new FileDescription(file.FileName, stream)
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            return new PhotoUploadResult{
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri
            };
        }
    }
}