using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texim;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Graphics.TileType
{
    class Binary2TileFormat : IConverter<BinaryFormat, TileFormat>
    {
        public Palette Palette { get; set; }
        public TileFormat Tile { get; set; }

        public TileFormat Convert(BinaryFormat source)
        {
            DataReader reader = new DataReader(source.Stream);
            Tile = new TileFormat();


            Tile.Pixels = new PixelArray();
            
            var tileArray = reader.ReadBytes((int) reader.Stream.Length);
            Tile.Pixels.Width = tileArray.Length / 8;
            Tile.Pixels.Height = 8;
            Tile.Pixels.SetData(
                tileArray,
                PixelEncoding.HorizontalTiles,
                ColorFormat.Indexed_8bpp,
                new Size(8,8));

            return Tile;
        }
    }
}
