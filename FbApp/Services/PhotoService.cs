using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace FbApp.Services
{
    public class PhotoService : IPhotoService
    {
        public int Create(IFormFile photo, string userId)
        {
            throw new NotImplementedException();
        }

        public byte[] PhotoAsBytes(IFormFile photo)
        {
            throw new NotImplementedException();
        }

        public bool PhotoExists(int photoId)
        {
            throw new NotImplementedException();
        }
    }
}