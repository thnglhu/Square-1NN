using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1
{
    public class Cube
    {
        static ColoredRearLayer TOP = new ColoredRearLayer("yrYrgygYgo yoYobybYbr");
        static ColoredMiddleLayer MIDDLE = new ColoredMiddleLayer("Rgo Obr");
        static ColoredRearLayer BOTTOM = new ColoredRearLayer("wrWrgwgWgo woWobwbWbr");
        static Texture2D[] small_top;
        static Texture2D[] small_bottom;
        static Texture2D[] small_top_side;
        static Texture2D[] small_bottom_side;
        static Texture2D[] large_top;
        static Texture2D[] large_bottom;
        static Texture2D[] large_top_side;
        static Texture2D[] large_bottom_side;
        static Texture2D[] middle_side;
        static Texture2D[] middle_top;
        static bool init = false;
        readonly LayerBase[] layers;
        List<(Texture2D, Color, Vector2)> prepare = null;
        Controller controller;
        Dictionary<char, Color> keys = new Dictionary<char, Color>()
        {
            ['w'] = Color.White,
            ['y'] = Color.Yellow,
            ['r'] = Color.Red,
            ['g'] = Color.LawnGreen,
            ['b'] = Color.DeepSkyBlue,
            ['o'] = Color.Orange,
        };
        public Cube(Controller controller)
        {
            Init(this.controller = controller);            
            layers = new LayerBase[3];
            layers[0] = TOP.Clone();
            layers[1] = MIDDLE.Clone();
            layers[2] = BOTTOM.Clone();
        }
        public Cube(Controller controller, string mask) : this(controller)
        {
            string[] split = mask.Split();
            layers[0] = new ColoredRearLayer(split[0] + " " + split[1]);
            layers[1] = new ColoredMiddleLayer(split[2] + " " + split[3]);
            layers[2] = new ColoredRearLayer(split[4] + " " + split[5]);
        }
        public void Display()
        {
            if (prepare == null) Prepare();
            prepare.ForEach(packed => controller.DrawTexture(packed.Item1, packed.Item2, packed.Item3));
        }
        private void DisplayLower(Vector2 vector2, string extract, string side_color)
        {
            extract = extract.Substring(6) + extract.Substring(0, 6);
            side_color = side_color.Substring(6) + side_color.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = extract[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                {
                    prepare.Add((small_bottom_side[pos], keys[side_color[subindex % side_color.Length]], vector2));
                }
                else if (code == 1)
                {
                    prepare.Add((large_bottom_side[2 * pos], keys[side_color[subindex % side_color.Length]], vector2));
                }
                else
                {
                    prepare.Add((large_bottom_side[(2 * pos + 23) % 24], keys[side_color[subindex % side_color.Length]], vector2));
                }
                code = extract[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                {
                    prepare.Add((small_bottom_side[pos], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
                else if (code == 1)
                {
                    prepare.Add((large_bottom_side[2 * pos], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
                else
                {
                    prepare.Add((large_bottom_side[(2 * pos + 23) % 24], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
            }
            int index = 6;
            foreach (char chr in extract)
            {
                int code = chr - '0';
                if (code == 2)
                {
                    prepare.Add((small_bottom[index], Color.White, vector2));
                }
                else if (code == 1)
                {
                    prepare.Add((large_bottom[index], Color.White, vector2));
                }
                index = (index + 1) % 12;
            }
        }
        

        private void DisplayMiddle(Vector2 vector2, string extract, string side_color)
        {
            if (extract[0] == '4')
            {
                prepare.Add((middle_side[0], keys[side_color[0]], vector2));
                prepare.Add((middle_top[0], Color.White, vector2));
            }
            else
            {
                prepare.Add((middle_side[1], keys[side_color[0]], vector2));
                prepare.Add((middle_top[1], Color.White, vector2));
            }
            if (extract[6] == '2')
            {
                prepare.Add((middle_side[2], keys[side_color[9]], vector2));
                prepare.Add((middle_side[3], keys[side_color[11]], vector2));
                prepare.Add((middle_top[2], Color.White, vector2));
            }
            else
            {
                prepare.Add((middle_side[4], keys[side_color[9]], vector2));
                prepare.Add((middle_side[5], keys[side_color[11]], vector2));
                prepare.Add((middle_top[3], Color.White, vector2));
            }
        }

        private void DisplayAlternativeMiddle(Vector2 vector2, string extract, string side_color)
        {
            if (extract[0] == '4')
            {
                prepare.Add((middle_side[8], keys[side_color[0]], vector2));
                prepare.Add((middle_side[9], keys[side_color[1]], vector2));
                prepare.Add((middle_top[6], Color.White, vector2));
            }
            else
            {
                prepare.Add((middle_side[10], keys[side_color[0]], vector2));
                prepare.Add((middle_side[11], keys[side_color[2]], vector2));
                prepare.Add((middle_top[7], Color.White, vector2));
            }
            if (extract[6] == '2')
            {
                prepare.Add((middle_side[6], keys[side_color[11]], vector2));
                prepare.Add((middle_top[4], Color.White, vector2));
            }
            else
            {
                prepare.Add((middle_side[7], keys[side_color[11]], vector2));
                prepare.Add((middle_top[5], Color.White, vector2));
            }
        }


        private void DisplayUpper(Vector2 vector2, string extract, string side_color, string top_color)
        {
            string backup = extract;
            extract = extract.Substring(6) + extract.Substring(0, 6);
            side_color = side_color.Substring(6) + side_color.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = extract[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                    prepare.Add((small_top_side[pos], keys[side_color[subindex % side_color.Length]], vector2));
                else if (code == 1)
                    prepare.Add((large_top_side[2 * pos], keys[side_color[subindex % side_color.Length]], vector2));
                else
                    prepare.Add((large_top_side[(2 * pos + 23) % 24], keys[side_color[subindex % side_color.Length]], vector2));
                code = extract[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                    prepare.Add((small_top_side[pos], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                else if (code == 1)
                    prepare.Add((large_top_side[2 * pos], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                else
                    prepare.Add((large_top_side[(2 * pos + 23) % 24], keys[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
            }
            int index = 0;
            foreach (char chr in backup)
            {
                int code = chr - '0';
                if (code == 2)
                    prepare.Add((small_top[index], keys[top_color[index]], vector2));
                else if (code == 1)
                    prepare.Add((large_top[index], keys[top_color[index]], vector2));
                index = (index + 1) % 12;
            }
        }

        public bool Act(string code)
        {
            if (code == "=") layers[1].Shift(true);
            else if (code == "/")
            {
                foreach (LayerBase layer in layers) if (layer.Rotatable() == false) return false;
                foreach (LayerBase layer in layers) layer.Reverse(true);
                ColoredRearLayer top, bottom;
                top = layers[0] as ColoredRearLayer;
                bottom = layers[2] as ColoredRearLayer;
                string temp = top.Major;
                top.Major = bottom.Major;
                bottom.Major = temp;
                temp = top.MajorColor;
                top.MajorColor = bottom.MajorColor;
                bottom.MajorColor = temp;
                temp = top.MajorSideColor;
                top.MajorSideColor = bottom.MajorSideColor;
                bottom.MajorSideColor = temp;
            }
            else if (code == "\\")
            {
                foreach (LayerBase layer in layers) if (layer.Rotatable() == false) return false;
                foreach (LayerBase layer in layers) layer.Reverse(false);
                ColoredRearLayer top, bottom;
                top = layers[0] as ColoredRearLayer;
                bottom = layers[2] as ColoredRearLayer;
                string temp = top.Minor;
                top.Minor = bottom.Minor;
                bottom.Minor = temp;
                temp = top.MinorColor;
                top.MinorColor = bottom.MinorColor;
                bottom.MinorColor = temp;
                temp = top.MinorSideColor;
                top.MinorSideColor = bottom.MinorSideColor;
                bottom.MinorSideColor = temp;
            }
            else
            {
                int value;
                LayerBase select = code[code.Length - 1] == '\'' ? layers[2] : layers[0];
                if (code[code.Length - 1] == '\'')
                {
                    value = -int.Parse(code.Substring(0, code.Length - 1));
                }
                else
                {
                    value = int.Parse(code);
                }
                select.Shift(value);
            }
            prepare = null;
            PrintHash();
            return true;
        }
        public List<(int, int)> GetAvailableMoves()
        {
            List<int> top = (layers[0] as RearLayer).GetAvailableMoves();
            List<int> bottom = (layers[2] as RearLayer).GetAvailableMoves();
            List<(int, int)> result = new List<(int, int)>();
            foreach (int top_move in top)
            {
                foreach (int bottom_move in bottom)
                {
                    result.Add((top_move, -bottom_move));
                }
            }
            return result;
        }
        public LinkedList<(int, int)> Scramble(int number)
        {
            Random random = new Random();
            LinkedList<(int, int)> result = new LinkedList<(int, int)>();
            for (int index = 0; index < number; index++)
            {
                List<(int, int)> available = GetAvailableMoves();
                (int, int) pick = available[random.Next(available.Count)];
                result.AddLast((-pick.Item1, -pick.Item2));
                Act($"{pick.Item1}");
                Act($"{pick.Item2}'");
                Act("\\");
            }
            // if (random.Next(2) == 1) Act("=");
            Act("\\");
            return result;
        }
        private static void Init(Controller controller)
        {
            if (init == false)
            {
                init = true;
                small_top = new Texture2D[12];
                small_bottom = new Texture2D[12];
                large_top = new Texture2D[12];
                large_bottom = new Texture2D[12];
                small_top_side = new Texture2D[12];
                large_top_side = new Texture2D[24];
                small_bottom_side = new Texture2D[12];
                large_bottom_side = new Texture2D[24];
                for (int index = 1; index <= 12; index++)
                {
                    small_top[index - 1] = controller.Content.Load<Texture2D>($"Pieces/smallTop{index}");
                    small_bottom[index - 1] = controller.Content.Load<Texture2D>($"Pieces/smallBottom{index}");
                    large_top[index - 1] = controller.Content.Load<Texture2D>($"Pieces/largeTop{index}");
                    large_bottom[index - 1] = controller.Content.Load<Texture2D>($"Pieces/largeBottom{index}");
                    small_top_side[index - 1] = controller.Content.Load<Texture2D>($"Pieces/smallTopSide{index}");
                    large_top_side[2 * (index - 1)] = controller.Content.Load<Texture2D>($"Pieces/largeTopSide{2 * index - 1}");
                    large_top_side[2 * (index - 1) + 1] = controller.Content.Load<Texture2D>($"Pieces/largeTopSide{2 * index}");
                    small_bottom_side[index - 1] = controller.Content.Load<Texture2D>($"Pieces/smallBottomSide{index}");
                    large_bottom_side[2 * (index - 1)] = controller.Content.Load<Texture2D>($"Pieces/largeBottomSide{2 * index - 1}");
                    large_bottom_side[2 * (index - 1) + 1] = controller.Content.Load<Texture2D>($"Pieces/largeBottomSide{2 * index}");
                }
                middle_side = new Texture2D[12];
                for (int index = 1; index <= 12; index++)
                {
                    middle_side[index - 1] = controller.Content.Load<Texture2D>($"Pieces/middleSide{index}");
                }
                middle_top = new Texture2D[8];
                for (int index = 1; index <= 8; index++)
                {
                    middle_top[index - 1] = controller.Content.Load<Texture2D>($"Pieces/middleTop{index}");
                }
            }
        }
        private void Prepare()
        {
            prepare = new List<(Texture2D, Color, Vector2)>();
            Vector2 center = new Vector2(100, 100);
            Vector2 center2 = new Vector2(100, 300);
            string extract, side_color, top_color;
            extract = layers[2].Major + layers[2].Minor;
            side_color = (layers[2] as ColoredRearLayer).MajorSideColor + (layers[2] as ColoredRearLayer).MinorSideColor;
            DisplayLower(center, extract, side_color);

            extract = layers[1].Major + layers[1].Minor;
            side_color = (layers[1] as ColoredMiddleLayer).MajorSideColor + (layers[1] as ColoredMiddleLayer).MinorSideColor;
            DisplayMiddle(center, extract, side_color);

            extract = layers[0].Major + layers[0].Minor;
            side_color = (layers[0] as ColoredRearLayer).MajorSideColor + (layers[0] as ColoredRearLayer).MinorSideColor;
            top_color = (layers[0] as ColoredRearLayer).MajorColor + (layers[0] as ColoredRearLayer).MinorColor;
            DisplayUpper(center, extract, side_color, top_color);

            layers[0].Shift(4);
            layers[0].Reverse(true);
            layers[0].Reverse(false);
            extract = layers[0].Minor + layers[0].Major;
            side_color = (layers[0] as ColoredRearLayer).MinorSideColor + (layers[0] as ColoredRearLayer).MajorSideColor;
            DisplayLower(center2, extract, side_color);
            layers[0].Reverse(false);
            layers[0].Reverse(true);
            layers[0].Shift(-4);

            extract = layers[1].Minor + layers[1].Major;
            side_color = (layers[1] as ColoredMiddleLayer).MinorSideColor + (layers[1] as ColoredMiddleLayer).MajorSideColor;
            LayerBase.Reverse(ref extract);
            LayerBase.Reverse(ref side_color);
            DisplayAlternativeMiddle(center2, extract, side_color);

            layers[2].Shift(4);
            layers[2].Reverse(true);
            layers[2].Reverse(false);
            extract = layers[2].Minor + layers[2].Major;
            side_color = (layers[2] as ColoredRearLayer).MinorSideColor + (layers[2] as ColoredRearLayer).MajorSideColor;
            top_color = (layers[2] as ColoredRearLayer).MinorColor + (layers[2] as ColoredRearLayer).MajorColor;
            DisplayUpper(center2, extract, side_color, top_color);
            layers[2].Reverse(false);
            layers[2].Reverse(true);
            layers[2].Shift(-4);
        }
        private void PrintHash()
        {
            ColoredRearLayer top = layers[0] as ColoredRearLayer;
            ColoredMiddleLayer middle = layers[1] as ColoredMiddleLayer;
            ColoredRearLayer bottom = layers[2] as ColoredRearLayer;
            Console.WriteLine();
            Console.WriteLine(top.MajorSideColor + top.MinorSideColor);
            Console.WriteLine(PerfectShuffle(top.Major, top.MajorColor) + PerfectShuffle(top.Minor, top.MinorColor));
            Console.WriteLine(middle.MajorSideColor + middle.MinorSideColor);
            Console.WriteLine(PerfectShuffle(middle.Major, middle.MajorSideColor) + PerfectShuffle(middle.Minor, middle.MinorSideColor));
            Console.WriteLine(bottom.MajorSideColor + bottom.MinorSideColor);
            Console.WriteLine(PerfectShuffle(bottom.Major, bottom.MajorColor) + PerfectShuffle(bottom.Minor, bottom.MinorColor));
        }
        private static string PerfectShuffle(string a, string b)
        {
            if (a.Length != b.Length) throw new IndexOutOfRangeException();
            StringBuilder result = new StringBuilder();
            for (int index = 0; index < a.Length; index++)
            {
                result.Append(a[index]);
                result.Append(b[index]);
            }
            return result.ToString();
        }
    }
}
