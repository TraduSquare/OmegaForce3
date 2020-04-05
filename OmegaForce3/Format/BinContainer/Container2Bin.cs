using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaForce3.Format.Bin;
using OmegaForce3.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OmegaForce3.Format.BinContainer
{
    public class Container2Bin : IConverter<NodeContainerFormat, Bin.Bin>
    {
        private Bin.Bin Bin { get; }

        public Container2Bin()
        {
            Bin = new Bin.Bin();
        }
        
        public Bin.Bin Convert(NodeContainerFormat source)
        {
            Bin.Count = source.Root.Children.Count;
            foreach (var child in source.Root.Children)
            {
                Bin.Blocks.Add(CheckFile(child));
            }

            return Bin;
        }


        private byte[] CheckFile(Node node)
        {
            if(node.Name.Contains(".po")) return ConvertGsm(node);
            return ConvertGeneric(node);
        }

        private byte[] ConvertGsm(Node node)
        {
            var stream = node.TransformWith<Binary2Po>().TransformWith<Po2Gsm>().TransformWith<Gsm2Binary>().Stream;
            byte[] array = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(array, 0, (int)stream.Length);
            Bin.Sizes.Add((ushort)array.Length);
            Bin.Magics.Add(0x4D534720);
            Bin.Types.Add(0);
            return Bin2Container.Decrypt(array, true);
            //Let's try to compress the all files
            //Bin.Types.Add(32768);
            //return Bin.CompressLzxText(Bin2Container.Decrypt(array,true));
        }

        private byte[] ConvertGeneric(Node node)
        {
            byte[] array = new byte[(int)node.Stream.Length];
            node.Stream.Read(array, 0, (int)node.Stream.Length);
            return array;
        }
    }
}
