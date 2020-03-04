using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1
{
    public class Cube : IDrawingObject
    {
        private enum Highlight
        {
            TOP, BOT, MIDDLE, NONE
        }
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
        Vector2 position = Vector2.Zero;
        Vector2 center = new Vector2(0, -100);
        Vector2 center2 = new Vector2(0, 100);
        Highlight current = Highlight.NONE;
        readonly LayerBase[] layers;
        List<(Texture2D, Color, Vector2)> top = null, middle = null, bot = null;
        Dictionary<char, Color> normal_map = new Dictionary<char, Color>()
        {
            ['w'] = Color.AntiqueWhite,
            ['y'] = Color.Yellow,
            ['r'] = Color.Red,
            ['g'] = Color.LawnGreen,
            ['b'] = Color.DeepSkyBlue,
            ['o'] = Color.Orange,
        };
        Dictionary<char, Color> highlight_map = new Dictionary<char, Color>()
        {
            ['w'] = Color.White,
            ['y'] = new Color(255, 250, 149),
            ['r'] = new Color(255, 89, 89),
            ['g'] = new Color(185, 255, 106),
            ['b'] = new Color(98, 220, 255),
            ['o'] = new Color(255, 196, 106),
        };
        // IDisplayer displayer;

        public Cube(IDisplayer displayer)
        {
            // this.displayer = displayer;
            Init(displayer.Manager());            
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
        public void Display(IDisplayer displayer)
        {
            if (top == null || bot == null || middle == null) Prepare();
            bot.ForEach(packed => displayer.DrawTexture(packed.Item1, packed.Item2, packed.Item3));
            middle.ForEach(packed => displayer.DrawTexture(packed.Item1, packed.Item2, packed.Item3));
            top.ForEach(packed => displayer.DrawTexture(packed.Item1, packed.Item2, packed.Item3));
        }
        public static bool InsideTriangle(int pos_x, int pos_y, int x, int y, int width, int height)
        {
            if (pos_x < x || pos_x > x + width || pos_y < y || pos_y > y + height) return false;
            if (pos_x < x + width / 2) pos_x = 2 * x + width - pos_x;
            double desire = (double)(x + width - pos_x) / (width / 2) * height;
            return pos_y - y <= desire;
        }
        public static bool InsideRectangle(int pos_x, int pos_y, int x, int y, int width, int height)
        {
            return !(pos_x < x || pos_x > x + width || pos_y < y || pos_y > y + height);
        }
        private void DisplayLower(Vector2 vector2, string extract, string side_color, Dictionary<char, Color> color)
        {
            extract = extract.Substring(6) + extract.Substring(0, 6);
            side_color = side_color.Substring(6) + side_color.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = extract[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                {
                    bot.Add((small_bottom_side[pos], color[side_color[subindex % side_color.Length]], vector2));
                }
                else if (code == 1)
                {
                    bot.Add((large_bottom_side[2 * pos], color[side_color[subindex % side_color.Length]], vector2));
                }
                else
                {
                    bot.Add((large_bottom_side[(2 * pos + 23) % 24], color[side_color[subindex % side_color.Length]], vector2));
                }
                code = extract[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                {
                    bot.Add((small_bottom_side[pos], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
                else if (code == 1)
                {
                    bot.Add((large_bottom_side[2 * pos], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
                else
                {
                    bot.Add((large_bottom_side[(2 * pos + 23) % 24], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                }
            }
            int index = 6;
            foreach (char chr in extract)
            {
                int code = chr - '0';
                if (code == 2)
                {
                    bot.Add((small_bottom[index], color['w'], vector2));
                }
                else if (code == 1)
                {
                    bot.Add((large_bottom[index], color['w'], vector2));
                }
                index = (index + 1) % 12;
            }
        }
        

        private void DisplayMiddle(Vector2 vector2, string extract, string side_color, Dictionary<char, Color> color)
        {
            if (extract[0] == '4')
            {
                middle.Add((middle_side[0], color[side_color[0]], vector2));
                middle.Add((middle_top[0], color['w'], vector2));
            }
            else
            {
                middle.Add((middle_side[1], color[side_color[0]], vector2));
                middle.Add((middle_top[1], color['w'], vector2));
            }
            if (extract[6] == '2')
            {
                middle.Add((middle_side[2], color[side_color[9]], vector2));
                middle.Add((middle_side[3], color[side_color[11]], vector2));
                middle.Add((middle_top[2], color['w'], vector2));
            }
            else
            {
                middle.Add((middle_side[4], color[side_color[9]], vector2));
                middle.Add((middle_side[5], color[side_color[11]], vector2));
                middle.Add((middle_top[3], color['w'], vector2));
            }
        }

        private void DisplayAlternativeMiddle(Vector2 vector2, string extract, string side_color, Dictionary<char, Color> color)
        {
            if (extract[0] == '4')
            {
                middle.Add((middle_side[8], color[side_color[0]], vector2));
                middle.Add((middle_side[9], color[side_color[1]], vector2));
                middle.Add((middle_top[6], color['w'], vector2));
            }
            else
            {
                middle.Add((middle_side[10], color[side_color[0]], vector2));
                middle.Add((middle_side[11], color[side_color[2]], vector2));
                middle.Add((middle_top[7], color['w'], vector2));
            }
            if (extract[6] == '2')
            {
                middle.Add((middle_side[6], color[side_color[11]], vector2));
                middle.Add((middle_top[4], color['w'], vector2));
            }
            else
            {
                middle.Add((middle_side[7], color[side_color[11]], vector2));
                middle.Add((middle_top[5], color['w'], vector2));
            }
        }


        private void DisplayUpper(Vector2 vector2, string extract, string side_color, string top_color, Dictionary<char, Color> color)
        {
            string backup = extract;
            extract = extract.Substring(6) + extract.Substring(0, 6);
            side_color = side_color.Substring(6) + side_color.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = extract[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                    top.Add((small_top_side[pos], color[side_color[subindex % side_color.Length]], vector2));
                else if (code == 1)
                    top.Add((large_top_side[2 * pos], color[side_color[subindex % side_color.Length]], vector2));
                else
                    top.Add((large_top_side[(2 * pos + 23) % 24], color[side_color[subindex % side_color.Length]], vector2));
                code = extract[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                    top.Add((small_top_side[pos], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                else if (code == 1)
                    top.Add((large_top_side[2 * pos], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
                else
                    top.Add((large_top_side[(2 * pos + 23) % 24], color[side_color[(9 + side_color.Length - subindex) % side_color.Length]], vector2));
            }
            int index = 0;
            foreach (char chr in backup)
            {
                int code = chr - '0';
                if (code == 2)
                    top.Add((small_top[index], color[top_color[index]], vector2));
                else if (code == 1)
                    top.Add((large_top[index], color[top_color[index]], vector2));
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
            bot = middle = top = null;
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
        private static void Init(ContentManager manager)
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
                    small_top[index - 1] = manager.Load<Texture2D>($"Pieces/smallTop{index}");
                    small_bottom[index - 1] = manager.Load<Texture2D>($"Pieces/smallBottom{index}");
                    large_top[index - 1] = manager.Load<Texture2D>($"Pieces/largeTop{index}");
                    large_bottom[index - 1] = manager.Load<Texture2D>($"Pieces/largeBottom{index}");
                    small_top_side[index - 1] = manager.Load<Texture2D>($"Pieces/smallTopSide{index}");
                    large_top_side[2 * (index - 1)] = manager.Load<Texture2D>($"Pieces/largeTopSide{2 * index - 1}");
                    large_top_side[2 * (index - 1) + 1] = manager.Load<Texture2D>($"Pieces/largeTopSide{2 * index}");
                    small_bottom_side[index - 1] = manager.Load<Texture2D>($"Pieces/smallBottomSide{index}");
                    large_bottom_side[2 * (index - 1)] = manager.Load<Texture2D>($"Pieces/largeBottomSide{2 * index - 1}");
                    large_bottom_side[2 * (index - 1) + 1] = manager.Load<Texture2D>($"Pieces/largeBottomSide{2 * index}");
                }
                middle_side = new Texture2D[12];
                for (int index = 1; index <= 12; index++)
                {
                    middle_side[index - 1] = manager.Load<Texture2D>($"Pieces/middleSide{index}");
                }
                middle_top = new Texture2D[8];
                for (int index = 1; index <= 8; index++)
                {
                    middle_top[index - 1] = manager.Load<Texture2D>($"Pieces/middleTop{index}");
                }
            }
        }
        private void Prepare()
        {
            bot = new List<(Texture2D, Color, Vector2)>();
            middle = new List<(Texture2D, Color, Vector2)>();
            top = new List<(Texture2D, Color, Vector2)>();
            string extract, side_color, top_color;
            extract = layers[2].Major + layers[2].Minor;
            side_color = (layers[2] as ColoredRearLayer).MajorSideColor + (layers[2] as ColoredRearLayer).MinorSideColor;
            Dictionary<char, Color> top_map = current == Highlight.TOP ? highlight_map : normal_map;
            Dictionary<char, Color> bot_map = current == Highlight.BOT ? highlight_map : normal_map;
            Dictionary<char, Color> middle_map = current == Highlight.MIDDLE ? highlight_map : normal_map;
            DisplayLower(center, extract, side_color, bot_map);

            extract = layers[1].Major + layers[1].Minor;
            side_color = (layers[1] as ColoredMiddleLayer).MajorSideColor + (layers[1] as ColoredMiddleLayer).MinorSideColor;
            DisplayMiddle(center, extract, side_color, middle_map);

            extract = layers[0].Major + layers[0].Minor;
            side_color = (layers[0] as ColoredRearLayer).MajorSideColor + (layers[0] as ColoredRearLayer).MinorSideColor;
            top_color = (layers[0] as ColoredRearLayer).MajorColor + (layers[0] as ColoredRearLayer).MinorColor;
            DisplayUpper(center, extract, side_color, top_color, top_map);

            layers[0].Shift(4);
            layers[0].Reverse(true);
            layers[0].Reverse(false);
            extract = layers[0].Minor + layers[0].Major;
            side_color = (layers[0] as ColoredRearLayer).MinorSideColor + (layers[0] as ColoredRearLayer).MajorSideColor;
            DisplayLower(center2, extract, side_color, top_map);
            layers[0].Reverse(false);
            layers[0].Reverse(true);
            layers[0].Shift(-4);

            extract = layers[1].Minor + layers[1].Major;
            side_color = (layers[1] as ColoredMiddleLayer).MinorSideColor + (layers[1] as ColoredMiddleLayer).MajorSideColor;
            LayerBase.Reverse(ref extract);
            LayerBase.Reverse(ref side_color);
            DisplayAlternativeMiddle(center2, extract, side_color, middle_map);

            layers[2].Shift(4);
            layers[2].Reverse(true);
            layers[2].Reverse(false);
            extract = layers[2].Minor + layers[2].Major;
            side_color = (layers[2] as ColoredRearLayer).MinorSideColor + (layers[2] as ColoredRearLayer).MajorSideColor;
            top_color = (layers[2] as ColoredRearLayer).MinorColor + (layers[2] as ColoredRearLayer).MajorColor;
            DisplayUpper(center2, extract, side_color, top_color, bot_map);
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
        bool top_highlight = true;
        static double Delta(double from, double to)
        {
            return 0;
        }
        bool prevent = false;
        public void Update(int x, int y)
        {
            if (!is_lock)
            {
                Highlight previous = current;
                Vector2 clone = new Vector2(center.X, center.Y);
                clone.X -= 70;
                clone.Y += 40;
                current = Highlight.NONE;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.BOT;
                    top_highlight = true;
                }
                clone.Y -= 26;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.MIDDLE;
                    top_highlight = true;
                }
                clone.Y -= 26;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.TOP;
                    top_highlight = true;
                }
                clone = new Vector2(center2.X, center2.Y);
                clone.X -= 70;
                clone.Y += 40;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.TOP;
                    top_highlight = false;
                }
                clone.Y -= 26;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.MIDDLE;
                    top_highlight = false;
                }
                clone.Y -= 26;
                if (InsideRectangle(x, y, (int)clone.X, (int)clone.Y - 23, 139, 23) || InsideTriangle(x, y, (int)clone.X, (int)clone.Y, 139, 33))
                {
                    current = Highlight.BOT;
                    top_highlight = false;
                }
                if (previous != current) Prepare();

            }
            else if (current != Highlight.NONE)
            {
                dx += x - px;
                dy += y - py;
                if (dy < -20 || dy > 20)
                {
                    dx = 0;
                }
                if (!prevent && (dy < -60 || dy > 60))
                {
                    if (top_highlight)
                    {
                        if (lx > center.X - 70 + 95) Act("/");
                        else Act("\\");
                    }
                    else
                    {
                        if (lx > center.X - 70 + 45) Act("/");
                        else Act("\\");
                    }
                    dx = 0;
                    if (dy < -60) dy += 60;
                    if (dy > 60) dy -= 60;
                    Unlock();
                }
                if (dx > 20)
                {
                    dy = 0;
                    dx = 0;
                    switch (current)
                    {
                        case Highlight.TOP:
                            Act(top_highlight ? "-1" : "1");
                            break;
                        case Highlight.BOT:
                            Act(top_highlight ? "1'" : "-1'");
                            break;
                        default: break;
                    }
                    prevent = true;
                }
                else if (dx < -20)
                {
                    dy = 0;
                    dx = 0;
                    switch (current)
                    {
                        case Highlight.TOP:
                            Act(top_highlight ? "1" : "-1");
                            break;
                        case Highlight.BOT:
                            Act(top_highlight ? "-1'" : "1'");
                            break;
                        default: break;
                    }
                    prevent = true;
                }
                px = x;
                py = y;
            }
        }
        int dx = 0, dy = 0, px = 0, py = 0, lx = 0, ly = 0;
        bool is_lock = false;
        public void Lock(int x, int y)
        {
            if (is_lock == false)
            {
                is_lock = true;
                prevent = false;
                px = x;
                py = y;
                lx = x;
                ly = y;
            }
        }
        public void Unlock()
        {
            if (is_lock == true)
            {
                is_lock = false;
                dx = dy = 0;
            }
        }

        public void Locate(Vector2 position)
        {
            this.position = position;
            center = position - new Vector2(0, 100);
            center2 = position + new Vector2(0, 100);
        }
    }
}
