using System;
using System.IO;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Format.Bin
{
    public class Binary2Bin : IConverter<BinaryFormat, Bin>
    {
        private Bin Bin { get; }
        private DataReader Reader { get; set; }

        public Binary2Bin()
        {
            Bin = new Bin();
        }

        public Bin Convert(BinaryFormat source)
        {
            Reader = new DataReader(source.Stream);
            ReadHeader();
            DumpBlocks();

            return Bin;
        }

        private void ReadHeader()
        {
            uint pos;
            do
            {
                pos = Reader.ReadUInt32();
                
                if (pos == (int)Reader.Stream.Length) continue;
                
                Bin.Positions.Add(pos);
                Bin.Sizes.Add(Reader.ReadUInt16());
                Bin.Types.Add(Reader.ReadUInt16());

            } while (pos != (int)Reader.Stream.Length);

            Bin.Count = Bin.Positions.Count;
        }

        private void DumpBlocks()
        {
            for (int i = 0; i < Bin.Count; i++)
            {
                Reader.Stream.Position = Bin.Positions[i];
                var block = Reader.ReadBytes((Bin.Types[i] != 32768)?Bin.Sizes[i]: //Not compressed
                    (i != Bin.Count-1)?(int)(Bin.Positions[i+1]-Bin.Positions[i]):(int)(Reader.Stream.Length - Bin.Positions[i])); //Compressed
                if (Bin.Types[i] == 32768)
                {
                    File.WriteAllBytes(i+".comp",block);
                    block = Bin.Lzx(block, "-d ");
                }
                Bin.Magics.Add(ReadMagic(block));
                Bin.Blocks.Add(block);

            }
        }

        private uint ReadMagic(byte[] block)
        {
            return BitConverter.ToUInt32(block, 0);
            /*Reader.Stream.PushCurrentPosition();
            var result = Reader.ReadUInt32();
            Reader.Stream.PopPosition();
            return result;*/
        }
    }
}
