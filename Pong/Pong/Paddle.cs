using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    class Paddle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        public Texture2D _paddleSprite;
        public Vector2 _paddlePosition;
        private string _side;
        public Rectangle _boundingBox;
        private int _size = 1;
        
        public Paddle (Game game, string side) : base(game)
        {
            _contentManager = new ContentManager(game.Services);
            _side = side;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            if(_side == "left")
            {
                _paddlePosition = new Vector2(0, (GraphicsDevice.Viewport.Height - _paddleSprite.Height) / 2);
            }
            else
            {
                _paddlePosition = new Vector2(GraphicsDevice.Viewport.Width - _paddleSprite.Width, (GraphicsDevice.Viewport.Height - _paddleSprite.Height) / 2);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _paddleSprite = _contentManager.Load<Texture2D>(@"Content\paddle");
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
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            _boundingBox = new Rectangle((int)_paddlePosition.X, (int)_paddlePosition.Y, _paddleSprite.Width, _paddleSprite.Height);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            if (_side == "left")
            {
                _spriteBatch.Draw(_paddleSprite, _paddlePosition, Color.Red);
            }
            else
            {
                _spriteBatch.Draw(_paddleSprite, _paddlePosition, Color.Blue);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ChangePaddleSize()
        {
            _size++;
            string paddlePath = @"Content\paddle" + _size;
            _paddleSprite = _contentManager.Load<Texture2D>(paddlePath);

            // Moves the paddle in if it's the right-hand one.
            if (_side == "right")
            {
                _paddlePosition.X = GraphicsDevice.Viewport.Width - _paddleSprite.Width;
            }
        }

        public void ResetPaddleSize()
        {
            _size = 1;
            _paddleSprite = _contentManager.Load<Texture2D>(@"Content\paddle");

            // Moves the paddle in if it's the right-hand one.
            if (_side == "right")
            {
                _paddlePosition.X = GraphicsDevice.Viewport.Width - _paddleSprite.Width;
            }
        }
    }
}
