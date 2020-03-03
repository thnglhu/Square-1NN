using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square_1NN.Square1
{

    public class CubeLite
    {
        string top, bot;
        bool mid;
        public string Top { get => top; }
        public bool Mid { get => mid; }
        public string Bot { get => bot; }
        public CubeLite()
        {
            bot = "abcdefgh";
            // top = "aaaaaaaaaaaa";
            top = "ijklmnop";
            mid = true;
        }
        public CubeLite(string top, bool mid, string bot)
        {
            this.top = top;
            this.mid = mid;
            this.bot = bot;
        }
        public bool Rotate()
        {
            int upper_bound = 0, lower_bound = 0;
            int upper = 0, lower = 0;
            while (upper < 6) upper += (top[upper_bound++] - 'a') % 2 + 1;
            if (upper > 6) return false;
            while (lower < 6) lower += (bot[lower_bound++] - 'a') % 2 + 1;
            if (lower > 6) return false;
            mid ^= false;
            string temp = top;
            top = bot.Substring(0, lower_bound) + top.Substring(upper_bound);
            bot = temp.Substring(0, upper_bound) + bot.Substring(lower_bound);
            return true;
        }
        private bool Shift(int upper, int lower)
        {

            int upper_bound = 0, lower_bound = 0;
            while (upper > 0) upper -= (top[upper_bound++] - 'a') % 2 + 1;
            if (upper < 0) return false;
            while (lower > 0) lower -= (bot[lower_bound++] - 'a') % 2 + 1;
            if (lower < 0) return false;
            top = top.Substring(upper_bound) + top.Substring(0, upper_bound);
            bot = bot.Substring(lower_bound) + bot.Substring(0, lower_bound);
            return true;
        }
        private string Reverse(string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}
