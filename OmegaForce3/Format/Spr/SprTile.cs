using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninoimager.Format;
using OmegaForce3.Format.Spr;
using Yarhl.FileFormat;
using Yarhl.IO;
using Image = Ninoimager.Format.Image;

namespace OmegaForce3.Format.Spr
{

    public class Tile : Image
    {
        public Tile()
        {

        }
    }
}

public class SprTile : IFormat
{
    public Tile Tiles { get; set; }
    public ushort Count { get; set; }

    public void ReadTile(DataReader reader, int posOr, int posPal)
    {

        Tiles = new Tile();
        reader.Stream.Position +=4;
        reader.Stream.Position = (reader.ReadInt32() + posOr);
        var arraySize = (int)(posPal - reader.Stream.Position); 

        var array = reader.ReadBytes(arraySize);
        Count = (ushort)(array.Length / 32);

        int numPixels = array.Length * 8 / 4;

        Tiles.Width = 8;
        Tiles.Height = numPixels / Tiles.Width;
        Tiles.SetData(
            array,
            PixelEncoding.HorizontalTiles,
            ColorFormat.Indexed_4bpp,
            new Size(8, 8));
    }
}
