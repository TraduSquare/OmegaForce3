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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaForce3.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.Media.Text;

namespace OmegaForce3
{
    class Program
    {
        static void Main(string[] args)
        {
            OmegaForce3.Export export = new OmegaForce3.Export();
            OmegaForce3.Format.GSM extract1 = new OmegaForce3.Format.GSM();
            OmegaForce3.Format.Graphic extract2 = new OmegaForce3.Format.Graphic();
            Console.WriteLine("OmegaForce3 — A MegaMan StarForce 3 toolkit for fantranslations by Darkmet98.\nThanks to Pleonex for Yarhl libraries and decryption.\nVersion: 1.0");
            if (args.Length != 1 && args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("\nUsage: OmegaForce3.exe <-decrypt/-encrypt");
                Console.WriteLine("Decrypt files: OmegaForce3.exe -decrypt \"file \"");
                return;
            }
            switch (args[0])
            {
                case "-decrypt":
                    export.Extract(args[1]);
                    break;
                case "-exporttext":
                    //extract1.Export(args[1]);
                    
                    break;
                case "-exportgraphics":
                    //extract2.(args[1]);
                    break;
                case "-decompress":
                    export.Decompress(args[1]).WriteTo(args[1] + ".decompressed");
                    break;
            }
        }
    }
}
