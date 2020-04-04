using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace OmegaForce3.Text
{
    public class Gsm : IFormat
    {
        //Values
        public string Magic { get; set; }
        public short Id { get; set; }
        public short BlockCount { get; set; }
        public short BlockMaxSize { get; set; } //I think this is incorrect

        //Arrays
        public short[] Positions { get; set; }
        public short[] Sizes { get; set; }
        public string[] Lines { get; set; }
        public byte[][] Blocks { get; set; }

        //Dictionary
        public Dictionary<ushort, string> DictionaryStrings;
        public Dictionary<ushort, string> DictionaryNames;

        public void InitializeArrays()
        {
            Positions = new short[BlockCount];
            Sizes = new short[BlockCount];
            Lines = new string[BlockCount];
            Blocks = new byte[BlockCount][];
        }

        public void ReadDictionary()
        {
            try
            {
                DictionaryStrings = new Dictionary<ushort, string>();
                var dictionary = System.IO.File.ReadAllLines("dic.txt");
                foreach (string line in dictionary)
                {
                    string[] lineFields = line.Split('=');
                    DictionaryStrings.Add(ushort.Parse(lineFields[0], System.Globalization.NumberStyles.HexNumber), lineFields[1]);
                }

                DictionaryNames = new Dictionary<ushort, string>();
                dictionary = System.IO.File.ReadAllLines("chara.txt");
                foreach (string line in dictionary)
                {
                    string[] lineFields = line.Split('=');
                    DictionaryNames.Add(ushort.Parse(lineFields[0], System.Globalization.NumberStyles.HexNumber), lineFields[1]);
                }
            }
            catch (Exception e)
            {
                Console.Beep();
                Console.WriteLine(@"The dictionary is wrong, please, check the readme and fix it. Press any Key to continue.");
                Console.WriteLine(e);
                Console.ReadKey();
                System.Environment.Exit(-1);
            }

        }
    }

}
