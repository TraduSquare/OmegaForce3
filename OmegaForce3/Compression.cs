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
namespace OmegaForce3
{
    public class Compression
    {
      private static final String COMP_PATH_WIN = @"\lib\NDS_Comp_CUE\";
      private static final String COMP_PATH_UNIX = "/lib/NDS_Comp_CUE/";

      public static Node DecompressLzx(Node node)
      {
          string tempFile = Path.GetTempFileName();

          using (var substream = new DataStream(node.Stream, 4, node.Stream.Length - 4))
          {
              substream.WriteTo(tempFile);
          }

          string program = COMP_PATH_WIN + "lzs.exe";

          string arguments = "-d " + tempFile;
          if (Environment.OSVersion.Platform != PlatformID.Win32NT)
          {
              program = COMP_PATH_UNIX + "lzs";
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

          DataStream fileStream = new DataStream(tempFile, FileOpenMode.Read);
          DataStream memoryStream = new DataStream();
          fileStream.WriteTo(memoryStream);

          fileStream.Dispose();
          File.Delete(tempFile);

          return new Node(node.Name, new BinaryFormat(memoryStream));
      }
    }
}
