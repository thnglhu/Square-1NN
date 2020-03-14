using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square_1NN.Support;
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
        CubeController controller;
        SpriteFont font;
        HashSet<IInteractable> interactables;
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
            interactables = new HashSet<IInteractable>();
            controller = new CubeController(
                new Cube("YrgygYgoyo YobybYbryr Rgo Obr wrWrgwgWgo woWobwbWbr"), 
                new CubeView(), 
                this);
            controller.Locate(new Vector2(100, 200));

            ThreeStageButton scramble_button = new ThreeStageButton(
                Content.Load<Texture2D>("Pieces/scramble-button-1"), 
                Content.Load<Texture2D>("Pieces/scramble-button-2"),
                Content.Load<Texture2D>("Pieces/scramble-button-3"),
                () => controller.Scramble(14, true));

            ThreeStageButton reset_button = new ThreeStageButton(
                Content.Load<Texture2D>("Pieces/reset-button-1"),
                Content.Load<Texture2D>("Pieces/reset-button-2"),
                Content.Load<Texture2D>("Pieces/reset-button-3"),
                () => controller.Reset()
                ); ;

            scramble_button.Locate(new Vector2(25, 500 - 13));
            reset_button.Locate(new Vector2(175, 500 - 13));

            interactables.Add(scramble_button);
            interactables.Add(reset_button);
            interactables.Add(controller);

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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState mouse = Mouse.GetState();
            foreach (IInteractable interactable in interactables)
            {
                interactable.Update(mouse.X, mouse.Y, gameTime);
            }
            if (mouse.LeftButton == ButtonState.Pressed && released)
            {
                foreach (IInteractable interactable in interactables)
                {
                    interactable.Press(mouse.X, mouse.Y);
                }
                released = false;
            }
            if (!released && mouse.LeftButton == ButtonState.Released)
            {
                foreach (IInteractable interactable in interactables)
                {
                    interactable.Release();
                }
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
            foreach (IInteractable interactable in interactables)
            {
                interactable.Display(this);
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
