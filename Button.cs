using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Square_1NN.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    class ThreeStageButton : IInteractable
    {
        public delegate void ActivationFunction();
        ActivationFunction activation;
        Texture2D normal, hover, click, current;
        Vector2 position = Vector2.Zero, A, B;
        public ThreeStageButton(Texture2D normal, Texture2D click, Texture2D hover, ActivationFunction function = null)
        {
            current = this.normal = normal;
            this.hover = hover;
            this.click = click;
            Locate(position);
            activation = function;
        }
        public void Display(IDisplayer displayer)
        {
            displayer.DrawTexture(current, Color.White, position);
        }
        public void Locate(Vector2 position)
        {
            this.position = position;
            Vector2 size = new Vector2(normal.Width, normal.Height);
            A = position - size / 2;
            B = position + size / 2;
        }
        public void Update(int x, int y, GameTime _)
        {
            if (current == normal && x >= A.X && x <= B.X && y >= A.Y && y <= B.Y)
            {
                current = hover;
            }
            else if (current == hover && !(x >= A.X && x <= B.X && y >= A.Y && y <= B.Y))
            {
                current = normal;
            }   
        }
        public void Press(int x, int y)
        {
            if (current == hover && x >= A.X && x <= B.X && y >= A.Y && y <= B.Y)
            {
                current = click;
            }
        }
        public void Release()
        {
            if (current == click)
            {
                current = normal;
                activation?.Invoke();
            }
        }
    }
}
