using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Square_1NN.Extension;
using Square_1NN.Square1.Layer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1
{
    class Cube : ICloneable
    {
        private static Cube singleton = null;
        ColoredRearLayer top, bot;
        public ColoredRearLayer Top { get => top; private set => top = value; }
        public ColoredRearLayer Bot { get => bot; private set => bot = value; }
        ColoredMiddleLayer mid;
        public ColoredMiddleLayer Mid { get => mid; private set => mid = value; }

        static Cube()
        {
            singleton = new Cube(
                "yrYrgygYgo yoYobybYbr " +
                "Rgo Obr " +
                "wrWrgwgWgo woWobwbWbr"
                );
        }
        public static Cube Solved() => singleton.Clone() as Cube;
        public Cube()
        {
            Top = singleton.Top.Clone() as ColoredRearLayer;
            Mid = singleton.Mid.Clone() as ColoredMiddleLayer;
            Bot = singleton.Bot.Clone() as ColoredRearLayer;
        }
        public Cube(string mask)
        {
            string[] split = mask.Split();
            Top = new ColoredRearLayer(split[0] + " " + split[1]);
            Mid = new ColoredMiddleLayer(split[2] + " " + split[3]);
            bot = new ColoredRearLayer(split[4] + " " + split[5]);
        }
        internal List<(int, int)> GetAvailableMoves()
        {
            List<int> top = Top.GetAvailableMoves();
            List<int> bottom = Bot.GetAvailableMoves();
            List<(int, int)> result = new List<(int, int)>();
            foreach (int top_move in top)
                foreach (int bottom_move in bottom)
                    result.Add((top_move, -bottom_move));
            return result;
        }
        public object Clone()
        {
            Cube clone = new Cube();
            clone.Top = Top.Clone() as ColoredRearLayer;
            clone.Mid = Mid.Clone() as ColoredMiddleLayer;
            clone.Bot = Bot.Clone() as ColoredRearLayer;
            return clone;
        }
    }
}
