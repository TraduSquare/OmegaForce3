using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninoimager;
using Ninoimager.Format;
using Image = Ninoimager.Format.Image;

namespace OmegaForce3.Graphics.Tile
{
    class TileFormat : Image
    {
        public TileFormat(string file)
        {
            this.GetInfo(file);
        }

        private void GetInfo(string file)
        {


            Stream st = new FileStream(file, FileMode.Open);
            Byte[] arrayFile;
            BinaryReader br = new BinaryReader(st);
            arrayFile = br.ReadBytes((int) br.BaseStream.Length);

            this.Width = 256;
            this.Height = 192;
            
            this.SetData(arrayFile, PixelEncoding.HorizontalTiles, ColorFormat.Indexed_8bpp);
        }
    }
}
