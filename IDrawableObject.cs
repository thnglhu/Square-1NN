using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    interface IDrawableObject
    {
        void Display(IDisplayer displayer);
        void Locate(Vector2 position);
    }
}
