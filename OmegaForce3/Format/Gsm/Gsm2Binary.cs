using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Text
{
    public class Gsm2Binary : IConverter<Gsm, BinaryFormat>
    {

        private DataWriter Writer { get; set; }
        private Gsm Gsm { get; set; }

        public BinaryFormat Convert(Gsm source)
        {
            Gsm = source;
            Writer = new DataWriter(new DataStream());
            WriteHeader();
            WriteBlock();
            UpdateHeader();
            return new BinaryFormat(Writer.Stream);
        }

        private void WriteHeader()
        {
            Writer.Write(Gsm.Magic,false);
            Writer.Write(Gsm.Id);
            Writer.Write(Gsm.BlockCount);
            Writer.Write(Gsm.BlockMaxSize);
            Writer.Write((ushort)0xFFFF);

            Writer.WriteTimes(0,(Gsm.BlockCount*4));
        }

        private void WriteBlock()
        {
            var i = 0;
            foreach (var block in Gsm.Blocks)
            {
                Gsm.Positions[i] = (ushort)Writer.Stream.Position;
                if (block != null)
                {
                    Writer.Write(block);
                }
                Writer.WriteTimes(0, 2);
                i++;
            }
        }


        private void UpdateHeader()
        {
            Writer.Stream.Position = 0xC;
            for (int i = 0; i < Gsm.BlockCount; i++)
            {
                Writer.Write(Gsm.Positions[i]);
                Writer.Write(Gsm.Sizes[i]);
            }
        }
    }
}
