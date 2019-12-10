using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FbApp.Services
{
    public interface IPhotoService
    {
        int Create(IFormFile photo, string userId);

        bool PhotoExists(int photoId);

        byte[] PhotoAsBytes(IFormFile photo);
    }
}