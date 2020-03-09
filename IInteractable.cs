using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    interface IInteractable : IDrawableObject
    {
        void Update(int x, int y, GameTime game_time);
        void Press(int x, int y);
        void Release();
    }
}
