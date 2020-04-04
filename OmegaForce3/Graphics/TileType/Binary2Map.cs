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
    class Binary2Map : IConverter<BinaryFormat, MapFormat>
    {
        public MapFormat Convert(BinaryFormat source)
        {
            MapFormat mapFormat = new MapFormat();
            DataReader reader = new DataReader(source.Stream);

            mapFormat.Info = new MapInfo[((int)reader.Stream.Length) / 2];

            var i = 0;
            do
            {
                mapFormat.Info[i] = (new MapInfo(reader.ReadUInt16()));
                i++;
            } while (!reader.Stream.EndOfStream);


            mapFormat.TileSize = new System.Drawing.Size(8, 8);
            mapFormat.Width = 256;
            mapFormat.Height = 192;
            mapFormat.SetMapInfo(mapFormat.Info);

            return mapFormat;
        }
    }
}
