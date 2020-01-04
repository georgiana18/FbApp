using System.IO;
using System.Linq;
using FbApp.Models;
using Microsoft.AspNetCore.Http;

namespace FbApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public PhotoService()
        {
        }

        public int Create(IFormFile photo, string userId)
        {
            using (var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);

                var instanceOfPhoto = new Photo
                {
                    PhotoAsBytes = memoryStream.ToArray(),
                    UserId = userId
                };

                db.Photos.Add(instanceOfPhoto);
                db.SaveChanges();

                return instanceOfPhoto.Id;
            }
        }

        public byte[] PhotoAsBytes(IFormFile photo)
        {
            byte[] photoAsBytes;
            using (var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);
                photoAsBytes = memoryStream.ToArray();
            }
            return photoAsBytes;
        }

        public bool PhotoExists(int photoId) => this.db.Photos.Any(p => p.Id == photoId);
    }
}