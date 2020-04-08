using System.IO;
using Yarhl.IO;

namespace OmegaForce3.Format.Spr
{
    public class Binary2Spr
    {
        public void GenerateImage(BinaryFormat source, string name)
        {
            DataReader reader = new DataReader(source.Stream);

            var posOr = reader.ReadInt32();
            var posPalette = reader.ReadInt32();
            var unknown = reader.ReadInt32();
            var posOam = reader.ReadInt32();

            reader.Stream.Position = posOr;

            Spr spr = new Spr();

            spr.Tiles.ReadTile(reader, posOr, posPalette);

            reader.Stream.Position = posPalette;
            spr.Palette.ReadPalette(reader, (unknown - posPalette)-4);

            reader.Stream.Position = posOam;
            spr.Oam.ReadOam(reader);

            if (!Directory.Exists(name)) Directory.CreateDirectory(name);

            var count = spr.Oam.Frames.Length;

            for (int i = 0; i < count; i++)
            {
                spr.Oam.CreateBitmap(i, spr.Tiles.Tiles, spr.Palette.Palette).Save(name + Path.DirectorySeparatorChar+i+".png");
            }

        }
    }
}
