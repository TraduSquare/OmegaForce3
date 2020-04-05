// Copyright (C) 2018 Darkmet98
//
// This file is part of OmegaForce3.
//
// OmegaForce3 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General public static License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OmegaForce3 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General public static License for more details.
//
// You should have received a copy of the GNU General public static License
// along with OmegaForce3. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using OmegaForce3.Format.Bin;
using OmegaForce3.Format.BinContainer;
using OmegaForce3.Graphics.TileType;
using OmegaForce3.Text;
using Yarhl.FileSystem;
using Yarhl.Media.Text;

namespace OmegaForce3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OmegaForce3 — A MegaMan StarForce 3 toolkit for fantranslations by Darkmet98.\nThanks to Pleonex for Yarhl libraries and decryption.\nVersion: 1.0");
            if (args.Length != 1 && args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("\nUsage: OmegaForce3.exe <-decrypt/-encrypt");
                Console.WriteLine("Decrypt files: OmegaForce3.exe -decrypt \"file \"");
                return;
            }
            switch (args[0])
            {
                case "-unpack":
                    //export.Extract(args[1]);
                    // 1
                    var nod = NodeFactory.FromFile(args[1]); // BinaryFormat

                    // 2
                    Node nodBin = nod.TransformWith<Binary2Bin>();

                    // 3
                    Node nodoContainer = nodBin.TransformWith<Bin2Container>();

                    //4
                    var PathFolder = Path.GetFileNameWithoutExtension(args[1]);
                    if (!Directory.Exists(PathFolder)) Directory.CreateDirectory(PathFolder);

                    foreach (var child in Navigator.IterateNodes(nodoContainer))
                    {
                        if (child.Stream == null)
                            continue;
                        string output = Path.Combine(PathFolder + "/" + child.Name);
                        child.Stream.WriteTo(output);
                    }
                    break;

                case "-pack":
                    Node nodeFolder = NodeFactory.FromDirectory(args[1], "*.*");

                    // 2
                    Node nodoTbl = nodeFolder.TransformWith<Container2Bin>();

                    // 3
                    Node nodoBin = nodoTbl.TransformWith<Bin2Binary>();

                    //4
                    nodoBin.Stream.WriteTo(args[1] + "_new.bin");
                    break;
                case "-importtext":
                    var nodo = NodeFactory.FromFile(args[1]);
                    nodo.TransformWith<Binary2Po>();

                    Node nodoRtp = nodo.TransformWith<Po2Gsm>();

                    // 3
                    Node nodoBinary = nodoRtp.TransformWith<Gsm2Binary>();

                    //4
                    nodoBinary.Stream.WriteTo(args[1] + "_new.bin");
                    break;
                case "-exportgraphics":
                    /*var nodoPal = NodeFactory.FromFile("0001.dat");
                    var nodoTile = NodeFactory.FromFile("0002.dat");
                    var nodoMap = NodeFactory.FromFile("0003.dat");

                    Map map = nodoMap.TransformWith<Binary2Map>().GetFormatAs<Map>();

                    Palette pal = nodoPal.TransformWith<Binary2Palette>().GetFormatAs<Palette>();

                    var tile = nodoTile.TransformWith<Binary2TileFormat>().GetFormatAs<TileFormat>();

                    map.CreateBitmap(tile.Pixels, pal).Save("hola2.png");
                    */
                    //string path = Path.Combine(outputPath, nDIG.Name + ".png");
                    ImagenPuto hola = new ImagenPuto("0001.dat", "0002.dat", "0003.dat");
                    hola.GenerateImage();
                    //Convert2Image hola = new Convert2Image("0001.dat", "0002.dat", "0003.dat");
                    //hola.GenerateFinalImage();
                    break;
            }
        }
    }
}
