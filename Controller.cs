using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square_1NN.Square1;
using System.Collections.Generic;

namespace Square_1NN
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Controller : Game, IDisplayer
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Cube cube;
        SpriteFont font;
        ThreeStageButton button;

        public Controller()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 200;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        LinkedList<(int, int)> trace;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cube = new Cube(this);
            cube.Locate(new Vector2(100, 200));
            button = new ThreeStageButton(
                Content.Load<Texture2D>("Pieces/scramble-button-1"), 
                Content.Load<Texture2D>("Pieces/scramble-button-2"),
                Content.Load<Texture2D>("Pieces/scramble-button-3"));
            button.Locate(new Vector2(25, 500-13));
            trace = cube.Scramble(50);
            font = Content.Load<SpriteFont>("MarioFont");
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        bool space = false, rotate = false, middle = false;
        int current = 0;
        List<string> log = new List<string>() { "" };
        int up = 0, down = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //game_time += gameTime.ElapsedGameTime.TotalSeconds;
            //if (game_time >= 0.01)
            //{
            //    game_time -= 0.01;
            //    if (trace != null && trace.Count > 0)
            //    {
            //        LinkedListNode<(int, int)> last = trace.Last;

            //        if (last.Value.Item1 == 0 && last.Value.Item2 == 0)
            //        {
            //            cube.Act("\\");
            //            trace.RemoveLast();
            //        }
            //        else
            //        {
            //            if (last.Value.Item1 < 0)
            //            {
            //                cube.Act("-1");
            //                last.Value = (last.Value.Item1 + 1, last.Value.Item2);
            //            }
            //            else if (last.Value.Item1 > 0)
            //            {
            //                cube.Act("1");
            //                last.Value = (last.Value.Item1 - 1, last.Value.Item2);
            //            }
            //            else if (last.Value.Item2 < 0)
            //            {
            //                cube.Act("-1'");
            //                last.Value = (last.Value.Item1, last.Value.Item2 + 1);
            //            }
            //            else if (last.Value.Item2 > 0)
            //            {
            //                cube.Act("1'");
            //                last.Value = (last.Value.Item1, last.Value.Item2 - 1);
            //            }

            //        }
            //    }
            //    else if (trace != null)
            //    {
            //        cube.Act("\\");
            //        trace = null;
            //    }
            // }
            // TODO: Add your update logic here
            //KeyboardState state = Keyboard.GetState();
            //if (!space && state.IsKeyDown(Keys.Space))
            //{
            //    int code = state.IsKeyDown(Keys.LeftShift) ? -1 : 1;
            //    if (state.IsKeyDown(Keys.LeftControl))
            //    {
            //        down += code;
            //        cube.Act(code + "'");
            //    }
            //    else
            //    {
            //        up += code;
            //        cube.Act(code.ToString());
            //    }
            //    if (up > 6) up -= 12;
            //    if (down > 6) down -= 12;
            //    if (up < -5) up += 12;
            //    if (down < -5) down -= 12;
            //    space = true;
            //}
            //else if (space && state.IsKeyUp(Keys.Space))
            //{
            //    space = false;
            //}
            //if (!rotate && state.IsKeyDown(Keys.R))
            //{
            //    if (state.IsKeyDown(Keys.LeftControl)) cube.Act("\\");
            //    else cube.Act("/");
            //    log[current] += $"({up}, {down})/";
            //    if (log[current].Length > 70)
            //    {
            //        log.Add("");
            //        current++;
            //    }
            //    up = down = 0;
            //    rotate = true;
            //}
            //else if (rotate && state.IsKeyUp(Keys.R))
            //{
            //    rotate = false;
            //}
            //if (!middle && state.IsKeyDown(Keys.Tab))
            //{
            //    middle = true;
            //    cube.Act("=");
            //}
            //else if (middle && state.IsKeyUp(Keys.Tab))
            //{
            //    middle = false;
            //}
            MouseState mouse = Mouse.GetState();
            cube.Update(mouse.X, mouse.Y);
            button.Update(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed && released)
            {
                cube.Lock(mouse.X, mouse.Y);
                button.Press(mouse.X, mouse.Y);
                released = false;
            }
            if (!released && mouse.LeftButton == ButtonState.Released)
            {
                cube.Unlock();
                button.Release();
                released = true;
            }
            base.Update(gameTime);
        }
        bool released = true;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            cube.Display(this);
            button.Display(this);
            for (int index = 0; index < log.Count; index++)
            {
                spriteBatch.DrawString(font, log[index], new Vector2(300, 40 + 20 * index), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void DrawTexture(Texture2D texture, Color tint, Vector2 position)
        {
            if (texture == null) return;
            position = Vector2.Add(position, new Vector2(-texture.Width / 2, -texture.Height / 2));
            spriteBatch.Draw(texture, position, tint);
        }
        public void DrawString(SpriteFont font, string text, Color tint, Vector2 position)
        {
            spriteBatch.DrawString(font, text, position - font.MeasureString(text) / 2, tint);
        }

        public ContentManager Manager()
        {
            return Content;
        }
    }
}
