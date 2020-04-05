using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaForce3.Properties;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Format.Bin
{
    public class Bin : IFormat
    {
        public List<uint> Positions { get; set; }
        public List<ushort> Sizes { get; set; }
        public List<ushort> Types { get; set; }
        public List<byte[]> Blocks { get; set; }
        public List<uint> Magics { get; set; }
        public int Count { get; set; }
        /*
         * 00 = Normales
         * 32768 = Comprimido con Lzx
         */

        public Bin()
        {
            Positions = new List<uint>();
            Sizes = new List<ushort>();
            Types = new List<ushort>();
            Blocks = new List<byte[]>();
            Magics = new List<uint>();
        }

        //Compression - Thanks CUE
        public byte[] Lzx(byte[] file, string argument)
        {
            var tempFile = Path.GetTempFileName();
            var program = Path.GetTempFileName() + ".exe";
            File.WriteAllBytes(tempFile, file);
            File.WriteAllBytes(program, Resources.lzx);

            // "-d " Export
            // "-evb " Import

            var arguments = argument + tempFile;

            /*if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                program = Path.GetFullPath(@"../../") + COMP_PATH_UNIX + "lzx";
            }*/

            var process = new Process
            {
                StartInfo =
                {
                    FileName = program,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();

            process.WaitForExit();

            var streamNew = File.ReadAllBytes(tempFile);

            File.Delete(tempFile);
            File.Delete(program);

            return streamNew;
        }
    }
}
