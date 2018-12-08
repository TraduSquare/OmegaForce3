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

        //PO
        public static List<string> var = new List<string>(); //Variables
        public static string names = ""; //Names
        public static List<string> original = new List<string>(); //Original Text

        //TEXT
        public static UInt16 rawbytes; //Bytes
        public static List<string> text = new List<string>(); //Text
        public static bool isname = false; //Is name
        public static bool dialogenabled = false; //Dialog start

        public static Dictionary<UInt16, string> variables = new Dictionary<UInt16, string>() {
            //Variables
            {0x0001, "[END_VAR]"},
            {0xEC00, "[Conversation]"},
            {0x0000, "[BLANK]"},
            {0xEC01, "[Dialog]"},
            {0xE211, "[Unknown1]"},
            {0x0002, "[Unknown2]"},
            {0x0003, "[Unknown3]"},
            {0x003C, "[Unknown4]"},
            {0xE213, "[Unknown5]"},
            {0xE210, "[Unknown6]"},
            {0xEC16, "[Unknown7]"},
            {0x0084, "[Unknown8]"},
            {0xE212, "[Unknown9]"},
            {0x001C, "[Unknown10]"},
            {0xEC02, "[New_Dialog_Box]"},
            {0xE10A, "[TOUCHSCREEN]"},//RESEARCH THIS
            {0x0284, "[IG_Unk_6]"}, //RESEARCH THIS
        };

        public static Dictionary<UInt16, string> ingame = new Dictionary<UInt16, string>() {
            //Text effect variables
            {0xEC13, "[Efe1]"},
            {0xE113, "[Efe2]"},
            {0xE114, "[Efe3]"},
            {0xE115, "[Efe4]"},
            {0xE116, "[Efe5]"},
            //Color names
            {0xE240, "[Virus_Busting-color]"},
            {0xE242, "[Mobile_Terminal-color]"},
            {0xE243, "[EM_Body-color]"},
            {0xE244, "[Visualizer-color]"},
            {0xE245, "[Virus-color]"},
            {0xE249, "[Wave_Road-color]"},
            {0xE250, "[Hunter-VG-color]"},
            {0xE255, "[Mega-color]"},
            {0xE256, "[Prez-color]"},
            {0xE257, "[Bud-color]"},
            {0xE258, "[Zack-color]"},
            {0xE25F, "[Kelvin-color]"},
            //Sound Variables
            {0xE206, "[Buzz_1]"},
            {0xE210, "[Buzz_2]"},
            {0x03B4, "[Buzz_3]"},
            {0xEC16, "[Buzz_4]"},
            //Terminology
            {0xE4A6, "[Hunter]"},
            {0xE570, "[Visualizer]"},
            //Another or unknown Variables
            {0xE10A, "[TOUCHSCREEN]"},
            {0x0284, "[IG_Unk_6]"},
        };

        public static Dictionary<UInt16, string> var_names = new Dictionary<UInt16, string>() {
            //Names
            {0x0000, "[NONAME]"},
            {0x0014, "[Geo]"},
            {0x0017, "[Prez]"},
            {0x0018, "[Bud]"},
            {0x0019, "[Zack]"},
            {0x001D, "[Jack]"},
            {0x001E, "[(Queen)Tia]"},
            {0x0028, "[Mary.McLovin]"},
            {0x0035, "[Geo_Visualizer]"},
            {0x0039, "[Mr_Shepar]"},
            {0x0065, "[Mega]"},
        };
    }
}
