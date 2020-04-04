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

namespace OmegaForce3
{
    class Encryption
    {
        public void Decrypt(string file, byte[] array, bool istext)
        {

            Values.headersize = 0xC + (array[6] * 0x4); //The header size
            //Make a stream from the file
            using (BinaryWriter writer = new BinaryWriter(File.Open(file + ".exported", FileMode.Create))) //Make a decrypted file
            {
                System.Console.WriteLine("Decrypting " + file);
                if (istext) {
                    for (int i = 0; i < Values.headersize; i++) //Make a bucle to write the header
                    {
                        writer.Write((byte)(array[i])); //Write the no encrypted data
                    }
                    for (int i = Values.headersize; i < array.Length; i++) //Make a bucle reading the encrypted data
                    {
                        writer.Write((byte)(array[i] ^ Values.key)); //Decrypt
                    } 
                }
                else {
                    for (int i = 0; i < array.Length; i++) //Make a bucle reading the encrypted data
                    {
                        writer.Write((byte)(array[i] ^ Values.key)); //Decrypt
                    } 
                }
            }
        }
    }
}
