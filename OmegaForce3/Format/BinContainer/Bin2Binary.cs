using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Format.BinContainer
{
    public class Bin2Binary : IConverter<Bin.Bin, BinaryFormat>
    {
        private Bin.Bin Bin { get; set; }
        private DataWriter Writer { get; set; }

        public BinaryFormat Convert(Bin.Bin source)
        {
            Bin = source;
            Writer = new DataWriter(new DataStream());
            WriteWhiteHeader();
            WriteContent();
            UpdateHeader();

            return new BinaryFormat(Writer.Stream);
        }

        private void WriteWhiteHeader()
        {
            Writer.WriteTimes(0, 8*(Bin.Count+1));

        }

        private void WriteContent()
        {
            for (int i = 0; i < Bin.Count; i++)
            {
                Bin.Positions.Add((uint)Writer.Stream.Position);
                Writer.Write(Bin.Blocks[i]);
            }

        }

        private void UpdateHeader()
        {
            Writer.Stream.Position = 0;
            for (int i = 0; i < Bin.Count; i++)
            {
                Writer.Write(Bin.Positions[i]);
                Writer.Write(Bin.Sizes[i]);
                Writer.Write(Bin.Types[i]);
            }
            Writer.Write((uint)Writer.Stream.Length);
        }
    }
}
