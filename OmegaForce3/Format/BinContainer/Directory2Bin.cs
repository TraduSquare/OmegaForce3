using System.IO;
using OmegaForce3.Format.Bin;
using OmegaForce3.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OmegaForce3.Format.BinContainer
{
    class Directory2Bin : IConverter<BinaryFormat, Bin.Bin>
    {
        private string Directory { get; }
        public Bin.Bin Bin { get; set; }

        public Directory2Bin()
        {
        }

        public Directory2Bin(string directory)
        {
            Directory = directory;
        }

        public Bin.Bin Convert(BinaryFormat source)
        {
            return Bin;
        }


        public Bin.Bin GenerateBin()
        {
            Bin = new Bin.Bin();
            var info = File.ReadAllLines(Directory + Path.DirectorySeparatorChar + "Header.ome");
            
            foreach (var line in info)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var file = line.Split('|');
                if (!File.Exists(Directory + Path.DirectorySeparatorChar + file[0])) throw new FileNotFoundException();
                var type = System.Convert.ToUInt16(file[1]);
                var array = File.ReadAllBytes(Directory + Path.DirectorySeparatorChar + file[0]);
                Bin.Types.Add(type);

                Bin.Blocks.Add(CheckFile(array, file[0],
                    type));

            }

            Bin.Count = Bin.Blocks.Count;
            return Bin;
        }

        private byte[] CheckFile(byte[] file, string name, ushort type)
        {
            var check = type == 32768;
            if (name.Contains(".po")) return ConvertGsm(GenerateNode(file, name),check);


            Bin.Sizes.Add((ushort)file.Length);

            return (type == 32768) ? CompressFile(file) : file;
        }

        private byte[] ConvertGsm(Node node, bool compressed)
        {
            var stream = node.TransformWith<Binary2Po>().TransformWith<Po2Gsm>().TransformWith<Gsm2Binary>().Stream;
            byte[] array = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(array, 0, (int)stream.Length);
            Bin.Sizes.Add((ushort)array.Length);
            Bin.Magics.Add(0x4D534720);
            
            if (compressed)
            {
                return CompressFile(Bin2Container.Decrypt(array,true));
            }

            return Bin2Container.Decrypt(array, true);
        }

        private byte[] CompressFile(byte[] file)
        {
            return Bin.Lzx(file, "-evb ");
        }

        private Node GenerateNode(byte[] bytes, string name)
        {
            var node = NodeFactory.FromMemory(name);
            node.Stream.Write(bytes,0,bytes.Length);
            node.Stream.Position = 0;
            return node;
        }
    }
}
