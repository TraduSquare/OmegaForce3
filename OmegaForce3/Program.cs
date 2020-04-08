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
using System.Reflection;
using OmegaForce3.Format.Bin;
using OmegaForce3.Format.BinContainer;
using OmegaForce3.Format.Spr;
using OmegaForce3.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OmegaForce3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OmegaForce3 — A MegaMan StarForce 3 toolkit for fantranslations by Darkmet98." +
                              "\nThanks to Pleonex for Yarhl libraries and decryption." +
                              "\nThanks to CUE for Lzx compression." +
                              "\nVersion: 1.0");
            if (args.Length != 1 && args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("\nUsage: OmegaForce3.exe <-unpack/-pack> <file/folder>");
                Console.WriteLine("Unpack files: OmegaForce3.exe -unpack \"mess_game.bin \"");
                Console.WriteLine("Pack files: OmegaForce3.exe -pack \"mess_game \"");
                return;
            }
            switch (args[0])
            {
                case "-unpack":
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
                    var generate = new Directory2Bin(args[1]);
                    var bin = generate.GenerateBin();
                    Node nodeBin = NodeFactory.FromMemory("test");
                    nodeBin.TransformWith(generate).TransformWith<Bin2Binary>().Stream.WriteTo(args[1] + "_new.bin");
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
                    var nodoPal = NodeFactory.FromFile(args[1]);
                    var Bin2Spr = new Binary2Spr();
                    Bin2Spr.GenerateImage(new BinaryFormat(nodoPal.Stream), Path.GetFileNameWithoutExtension(args[1]));
                    break;
            }
        }
    }
}
