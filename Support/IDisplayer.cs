using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Square_1NN.Support
{
    public interface IDisplayer
    {
        ContentManager GetManager();
        void DrawTexture(Texture2D texture, Color tint, Vector2 position);
        void DrawString(SpriteFont font, string text, Color tint, Vector2 position);
    }
}