using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaForce3.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OmegaForce3.Format.Bin
{
    public class Bin2Container : IConverter<Bin, NodeContainerFormat>
    {
        public NodeContainerFormat Convert(Bin source)
        {
            NodeContainerFormat container = new NodeContainerFormat();

            for (int i = 0; i < source.Count; i++)
            {
                container.Root.Add(CheckFile(source.Blocks[i], source.Magics[i], i));
            }

            return container;

        }



        private Node CheckFile(byte[] file,uint magic, int i)
        {
            if (magic == 0x4D534720) return ConvertGsm(Decrypt(file, true), i); //Text file
            return ConvertGeneric(file, i);
        }

        private Node ConvertGsm(byte[] file, int i)
        {
            return NodeFactory.FromSubstream(i.ToString().PadLeft(2, '0') + ".po",DataStreamFactory
                    .FromArray(file,0,file.Length),0,file.Length)
                .TransformWith<Binary2Gsm>().TransformWith<Gsm2Po>().TransformWith<Po2Binary>();
        }

        private Node ConvertGeneric(byte[] file, int i)
        {
            Node child = NodeFactory.FromMemory(i.ToString().PadLeft(2, '0') + ".bin");
            child.Stream.Write(file, 0, file.Length);
            return child;
        }

        public static byte[] Decrypt(byte[] array, bool istext)
        {
            var key = 0x55; //Xor Key — Thanks Pleonex
            
            var result = array;

            var size = (istext) ? 0xC + (BitConverter.ToUInt16(array, 6) * 0x4): 0;

            for (int i = size; i < result.Length; i++)
            {
                result[i] = (byte)(result[i] ^ key);//Decrypt
            }

            return result;
        }
    }
}
