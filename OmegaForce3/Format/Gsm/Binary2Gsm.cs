using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OmegaForce3.Text
{
    public class Binary2Gsm : IConverter<BinaryFormat, Gsm>
    {
        private DataReader Reader { get; set; }
        private Gsm Gsm { get; set; }

        public Binary2Gsm()
        {
            Gsm = new Gsm();
        }

        public Gsm Convert(BinaryFormat source)
        {
            Reader = new DataReader(source.Stream)
            {
                DefaultEncoding = Encoding.Default,
                Endianness = EndiannessMode.LittleEndian
            };
            Gsm.ReadDictionary();
            DumpData();
            GenerateEntries();
            return Gsm;
        }

        private void DumpData()
        {
            Gsm.Magic = Reader.ReadString(4);
            if(Gsm.Magic != " GSM") throw new FormatException("This is not a Text file");
            Gsm.Id = Reader.ReadInt16();
            Gsm.BlockCount = Reader.ReadInt16();
            Gsm.BlockMaxSize = Reader.ReadInt16();
            Reader.Stream.Position += 2;

            Gsm.InitializeArrays();

            for (var i = 0; i < Gsm.BlockCount; i++)
            {
                Gsm.Positions[i] = Reader.ReadUInt16();
                Gsm.Sizes[i] = Reader.ReadInt16();
            }
        }


        private int eCount { get; set; }
        private void GenerateEntries()
        {
            for (int i = 0; i < Gsm.BlockCount; i++)
            {
                string line = "";
                if (Gsm.Sizes[i] == 0) line = "[NULL]";
                else
                {
                    Reader.Stream.Position = Gsm.Positions[i];
                    eCount = 0;
                    do
                    {
                        line += ReturnString(Reader.ReadUInt16());
                        eCount++;
                    } while (eCount != Gsm.Sizes[i]);
                }

                Gsm.Lines[i] = line;
            }
        }

        private string ReturnString(ushort val)
        {
            if (val == 0xEC01) return GetCharaName();

            if (Gsm.DictionaryStrings.TryGetValue(val, out string result))
                return result;
            return "{" + System.Convert.ToString(val, 16) + "}";
        }

        private string GetCharaName()
        {
            string line = "\n[DIALOG_START]";
            eCount++;
            ushort val = Reader.ReadUInt16();
            if (Gsm.DictionaryNames.TryGetValue(val, out string result))
                line += result + "\n";
            else
                line+= "{" + System.Convert.ToString(val, 16) + "}";

            return line;
        }
    }
}
