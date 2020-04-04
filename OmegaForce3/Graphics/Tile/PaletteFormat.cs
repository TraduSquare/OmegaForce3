using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninoimager;
using Ninoimager.Format;
using Color = Emgu.CV.Structure.Bgra;

namespace OmegaForce3.Graphics.Tile
{
    class PaletteFormat
    {

        public PaletteFormat(string file)
        {
            Stream st = new FileStream(file, FileMode.Open);
            ReadData(st);
        }

        public Color[] PaletteColors {
            get;
            set;
        }

        protected void ReadData(Stream strIn)
        {
            BinaryReader br = new BinaryReader(strIn);
            this.PaletteColors = br.ReadBytes((int)br.BaseStream.Length).ToBgr555Colors();
        }
	}
}
