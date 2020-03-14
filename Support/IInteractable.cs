using Microsoft.Xna.Framework;


namespace Square_1NN.Support
{
    interface IInteractable : IDrawableObject
    {
        void Update(int x, int y, GameTime game_time);
        void Press(int x, int y);
        void Release();
    }
}
