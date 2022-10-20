using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using sports_hub.Models;

namespace sports_hub.Services.Implementations.AdminPage
{
    public class ImageService : IImageService
    {
        public byte[] ConvertIFormFileToBytes(IFormFile image)
        {
            byte[] fileData = null;

            using (var binaryReader = new BinaryReader(image.OpenReadStream()))
            {
                fileData = binaryReader.ReadBytes((int)image.Length);
            }

            return fileData;
        }

        public string ConvertImageBytesToString(byte[] image)
        {
            return Convert.ToBase64String(image);
        }
    }
}
