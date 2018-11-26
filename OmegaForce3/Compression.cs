// Copyright (C) 2018 Darkmet98
//
// This file is part of OmegaForce3.
//
// OmegaForce3 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General public static License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OmegaForce3 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General public static License for more details.
//
// You should have received a copy of the GNU General public static License
// along with OmegaForce3. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Diagnostics;
using System.IO;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace OmegaForce3
{
    public static class Compression
    {
        private static readonly String COMP_PATH_WIN = @"\lib\NDS_Comp_CUE\";
        private static readonly String COMP_PATH_UNIX = "/lib/NDS_Comp_CUE/";
        public static DataStream DecompressLzx(DataStream file)
        {
            string tempFile = Path.GetTempFileName();
            file.WriteTo(tempFile);

            string program = System.IO.Path.GetFullPath(@"../../") + COMP_PATH_WIN + "lzx.exe";

            string arguments = "-d " + tempFile;

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                program = System.IO.Path.GetFullPath(@"../../") + COMP_PATH_UNIX + "lzx";
            }

            Process process = new Process();
            process.StartInfo.FileName = program;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            process.WaitForExit();

            DataStream streamNew = new DataStream(tempFile, FileOpenMode.Read);

            File.Delete(tempFile);

            return streamNew;
        }
    }
}
