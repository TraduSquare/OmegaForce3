using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninoimager.Format;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OmegaForce3.Format.Spr
{
    public class SprOam : Sprite
    {
        public Frame[] Frames { get; set; }
        public int[] Positions { get; set; }


        private DataReader Reader { get; set; }
        public void ReadOam(DataReader reader)
        {
            Reader = reader;
            ReadPositions();
            ReadOam();
            SetFrames(Frames);
        }

        private void ReadPositions()
        {
            int difference = (int)Reader.Stream.Position;
            int count = Reader.ReadInt32();

            Frames = new Frame[count];
            Positions = new int[count];

            for (int i = 0; i < count; i++)
            {
                Positions[i] = Reader.ReadInt32() + difference;
            }
        }

        private void ReadOam()
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                var nextPosition = ((i + 1) != Positions.Length) ? Positions[i + 1] : Reader.Stream.Length;
                Reader.Stream.Position = Positions[i];
                
                Frames[i] = new Frame();
                var oams = new List<Obj>();

                var j = 0;
                do
                {
                    var oam = ReadOamInfo(Reader.ReadBytes(8));
                    oam.Id = (ushort)j;
                    oams.Add(oam);
                    j++;
                } while (Reader.Stream.Position != nextPosition);

                Frames[i].SetObjects(oams.ToArray());
            }
        }

        private Obj ReadOamInfo(byte[] array)
        {
            Obj obj = new Obj();

            // X Y
            obj.CoordY = (sbyte)(array[2]);
            obj.CoordX = (sbyte)array[1];

            //Flips
            if (array[5] != 0)
            {
                obj.HorizontalFlip = (array[5] == 1 || array[5] == 3);
                obj.VerticalFlip = (array[5] == 2 || array[5] == 3);
            }

            //Palette
            obj.PaletteMode = PaletteMode.Palette16_16;
            obj.PaletteIndex = array[7];

            //Sizes
            Size size;
            switch (array[3])
            {
                case 0:
                    size = new Size(16, 8);
                    break;
                case 1:
                    size = new Size(32, 8);
                    break;
                case 2:
                    size = new Size(32, 16);
                    break;
                case 3:
                    size = new Size(64, 32);
                    break;
                default:
                    size = new Size(4, 2);
                    break;

            }
            obj.SetSize(size);

            //Shape
            obj.Shape = (ObjShape)array[4];

            //Priority
            obj.ObjPriority = array[6];

            //Position
            obj.TileNumber = (ushort)(array[0]);

            return obj;
        }
    }
}
