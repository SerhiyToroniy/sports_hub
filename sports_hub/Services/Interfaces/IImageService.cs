using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Interfaces
{
    public interface IImageService
    {
        public byte[] ConvertIFormFileToBytes(IFormFile image);

        public string ConvertImageBytesToString(byte[] image);
    }
}
