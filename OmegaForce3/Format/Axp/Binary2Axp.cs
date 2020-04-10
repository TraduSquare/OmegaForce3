using System;
using System.Collections.Generic;
using System.Drawing;
using Ninoimager;
using Ninoimager.Format;
using Yarhl.FileFormat;
using Yarhl.IO;
using Image = Ninoimager.Format.Image;

namespace OmegaForce3.Format.Axp
{
    public class Binary2Axp : IConverter<BinaryFormat,Axp>
    {
        private DataReader Reader { get; set; }
        private Axp Axp { get; }

        public Binary2Axp()
        {
            Axp = new Axp();
        }

        public Axp Convert(BinaryFormat source)
        {
            Reader = new DataReader(source.Stream);
            ReadHeader();
            Axp.Tiles = ReadTiles();
            ReadAxpGroup();
            Axp.Palette = GeneratePalette();

            for (int i = 0; i < Axp.NumGroups; i++)
            {
                Axp.Groups[i].Map.CreateBitmap(Axp.Tiles, Axp.Palette).Save("holax"+i+".png");
            }

            return Axp;
            //throw new NotImplementedException();
        }


        private void ReadHeader()
        {
            Axp.Magic = Reader.ReadString(4);
            //if(Axp.Magic != " AXP" || Axp.Magic != "iAXP") throw new FormatException();
            
            Reader.Stream.Position += 2;
            
            Axp.HorTiles = Reader.ReadUInt16();
            Axp.VerTiles = Reader.ReadUInt16();
            Axp.NumGroups = Reader.ReadUInt16();
            
            Reader.Stream.Position += 2;
            
            Axp.PaletteType = Reader.ReadUInt16();
            Axp.DataPos = Reader.ReadInt32();

            Reader.Stream.Position += 4;
            Axp.GroupPos = new int[Axp.NumGroups];
            for (var i = 0; i < Axp.NumGroups; i++)
            {
                Axp.GroupPos[i] = Reader.ReadInt32();
            }
        }

        private void ReadAxpGroup()
        {
            Axp.Groups = new AxpGroup[Axp.NumGroups];

            for (var i = 0; i < Axp.NumGroups; i++)
            {
                var size = ((((i + 1) != Axp.NumGroups) ? Axp.GroupPos[i + 1] : Axp.DataPos)-4);
                
                Reader.Stream.Position = Axp.GroupPos[i];
                Axp.Groups[i] = new AxpGroup();


                Axp.Groups[i].NumSprites = Reader.ReadSByte();
                Axp.Groups[i].NumTiles = Reader.ReadSByte();
                Axp.Groups[i].Map = GetMap(size);
            }
        }

        private Map GetMap(int count)
        {
            var result = new Map();

            var map = new List<MapInfo>();

            for (var i = 0; i < count; i+=2)
            {
                map.Add(new MapInfo(Reader.ReadUInt16()));
            }

            var arr = map.ToArray();

            result.TileSize = new Size(8, 8);
            result.Width = arr.Length / Axp.HorTiles;
            result.Height = 8;
            result.SetMapInfo(arr);

            return result;
        }

        private Image ReadTiles()
        {
            var result = new Image();

            var array = Reader.ReadBytes((int) (Reader.Stream.Length - Axp.DataPos));

            //result.Width = 
            int numPixels = array.Length * Axp.HorTiles;

            result.Height = Axp.VerTiles;
            result.Width = numPixels;
            
            result.SetData(array,
            PixelEncoding.HorizontalTiles,
            (Axp.PaletteType == 0)?ColorFormat.Indexed_4bpp:ColorFormat.Indexed_8bpp,
            new Size(8,8)
            );

            return result;
        }

        private Palette GeneratePalette()
        {
            var result = new Palette();
            var random = new Random();
            byte[] array = new byte[0x200];
            random.NextBytes(array);
            result.SetPalette(array.ToBgr555Colors());
            return result;
            // if (Axp.PaletteType == 0) array = new byte[];
        }
    }
}
