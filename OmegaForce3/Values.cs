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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaForce3
{
    public class Values
    {
        public static Int32 key = 0x55; //Xor Key — Thanks Pleonex
        public static Int16 Unknown1; //Unknown value, the value always is 01
        public static Int16 blockcounter; //Number of block counters
        public static Int16 blockmaxsize; //The biggest block
        public static Int16 positionblock; //Block position
        public static Int16 sizeblock; //Block size
        public static Int32 position; //The position to read the file block
        public static Int16 size; //The file size
        public static UInt16 type; //Encrypted, compresed...
        public static bool istext = false; //Is text
        public static Int32 headersize; //The file size
        public static Int32 magic; // Magic
        public static List<int> sizes = new List<int>(); //List sizes
        public static List<int> positions = new List<int>(); //List positions
        public static List<int> types = new List<int>(); //List positions
        public static string result = "";


        //TEXT VALUES - WIP
        public static Int32 StartBlock;
        public static Int16 TypeBlock;
        public static Int16 Name;
        public static byte[] text;
        public static byte[] header;
        public static List<string> textx = new List<string>();
        public static List<Int32> headerx = new List<int>();
        public static List<byte> textx2 = new List<byte>();
        public static List<Int32> headerx2 = new List<int>();
        public static string Name_string;

        public static Dictionary<Int16, string> names = new Dictionary<Int16, string>() {
            {0x1D00, "Jack"},
            {0x1E00, "(Queen)Tia"},
        };

        public static Dictionary<Int16, string> flags = new Dictionary<Int16, string>() {
            {0x01EC, "Start dialog"},
            {0x13EC, "Efecto 2"},
        };

    }
}
