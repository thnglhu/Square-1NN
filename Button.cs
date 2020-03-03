using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    class Button : IDrawingObject
    {
        Texture2D normal, click;
        Vector2 position = Vector2.Zero;
        public Button(Texture2D normal, Texture2D click)
        {
            this.normal = normal;
            this.click = click;
        }
        public void Display(IDisplayer displayer)
        {
            displayer.DrawTexture(normal, Color.White, position);
        }

        public void Locate(Vector2 position)
        {
            this.position = position;
        }
    }
}
