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
using Yarhl.IO;
using Yarhl.Media.Text;
using Yarhl.FileFormat;
using System.IO;
using System.Text;
using System.Linq;

namespace OmegaForce3.Format
{
    public class GSM
    {
        public void Export(string file){
        

            System.Console.WriteLine("Exporting " + file + " to po");

            //Po header
            Po po = new Po
            {
                Header = new PoHeader("Megaman Starforce 3", "glowtranslations@gmail.com", "es")
                {
                    LanguageTeam = "GlowTranslations & Transcene",
                }
            };

            //Cleaning the lists
            Values.positions.Clear();
            Values.sizes.Clear();
            int i = 0;
            int o = 0;

            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                Values.magic = reader.ReadInt32(); //Read magic
                Values.Unknown1 = reader.ReadInt16(); //Read a Unknown value
                Values.blockcounter = reader.ReadInt16(); //Read the number of blocks on the file
                Values.blockmaxsize = reader.ReadInt16(); //Read the biggest block on the file
                reader.BaseStream.Position = reader.BaseStream.Position + 0x02; //Skip

                for (i = 0; i < Values.blockcounter; i++) {
                    Values.positionblock = reader.ReadInt16(); //Read the position
                    Values.positions.Add(Values.positionblock); //Add the position on a list
                    Values.sizeblock = reader.ReadInt16(); //Read the size
                    Values.sizes.Add(Values.sizeblock); //Add the size on a list
                }
                //int a = 0;
                for (o = 0; o < Values.positions.Count; o++) {
                    reader.BaseStream.Position = Values.positions[o]; //Get the position
                    byte[] array = reader.ReadBytes(Values.sizes[o] * 2); //Get the size
                    //SHITTY CODE, DOESN'T WORK
                    /*for (i = 0; i < array.Length; i++)
                    {
                        if (Values.istext) {
                            if (array[i] == 0x02 && array[i + 1] == 0xEC)
                            {
                                Values.istext = false;
                                Values.textx2.RemoveAt(0);
                                //Values.textx2.RemoveAt(1);
                                byte[] arrayX = Values.textx2.ToArray();
                                //Array.Reverse(arrayX);
                                Values.result = Encoding.BigEndianUnicode.GetString(arrayX);
                                Values.textx.Add(Values.result);
                                Values.textx2.Clear();
                            }
                            else
                            {
                                Values.textx2.Add(array[i]);
                            }
                        }
                        else {
                            if (array[i] == 0x01 && array[i + 1] == 0xEC) {
                                Values.istext = true;
                                //int e = BitConverter.ToInt32(Values.header, 0);
                                //Values.headerx.AddRange(e);
                            }
                            else {
                                Values.header = new byte[] { array[i] };
                            }
                        }
                    }
                    //byte[] array2 = new byte[Values.text.GetLength(0)];
                    for (i = 0; i < Values.textx.Count(); i++) {
                        Console.WriteLine(Values.textx[i]);
                        }*/
                }
            }
        }
    }
}
