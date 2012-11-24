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
    class Ball : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        public Texture2D _ballSprite;
        public Vector2 _ballPosition;
        public Vector2 _ballDirection;
        public Rectangle _boundingBox;
        private SoundEffect _sideHit;
        public bool _soundEffectsAudible = true;
        
        public Ball (Game game) : base(game)
        {
            _contentManager = new ContentManager(game.Services);
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
            Restart();
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
            _ballSprite = _contentManager.Load<Texture2D>(@"Content\ball");
            _sideHit = _contentManager.Load<SoundEffect>(@"Content\sideHit");
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
            _ballPosition += _ballDirection*(float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            BounceOffWalls();
            _boundingBox = new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, _ballSprite.Width, _ballSprite.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_ballSprite, _ballPosition, Color.Transparent); // DArk Green
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Restart()
        {
            //Makes a random start point and a random direction for the ball.
            Random random = new Random();
            int randomWidth = random.Next(50, GraphicsDevice.Viewport.Width - 50 - _ballSprite.Width);
            int randomHeight = random.Next(50, GraphicsDevice.Viewport.Height - 50 - _ballSprite.Height);
            int randomDirectionLeftX = random.Next(-250, -150);
            int randomDirectionRightX = random.Next(150, 250);
            int randomDirectionUpY = random.Next(-250, -150);
            int randomDirectionDownY = random.Next(150, 250);

            int[] randomDirectionsX = {randomDirectionLeftX, randomDirectionRightX};
            int[] randomDirectionsY = {randomDirectionUpY, randomDirectionDownY};

            int randomDirectionX = randomDirectionsX[random.Next(0, 2)];
            int randomDirectionY = randomDirectionsY[random.Next(0, 2)];

            _ballPosition.X = randomWidth;
            _ballPosition.Y = randomHeight;

            _ballDirection.X = randomDirectionX;
            _ballDirection.Y = randomDirectionY;
        }

        private void BounceOffWalls()
        {
            if(_ballPosition.Y < 0 || _ballPosition.Y > GraphicsDevice.Viewport.Height - _ballSprite.Height)
            {
                if (_soundEffectsAudible)
                {
                    _sideHit.Play();
                }
                ChangeYDirection();
            }
        }

        public void ChangeXDirection()
        {
            _ballDirection.X *= -1;
        }

        public void ChangeYDirection()
        {
            _ballDirection.Y *= -1;
        }

        public void SpeedUp()
        {
            if(_ballDirection.X < 0)
            {
                _ballDirection.X -= 30;
            }
            else
            {
                _ballDirection.X += 30;
            }

            if (_ballDirection.Y < 0)
            {
                _ballDirection.Y -= 30;
            }
            else
            {
                _ballDirection.Y += 30;
            }
        }
    }
}