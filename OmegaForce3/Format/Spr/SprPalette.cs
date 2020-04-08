using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Ninoimager;
using Ninoimager.Format;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Format.Spr
{
    public class SprPalette : IFormat
    {
        public Palette Palette { get; set; }
        public ushort Type { get; set; }
        public ushort Count { get; set; }


        public void ReadPalette(DataReader reader, int length)
        {
            /*Type = reader.ReadUInt16();
            if(Type != 5) throw new Exception("Unsupported palette");
            Count = reader.ReadUInt16();
            */
            reader.Stream.Position += 4;
            var colors = new Bgra[1][];

            //var read = 0x1C * Count;

            colors[0] =  reader.ReadBytes(length).ToBgr555Colors();

            Palette = new Palette(colors);
        }

    }
}
