using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Interfaces
{
    public interface IImageConverter
    {
        public byte[] imgToByteArray(Image img);

        public Image byteArrayToImage(byte[] byteArrayIn);
    }
}