using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace OmegaForce3.Text
{
    public class Po2Gsm : IConverter<Po, Gsm>
    {
        private Gsm Gsm { get; }
        private Po Po { get; set; }

        public Po2Gsm()
        {
            Gsm = new Gsm();
        }

        public Gsm Convert(Po source)
        {
            Po = source;
            WriteHeader();
            Gsm.InitializeArrays();
            Gsm.ReadDictionary();
            GenerateBlocks();
            return Gsm;
        }


        private void WriteHeader()
        {
            Gsm.Magic = " GSM";
            Gsm.Id = 256;
            Gsm.BlockCount = (short)Po.Entries.Count;
        }

        private void GenerateBlocks()
        {
            var i = 0;
            short blockSizeMax = 0;
            foreach (var entry in Po.Entries)
            {
                if (entry.Original != "[NULL]")
                {
                    var line = (!string.IsNullOrEmpty(entry.Translated)) ? entry.Translated : entry.Original;
                    var block = GenerateBlock(ConvertTags(line));
                    if (block.Length > blockSizeMax) blockSizeMax = (short)block.Length;
                    Gsm.Sizes[i] = (short)block.Length;
                    Gsm.Blocks[i] = block.SelectMany(BitConverter.GetBytes).ToArray();
                }
                else
                {
                    Gsm.Sizes[i] = 0;
                    Gsm.Blocks[i] = null;
                }

                i++;
            }
            Gsm.BlockMaxSize = blockSizeMax;
        }

        private ushort[] GenerateBlock(string line)
        {
            List<ushort> block = new List<ushort>();
            char[] array = line.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                switch (array[i])
                {
                    case '{':
                        string bytes = "";
                        while (array[i] != '}')
                        {
                            i++;
                            bytes += array[i].ToString();
                        }

                        bytes = bytes.Replace("{", "").Replace("}", "");
                        block.Add(System.Convert.ToUInt16(bytes, 16));
                        break;
                    default:
                        block.Add(GetChar(array[i].ToString()));
                        break;
                }
            }
            return block.ToArray();

        }

        private string ConvertTags(string line)
        {
            var result = line;
            result = result.Replace("\n[DIALOG_START]", "{" + System.Convert.ToString(0xEC01, 16) + "}");
            result = result.Replace("\n[NEW_DIALOG]\n", "{" + System.Convert.ToString(0xEC02, 16) + "}");
            foreach (var entry in Gsm.DictionaryNames)
            {
                result = result.Replace(entry.Value+"\n", "{" + System.Convert.ToString(entry.Key, 16) + "}");
            }

            return result;
        }

        private ushort GetChar(string c)
        {
            c = (c == "\n") ? "\\n" : c;
            foreach (var entry in Gsm.DictionaryStrings)
            {
                if (c == entry.Value) return entry.Key;
            }
            return 0;
        }


    }
}
