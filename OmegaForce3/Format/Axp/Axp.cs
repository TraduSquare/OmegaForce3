using Ninoimager.Format;
using Yarhl.FileFormat;

namespace OmegaForce3.Format.Axp
{
    public class Axp : IFormat
    {
        //Basics
        public Image Tiles { get; set; }
        public Palette Palette { get; set; }
        public AxpGroup[] Groups { get; set; }

        //Data - Thanks Pleonex
        public string Magic { get; set; }
        public ushort HorTiles { get; set; }
        public ushort VerTiles { get; set; }
        public ushort NumGroups { get; set; }
        public ushort PaletteType { get; set; }
        public int DataPos { get; set; }
        public int[] GroupPos { get; set; }

    }

    public class AxpGroup
    {
        public Map Map { get; set; }
        public sbyte NumSprites { get; set; }
        public sbyte NumTiles { get; set; }
    }
}
