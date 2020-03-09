using System;
using System.Collections.Generic;
using System.Text;

namespace Square_1NN.Square1
{
    class ColoredRearLayer : RearLayer
    {
        string major_color, major_side_color, minor_color, minor_side_color;
        public string MajorColor { get => major_color; set => major_color = value; }
        public string MajorSideColor { get => major_side_color; set => major_side_color = value; }
        public string MinorColor { get => minor_color; set => minor_color = value; }
        public string MinorSideColor { get => minor_side_color; set => minor_side_color = value; }

        protected ColoredRearLayer() : base() { }
        public ColoredRearLayer(string mask) : base(Convert(mask))
        {
            string[] split = mask.Split();
            StringBuilder top = new StringBuilder(), side = new StringBuilder();
            for (int index = 0; index < split[0].Length; index += 2)
            {
                char code = split[0][index];
                if (char.ToLower(code) != 'y' && char.ToLower(code) != 'w')
                {
                    side.Append(code);
                    index--;
                }
                else if (char.IsUpper(code))
                {
                    top.Append(char.ToLower(code));
                    side.Append(split[0][index + 1]);
                    if (index + 2 < split[0].Length) side.Append(split[0][index + 2]);
                    index++;
                }
                else
                {
                    top.Append(code);
                    side.Append(split[0][index + 1]);
                }
            }
            major_color = top.ToString();
            major_side_color = side.ToString();
            top.Clear();
            side.Clear();
            for (int index = 0; index < split[1].Length; index += 2)
            {
                char code = split[1][index];
                if (char.ToLower(code) != 'y' && char.ToLower(code) != 'w')
                {
                    side.Append(code);
                    index--;
                }
                else if (char.IsUpper(code))
                {
                    top.Append(char.ToLower(code));
                    side.Append(split[1][index + 1]);
                    if (index + 2 < split[1].Length) side.Append(split[1][index + 2]);
                    index++;
                }
                else
                {
                    top.Append(code);
                    side.Append(split[1][index + 1]);
                }
            }
            minor_color = top.ToString();
            minor_side_color = side.ToString();
            if (Major[0] == '3') MajorColor = MinorColor[MinorColor.Length - 1] + MajorColor;
            for (int index = 1; index < Major.Length; index++)
            {
                if (Major[index] == '3')
                {
                    MajorColor = MajorColor.Substring(0, index - 1) + MajorColor[index - 1] + MajorColor.Substring(index - 1);
                }
            }
            if (Minor[0] == '3') MinorColor = MajorColor[MajorColor.Length - 1] + MinorColor;
            for (int index = 1; index < Minor.Length; index++)
            {
                if (Minor[index] == '3')
                    MinorColor = MinorColor.Substring(0, index - 1) + MinorColor[index - 1] + MinorColor.Substring(index - 1);
            }
        }
        private static string Convert(string mask)
        {
            string[] split = mask.Split();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < split[0].Length; index += 2)
            {
                if (char.ToLower(split[0][index]) != 'y' && char.ToLower(split[0][index]) != 'w')
                {
                    builder.Append("3");
                    index--;
                }
                else if (char.IsLower(split[0][index])) builder.Append('2');
                else if (index == split[0].Length - 2)
                {
                    builder.Append("1");
                    break;
                }
                else
                {
                    builder.Append("13");
                    index++;
                }
            }
            builder.Append(' ');
            for (int index = 0; index < split[1].Length; index += 2)
            {
                if (char.ToLower(split[1][index]) != 'y' && char.ToLower(split[1][index]) != 'w')
                {
                    builder.Append("3");
                    index--;
                }
                else if (char.IsLower(split[1][index])) builder.Append('2');
                else if (index == split[1].Length - 2)
                {
                    builder.Append("1");
                    break;
                }
                else
                {
                    builder.Append("13");
                    index++;
                }
            }
            Console.WriteLine(builder.ToString());
            return builder.ToString();
        }
        public override void Shift(bool forward)
        {
            (string, string, string, string) backup = (MajorColor, MajorSideColor, MinorColor, MinorSideColor);
            int count = forward ? 1 : -1;

            MajorSideColor = ShiftString(backup.Item2, backup.Item4, count);
            MinorSideColor = ShiftString(backup.Item4, backup.Item2, count);
            MajorColor = ShiftString(backup.Item1, backup.Item3, count);
            MinorColor = ShiftString(backup.Item3, backup.Item1, count);
            //if (forward)
            //{
            //    if (Major[0] != '3')
            //    {
            //        MinorColor = MinorColor + MajorColor[0];
            //        MajorColor = MajorColor.Substring(1);
            //    }
            //    if (Minor[0] != '3')
            //    {
            //        MajorColor = MajorColor + MinorColor[0];
            //        MinorColor = MinorColor.Substring(1);
            //    }
            //}
            //else
            //{
            //    if (Minor[Minor.Length - 1] != '3')
            //    {
            //        MajorColor = MinorColor[MinorColor.Length - 1] + MajorColor;
            //        MinorColor = MinorColor.Substring(0, MinorColor.Length - 1);
            //    }
            //    if (Major[Major.Length - 1] != '3')
            //    {
            //        MinorColor = MajorColor[MajorColor.Length - 1] + MinorColor;
            //        MajorColor = MajorColor.Substring(0, MajorColor.Length - 1);
            //    }
            //}
            base.Shift(forward);
        }
        public override void Reverse(bool forMajor)
        {
            base.Reverse(forMajor);
            if (forMajor)
            {
                Reverse(ref major_color);
                Reverse(ref major_side_color);
            }
            else
            {
                Reverse(ref minor_color);
                Reverse(ref minor_side_color);
            }
        }
        public override LayerBase Clone()
        {
            return new ColoredRearLayer()
            {
                major = Major,
                minor = Minor,
                major_color = MajorColor,
                minor_color = MinorColor,
                major_side_color = MajorSideColor,
                minor_side_color = MinorSideColor,
            };
        }
    }
}
