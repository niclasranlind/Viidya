using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VidyaTutorial
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CubeCharserGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Maze maze;
        BasicEffect effect;

        float moveScale = 1.5f;
        float rotateScale = MathHelper.PiOver2;

        public CubeCharserGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            camera = new Camera(new Vector3(0.5f, 0.5f, 0.5f), 0, GraphicsDevice.Viewport.AspectRatio, 0.05f, 100f);
            effect = new BasicEffect(GraphicsDevice);
            maze = new Maze(GraphicsDevice);
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            #region gamepad
            GamePadState gpstate = GamePad.GetState(PlayerIndex.One);
            float moveYAmount = 0;
            float moveXAmount = 0;
            if (gpstate.ThumbSticks.Left.X != 0.0f)
            {
                camera.Rotation = MathHelper.WrapAngle(
                camera.Rotation - (gpstate.ThumbSticks.Left.X * elapsed));
            }
            
            if (gpstate.ThumbSticks.Right.Y != 0.0f)
            {
                //camera.MoveForward(moveScale * elapsed);

                moveYAmount = gpstate.ThumbSticks.Right.Y * elapsed;
            }
            if (gpstate.ThumbSticks.Right.X != 0.0f)
            {
                //camera.MoveForward(moveScale * elapsed);
                moveXAmount = gpstate.ThumbSticks.Right.X * elapsed;
            }
            #endregion
            KeyboardState keyState = Keyboard.GetState();
            
            if (keyState.IsKeyDown(Keys.Right))
            {
                camera.Rotation = MathHelper.WrapAngle(
                camera.Rotation - (rotateScale * elapsed));
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                camera.Rotation = MathHelper.WrapAngle(
                camera.Rotation + (rotateScale * elapsed));
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                //camera.MoveForward(moveScale * elapsed);

                moveYAmount = moveScale * elapsed;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                //camera.MoveForward(-moveScale * elapsed);
                moveYAmount = -moveScale * elapsed;
            }
            if (moveYAmount != 0)
            {
                Vector3 newLocation = camera.PreviewMove(moveYAmount);
                bool moveOk = true;
                if (newLocation.X < 0 || newLocation.X > Maze.mazeWidth)
                    moveOk = false;
                if (newLocation.Z < 0 || newLocation.Z > Maze.mazeHeight)
                    moveOk = false;
                if (moveOk)
                    camera.MoveForward(moveYAmount);
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Gray);
            maze.Draw(camera, effect);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
