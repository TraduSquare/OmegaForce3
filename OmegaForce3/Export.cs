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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace OmegaForce3 {
    class Export {
        public void Extract(string file) {
            OmegaForce3.Encryption encryption = new OmegaForce3.Encryption();
            OmegaForce3.Format.GSM extracting = new OmegaForce3.Format.GSM();
            int i = 0;
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open))) { //Open the file
                for (i = 0; i < reader.BaseStream.Length; i++) { //Read all file because the file doesn't have a counter.
                    Values.position = reader.ReadInt32(); //Read position
                    Values.positions.Add(Values.position); //Add the position on a list
                    Values.size = reader.ReadInt16(); //Read the size
                    Values.sizes.Add(Values.size); //Add the size on a list
                    Values.type = reader.ReadUInt16(); //Read the type
                    Values.types.Add(Values.type); //Add the type on a list
                    if (Values.position == 0x00000000 || Values.size == 0x0000) { //If position or size get a blank int32, close the bucle
                        break;
                    } 
                }

                for (i = 0; i < Values.positions.Count-1; i++ ) {
                    System.Console.WriteLine("Extracting " + file + "." + i.ToString());
                    reader.BaseStream.Position = Values.positions[i]; //Get the position
                    Values.magic = reader.ReadInt32(); //Get the magic
                    reader.BaseStream.Position = Values.positions[i]; //Get back to the position
                    byte[] array = reader.ReadBytes(Values.sizes[i]); //Get the byte array

                    // *** QUitamos el encrypted

                    if (Values.types[i] == 0x0) { //Encrypted
                        if (Values.magic == 0x4D534720) { //Text file
                            Values.istext = true;
                        }
                        else { //Graphic??
                            Values.istext = false;
                        }
                        encryption.Decrypt(file + "." + i.ToString(), array, Values.istext); //Decrypt the file
                    }
                    else if (Values.types[i] == 0x8000) { //Compressed
                        Extract(file + "." + i.ToString(), array); //Test, only for extract
                    }
                    //else {
                        
                    //}
                }
            }
            //Extract the text - WIP, DOESN'T WORK
            /*for(i = 0; i < Values.positions.Count - 1; i++ ) {
                extracting.Export(file + "." + i.ToString()); //Extract the text

            }*/
        }
        //Temporal - only for extract files
        public void Extract(string file, byte[] array) {

            using (BinaryWriter writer = new BinaryWriter(File.Open(file + ".exportedtest", FileMode.Create))) { //Make a decrypted file
                for (int i = 0; i < array.Length; i++) //Make a bucle to write the header
                {
                    writer.Write((byte)(array[i])); //Write the no encrypted data
                }
            }
        }

        public DataStream Decompress(string file)
        {
            using(DataStream stream = new DataStream(file, FileOpenMode.Read)){

                DataReader reader = new DataReader(stream);

                reader.Stream.PushCurrentPosition();

                if (IsCompressed(reader.ReadByte()))
                {
                    reader.Stream.PopPosition();
                    return Compression.DecompressLzx(stream);
                }
                else
                {
                    throw new Exception("File is not compressed!");
                }
            }
        }

        private readonly byte COMPRESSED_HEADER = 0x11;

        private bool IsCompressed(byte firstByte){
            return firstByte == COMPRESSED_HEADER;
        }
    }
}
