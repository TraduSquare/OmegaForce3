using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texim;
using Yarhl.FileFormat;

namespace OmegaForce3.Format.Spr
{
    public class Spr : IFormat
    {
        public SprTile Tiles { get; set; }
        public SprPalette Palette { get; set; }
        public SprOam Oam { get; set; }

        public Spr()
        {
            Tiles = new SprTile();
            Palette = new SprPalette();
            Oam = new SprOam();
        }

    }
}
