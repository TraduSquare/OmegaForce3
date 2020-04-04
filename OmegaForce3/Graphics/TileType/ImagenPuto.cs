using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texim;
using Yarhl.IO;

namespace OmegaForce3.Graphics.TileType
{
    class ImagenPuto
    {
        public string PaletteFile { get; set; }
        public string TileFile { get; set; }
        public string MapFile { get; set; }

        public ImagenPuto(string paletteFile, string tileFile, string mapFile)
        {
            PaletteFile = paletteFile;
            TileFile = tileFile;
            MapFile = mapFile;
        }

        public void GenerateImage()
        {
            var tile = GenerateTile(DataStreamFactory.FromFile(TileFile, FileOpenMode.Read));
            var map = GenerateMap(DataStreamFactory.FromFile(MapFile, FileOpenMode.Read));
            var palette = GeneratePalette(DataStreamFactory.FromFile(PaletteFile, FileOpenMode.Read));
            map.CreateBitmap(tile, palette).Save("hola2.png");
        }

        public PixelArray GenerateTile(DataStream source)
        {
            DataReader reader = new DataReader(source);
            PixelArray pixels = new PixelArray();

            var tileArray = reader.ReadBytes((int)reader.Stream.Length);
            pixels.Width = (tileArray.Length / 8);
            pixels.Height = 8;
            pixels.SetData(
                tileArray,
                PixelEncoding.HorizontalTiles,
                ColorFormat.Indexed_8bpp,
                new Size(8,8));

            return pixels;
        }

        public Palette GeneratePalette(DataStream source)
        {
            DataReader reader = new DataReader(source);
            var PaletteColors = reader.ReadBytes((int)reader.Stream.Length).ToBgr555Colors();
            return new Palette(PaletteColors);
        }

        private MapFormat GenerateMap(DataStream source)
        {
            MapFormat mapFormat = new MapFormat();
            DataReader reader = new DataReader(source);
            List<MapInfo> mapInfo = new List<MapInfo>();
            //mapFormat.Info = new MapInfo[((int)reader.Stream.Length) / 2];

            //var i = 0;
            do
            {
                mapInfo.Add(new MapInfo(reader.ReadUInt16()));
                //mapFormat.Info[i] = ();
                //i++;
            } while (!reader.Stream.EndOfStream);


            mapFormat.TileSize = new Size(8, 8);
            mapFormat.Width = 256;
            mapFormat.Height = 192;
            mapFormat.SetMapInfo(mapInfo.ToArray());

            return mapFormat;
        }
    }
}
