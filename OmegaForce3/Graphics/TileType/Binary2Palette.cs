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
    class Binary2Palette : IConverter<BinaryFormat, Palette>
    {
        public Palette Convert(BinaryFormat source)
        {
            DataReader reader = new DataReader(source.Stream);
            var PaletteColors = reader.ReadBytes((int)reader.Stream.Length).ToBgr555Colors();
            return new Palette(PaletteColors);
        }
    }
}
