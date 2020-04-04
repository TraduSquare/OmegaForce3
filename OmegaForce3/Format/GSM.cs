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
using System.Linq;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace OmegaForce3.Format {

    public class GSM {

        //Dump the text from the file
        private void Dumptext(BinaryReader reader, int o) {
            for (int a = 0; a < Values.sizes[o]; a++) {
                Values.rawbytes = reader.ReadUInt16(); //Read Two bytes

                if (Values.istext) { //Is a dialog box
                    if (Values.ingame.TryGetValue(Values.rawbytes, out string result) || Values.variables.TryGetValue(Values.rawbytes, out result)) { //Is a ingame value or header value
                        Values.text.Add(result);
                        if (result == "[Dialog]") {
                            Values.istext = false;
                            Values.isname = true;
                        }
                    }
                    else {
                        byte[] array = BitConverter.GetBytes(Values.rawbytes);
                        Values.result = Encoding.Unicode.GetString(array);
                        Values.text.Add(Values.result.Replace("_", " "));
                    }
                }
                else if (Values.isname) { //Chara name
                    if (Values.var_names.TryGetValue(Values.rawbytes, out string result)) { //Get the name
                        Values.text.Add(result);
                    }
                    Values.isname = false;
                    Values.istext = true;
                }
                else { //Header values
                    if (Values.variables.TryGetValue(Values.rawbytes, out string result)) { //Is a header value
                        if (result == "[Dialog]") {
                            Values.isname = true;
                        }
                        Values.text.Add(result);
                    }
                    else { //DEBUG
                        Console.WriteLine("AVISO, LA LÍNEA " + a + " ESTÁ FALLANDO");
                    }
                }
            }
            //Values.text.Add("[END]");
            Values.istext = false;
        }

        private void ParseList(Po po, int o) {
            int e = 0;
            for (int i = 0; i < Values.text.Count(); i++) {
                if (Values.istext && Values.text[i] != "[New_Dialog_Box]") { //Ingame text
                    if (Values.variables.ContainsValue(Values.text[i]) || Values.var_names.ContainsValue(Values.text[i])) { //Check is a ingame value or name
                        Values.istext = false;
                    }
                    else { //Add the text
                        Values.original.Add(Values.text[i]);
                    }
                }
                else if (!Values.istext && Values.text[i] != "[New_Dialog_Box]") { //Values
                    if (Values.variables.ContainsValue(Values.text[i]) || Values.var_names.ContainsValue(Values.text[i])) { //Header values or Names
                        Values.var.Add(Values.text[i]);
                    }

                    if (Values.var_names.ContainsValue(Values.text[i])) { //Names
                        Values.names = Values.text[i];
                        Values.istext = true;
                    }
                }
                if (Values.text[i] == "[New_Dialog_Box]" || Values.text[i] == "[IG_Unk_6]") { //Is a new dialog box
                    //DIRTY BUT I NEED INVESTIGATE THIS
                    if (Values.text[i] == "[IG_Unk_6]")
                        Values.original.Add(Values.text[i]);
                    PoGenerate(po, e, o); //New Entry
                    e++;
                }
            }
        }

        private void PoGenerate(Po po, int e, int o) {
            PoEntry entry = new PoEntry(); //New entry
            string jointext = String.Join("", Values.original.ToArray()); //Join all text
            Values.original.Clear(); //Clear the list

            //SI OCURRE ESTO, ALGO ESTÁ FALLANDO EN EL CODIGO
            if (string.IsNullOrEmpty(jointext))
                jointext = "<!FAIL>";

            entry.Original = jointext; //Original text
            string joinvar = String.Join("/", Values.var.ToArray()); //Join all values
            Values.var.Clear(); //Clear the list

            entry.Reference = joinvar; //Values
            entry.Context = o.ToString() + ":" + e.ToString(); //Block and line
            entry.ExtractedComments = Values.names; //Name

            po.Add(entry); //New entry
        }

        public void Export(string file) {
            System.Console.WriteLine("Exporting " + file + " to po");

            //Po header
            Po po = new Po {
                Header = new PoHeader("Megaman Starforce 3", "glowtranslations@gmail.com", "es") {
                    LanguageTeam = "GlowTranslations & Transcene",
                }
            };

            //Cleaning the lists
            Values.positions.Clear();
            Values.sizes.Clear();

            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open))) {
                Values.magic = reader.ReadInt32(); //Read magic
                Values.Unknown1 = reader.ReadInt16(); //Read a Unknown value
                Values.blockcounter = reader.ReadInt16(); //Read the number of blocks on the file
                Values.blockmaxsize = reader.ReadInt16(); //Read the biggest block on the file
                reader.BaseStream.Position = reader.BaseStream.Position + 0x02; //Skip

                for (int i = 0; i < Values.blockcounter; i++) {
                    Values.positionblock = reader.ReadInt16(); //Read the position
                    Values.positions.Add(Values.positionblock); //Add the position on a list
                    Values.sizeblock = reader.ReadInt16(); //Read the size
                    Values.sizes.Add(Values.sizeblock); //Add the size on a list
                }

                for (int o = 0; o < Values.positions.Count; o++) { //Check the all file
                    Values.text.Clear();
                    if (Values.sizes[o] == 0) { //White block
                        PoEntry entry = new PoEntry();
                        entry.Original = "[NULL]";
                        entry.Reference = "[NULL]";
                        entry.Context = o.ToString() + ":" + "NULL_DIALOG_BOX";
                        entry.ExtractedComments = "[NULL]";
                        po.Add(entry);
                    }
                    else {
                        reader.BaseStream.Position = Values.positions[o]; //Get the position
                        Dumptext(reader, o); //Dump the text
                        ParseList(po, o); //Parse the text and export to po

                        /*for (i = 0; i < Values.text.Count(); i++)
                        {
                            Console.Write(Values.text[i]);
                        }*/
                    }
                }
                //Write po
                //po.ConvertTo<BinaryFormat>().Stream.WriteTo(file + ".po");
            }
        }
    }
}