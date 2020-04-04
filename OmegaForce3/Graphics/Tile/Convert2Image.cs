using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Color = Emgu.CV.Structure.Bgra;
using Ninoimager;
using Ninoimager.Format;
using Yarhl.IO;
using Image = Ninoimager.Format.Image;

namespace OmegaForce3.Graphics.Tile
{
    class Convert2Image
    {
        public string PaletteFile { get; set; }
        public string TileFile { get; set; }
        public string MapFile { get; set; }
        public ImageFormat ImageFormat { get; set; }
        private DataReader Reader { get; set; }


        public Convert2Image(string paletteFile, string tileFile, string mapFile)
        {
            PaletteFile = paletteFile;
            TileFile = tileFile;
            MapFile = mapFile;
            ImageFormat = new ImageFormat();

            GeneratePalette();
            GenerateTile();
            GenerateMap();
        }


        private void GeneratePalette()
        {
            Stream st = new FileStream(PaletteFile, FileMode.Open);
            Color[] PaletteColors;
            BinaryReader br = new BinaryReader(st);
            PaletteColors = br.ReadBytes((int)br.BaseStream.Length).ToBgr555Colors();
            ImageFormat.Palette = new Palette(PaletteColors);
        }

        private void GenerateMap()
        {
            Reader = new DataReader(DataStreamFactory.FromFile(MapFile, FileOpenMode.Read));
            //MapInfo[] map = new MapInfo[((int)Reader.Stream.Length)/2];

            List<MapInfo> map = new List<MapInfo>();

            //var i = 0;
            do {
                map.Add(new MapInfo(Reader.ReadUInt16()));
                //i++;
            } while (!Reader.Stream.EndOfStream);

            ImageFormat.Map = new Map();
            var arr = map.ToArray();
            ImageFormat.Map.SetMapInfo(arr);
            ImageFormat.Map.TileSize = new Size(8,8);
            ImageFormat.Map.Width = arr.Length / 8;
            ImageFormat.Map.Height = 8;
        }

        private void GenerateTile()
        {

            ImageFormat.Tile = new TileFormat(TileFile);

        }

        public void GenerateFinalImage()
        {
            ImageFormat.Map.CreateBitmap(ImageFormat.Tile, ImageFormat.Palette).Save("hola.png");
        }
    }
}
