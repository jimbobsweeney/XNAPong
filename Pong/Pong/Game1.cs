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
using System.Threading;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Ball _ball;
        private Paddle _leftPaddle;
        private Paddle _rightPaddle;
        private int _leftScore;
        private int _rightScore;
        private double _roundStartTime;
        private SpriteFont _scoreFont;
        private SpriteFont _countInFont;
        private SpriteFont _winningFont;
        private Vector2 _leftScorePosition;
        private Vector2 _rightScorePosition;
        private Vector2 _countInPosition;
        private Vector2 _winningPosition;
        private string _countIn = "";
        private string _winningMessage = "Welcome to Ultim8 Pong!";
        private string _menuLine1 = "Please select an option:";
        private string _menuLine2 = "Start One Player Game";
        private string _menuLine3 = "Start Two Player Game";
        private string _menuLine4 = "Instructions";
        private string _menuLine5 = "Exit";
        private Vector2 _menuLine1Position;
        private Vector2 _menuLine2Position;
        private Vector2 _menuLine3Position;
        private Vector2 _menuLine4Position;
        private Vector2 _menuLine5Position;
        private int _optionSelected = 1;
        private KeyboardState _lastKeyboardState;
        private SoundEffect _menuChange;
        private SoundEffect _menuSelect;
        private SoundEffect _paddleHit;
        private SoundEffect _pointScored;
        private SoundEffect _male3;
        private SoundEffect _male2;
        private SoundEffect _male1;
        private SoundEffect _winningSound;
        private SoundEffect _powerUpSound;
        private SoundEffect _boing;
        private bool _winningSoundAvailable = true;
        private bool _male3Available = true;
        private bool _male2Available = true;
        private bool _male1Available = true;
        private bool _countInAudible = true;
        private bool _soundEffectsAudible = true;
        private Screen _currentScreen;
        private int _winningScore = 10;

        // Renderer that draws particles to screen
        private Renderer _myRenderer;
        // Particle effect object to store the info about particle
        private ParticleEffect _blueSunEffect;
        private ParticleEffect _pinkSunEffect;

        private enum Screen
        {
            initialMenu = 1,
            instructions = 2,
            gameOnePlayer = 3,
            gameTwoPlayer = 4,
            endMenu = 5
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Create new renderer and set its graphics devide to "this" device
            _myRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = _graphics
            };

            _blueSunEffect = new ParticleEffect();
            _pinkSunEffect = new ParticleEffect();

            _ball = new Ball(this);
            _leftPaddle = new Paddle(this, "left");
            _rightPaddle = new Paddle(this, "right");

            Components.Add(_ball);
            Components.Add(_leftPaddle);
            Components.Add(_rightPaddle);
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
            _currentScreen = Screen.initialMenu;
            _ball.Enabled = false;
            _leftScorePosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 100, -10);
            _rightScorePosition = new Vector2(GraphicsDevice.Viewport.Width/2 + 50, -10);
            _countInPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 60, GraphicsDevice.Viewport.Height/2 - 200);
            _winningPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 160, GraphicsDevice.Viewport.Height/2 - 100);
            _menuLine1Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 - 40);
            _menuLine2Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 + 40);
            _menuLine3Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 + 80);
            _menuLine4Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 83,
                                             GraphicsDevice.Viewport.Height/2 + 120);
            _menuLine5Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 30,
                                             GraphicsDevice.Viewport.Height/2 + 160);

            base.Initialize();
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
            _scoreFont = Content.Load<SpriteFont>("scoreFont");
            _countInFont = Content.Load<SpriteFont>("countInFont");
            _winningFont = Content.Load<SpriteFont>("winningFont");
            _menuChange = Content.Load<SoundEffect>("menuChange");
            _menuSelect = Content.Load<SoundEffect>("menuSelect");
            _paddleHit = Content.Load<SoundEffect>("paddleHit");
            _pointScored = Content.Load<SoundEffect>("pointScored");
            _male3 = Content.Load<SoundEffect>("male3");
            _male2 = Content.Load<SoundEffect>("male2");
            _male1 = Content.Load<SoundEffect>("male1");
            _winningSound = Content.Load<SoundEffect>("winning");
            _powerUpSound = Content.Load<SoundEffect>("powerUp");
            _boing = Content.Load<SoundEffect>("boing");

            _blueSunEffect = Content.Load<ParticleEffect>("BlueSun");
            _blueSunEffect.LoadContent(Content);
            _blueSunEffect.Initialise();
            
            _pinkSunEffect = Content.Load<ParticleEffect>("Sun");
            _pinkSunEffect.LoadContent(Content);
            _pinkSunEffect.Initialise();
            
            _myRenderer.LoadContent(Content);
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
            if (_currentScreen == Screen.gameOnePlayer || _currentScreen == Screen.gameTwoPlayer)
            {
                CheckKeys();
                KeepPaddlesOnScreen();
                if (_currentScreen == Screen.gameOnePlayer)
                {
                    MoveLeftPaddleForComputerPlayer(gameTime); 
                }
                CheckCollision(_leftPaddle);
                CheckCollision(_rightPaddle);
                CheckIfPointScored();

                _roundStartTime += gameTime.ElapsedGameTime.TotalSeconds;
            }

            // There is a second if statement here so that the winning screen works properly.
            if (_currentScreen == Screen.gameOnePlayer || _currentScreen == Screen.gameTwoPlayer)
            {
                // Gives a count-in.
                if (_roundStartTime < 1)
                {
                    _countIn = "3";
                    if (_male3Available)
                    {
                        _male3Available = false;
                        PlaySoundEffect(_male3, _countInAudible);
                    }
                }
                else if (_roundStartTime < 2)
                {
                    _countIn = "2";
                    if (_male2Available)
                    {
                        _male2Available = false;
                        PlaySoundEffect(_male2, _countInAudible);
                    }
                }
                else if (_roundStartTime < 3)
                {
                    _countIn = "1";
                    if (_male1Available)
                    {
                        _male1Available = false;
                        PlaySoundEffect(_male1, _countInAudible);
                    }
                }
                else
                {
                    _countIn = "";
                    _ball.Enabled = true;
                }
            }
            else if (_currentScreen == Screen.endMenu && (_leftScore == _winningScore || _rightScore == _winningScore))
            {
                _currentScreen = Screen.endMenu;
                if (_winningSoundAvailable)
                {
                    _winningSoundAvailable = false;
                    PlaySoundEffect(_winningSound, _soundEffectsAudible);
                }
                string winningSide;
                winningSide = _leftScore == _winningScore ? "Red" : "Blue";
                _winningMessage = String.Format("{0} has won!", winningSide);
                _winningPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 107,
                                               GraphicsDevice.Viewport.Height/2 - 100);
                CheckMenuKeys();
            }
            else if (_currentScreen == Screen.initialMenu)
            {
                CheckMenuKeys();
            }
            else if (_currentScreen == Screen.instructions)
            {
                CheckForSpace();
            }

            base.Update(gameTime);

            // "Deltatime" ie, time since last update call
            float SecondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _blueSunEffect.Update(SecondsPassed);
            _pinkSunEffect.Update(SecondsPassed);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_scoreFont, _leftScore.ToString(), _leftScorePosition, Color.Red);
            _spriteBatch.DrawString(_scoreFont, _rightScore.ToString(), _rightScorePosition, Color.Blue);
            _spriteBatch.DrawString(_countInFont, _countIn, _countInPosition, Color.Goldenrod);
            _spriteBatch.DrawString(_winningFont, _winningMessage, _winningPosition, Color.Gold);
            if (_currentScreen != Screen.gameOnePlayer && _currentScreen != Screen.gameTwoPlayer)
            {
                DrawMenu();
            }
            _spriteBatch.End();

            _myRenderer.RenderEffect(_blueSunEffect);
            _myRenderer.RenderEffect(_pinkSunEffect);

            base.Draw(gameTime);
        }

        private void CheckKeys()
        {
            int amountToMoveBy = 10;
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Up))
            {
                _rightPaddle._paddlePosition.Y -= amountToMoveBy;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                _rightPaddle._paddlePosition.Y += amountToMoveBy;
            }
            if (_currentScreen == Screen.gameTwoPlayer)
            {
                if (kState.IsKeyDown(Keys.A))
                {
                    _leftPaddle._paddlePosition.Y -= amountToMoveBy;
                }
                if (kState.IsKeyDown(Keys.Z))
                {
                    _leftPaddle._paddlePosition.Y += amountToMoveBy;
                }
            }
            if (kState.IsKeyDown(Keys.F11) && _lastKeyboardState != kState)
            {
                _boing.Play();
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
                RedrawElements();
            }
            if (kState.IsKeyDown(Keys.F10) && _lastKeyboardState != kState)
            {
                _boing.Play();
                _countInAudible = !_countInAudible;
            }
            if (kState.IsKeyDown(Keys.F9) && _lastKeyboardState != kState)
            {
                _boing.Play();
                _soundEffectsAudible = !_soundEffectsAudible;
                _ball._soundEffectsAudible = !_ball._soundEffectsAudible;
            }
            _lastKeyboardState = kState;
        }

        private void KeepPaddlesOnScreen()
        {
            if (_leftPaddle._paddlePosition.Y < 0)
            {
                _leftPaddle._paddlePosition.Y = 0;
            }
            if (_leftPaddle._paddlePosition.Y > GraphicsDevice.Viewport.Height - _leftPaddle._paddleSprite.Height)
            {
                _leftPaddle._paddlePosition.Y = GraphicsDevice.Viewport.Height - _leftPaddle._paddleSprite.Height;
            }

            if (_rightPaddle._paddlePosition.Y < 0)
            {
                _rightPaddle._paddlePosition.Y = 0;
            }
            if (_rightPaddle._paddlePosition.Y > GraphicsDevice.Viewport.Height - _rightPaddle._paddleSprite.Height)
            {
                _rightPaddle._paddlePosition.Y = GraphicsDevice.Viewport.Height - _rightPaddle._paddleSprite.Height;
            }
        }

        private void CheckCollision(Paddle paddle)
        {
            if (paddle._boundingBox.Intersects(_ball._boundingBox))
            {
                // This makes the tops and bottoms of the paddles do interesting things.
                if (_ball._ballDirection.Y < 0)
                {
                    float paddleBottom = paddle._paddlePosition.Y + paddle._paddleSprite.Height;
                    if (_ball._ballPosition.Y - paddleBottom > -20)
                    {
                        _ball.ChangeYDirection();
                    }
                }
                else
                {
                    float paddleTop = paddle._paddlePosition.Y;
                    if (paddleTop - (_ball._ballPosition.Y + _ball._ballSprite.Height) > -20)
                    {
                        _ball.ChangeYDirection();
                    }
                }

                PlaySoundEffect(_paddleHit, _soundEffectsAudible);
                _ball.ChangeXDirection();
                _ball.SpeedUp();
                if (paddle == _leftPaddle)
                {
                    _pinkSunEffect.Trigger(new Vector2(_ball._ballPosition.X + _ball._ballSprite.Width / 2, _ball._ballPosition.Y + _ball._ballSprite.Height / 2));
                }
                else
                {
                    _blueSunEffect.Trigger(new Vector2(_ball._ballPosition.X + _ball._ballSprite.Width / 2, _ball._ballPosition.Y + _ball._ballSprite.Height / 2));
                }
            }
        }

        private void CheckIfPointScored()
        {
            float ballLeftEdge = _ball._ballPosition.X;
            float ballRightEdge = _ball._ballPosition.X + _ball._ballSprite.Width;

            if (ballRightEdge < 0)
            {
                _rightScore++;
                PlaySoundEffect(_pointScored, _soundEffectsAudible);
                _roundStartTime = 0;
                _ball.Enabled = false;
                _male1Available = true;
                _male2Available = true;
                _male3Available = true;

                if (_rightScore != _winningScore)
                {
                    _ball.Restart();

                    // This makes the paddle smaller each time a player reaches a multiple of 2.
                    if (_rightScore % 2 == 0)
                    {
                        PlaySoundEffect(_powerUpSound, _soundEffectsAudible);
                        _rightPaddle.ChangePaddleSize();
                    }
                }
                else
                {
                    _currentScreen = Screen.endMenu;
                }
            }
            else if (ballLeftEdge > GraphicsDevice.Viewport.Width)
            {
                _leftScore++;
                PlaySoundEffect(_pointScored, _soundEffectsAudible);
                _roundStartTime = 0;
                _ball.Enabled = false;
                _male1Available = true;
                _male2Available = true;
                _male3Available = true;

                if (_leftScore != 10)
                {
                    _ball.Restart();

                    // This makes the paddle smaller each time a player reaches a multiple of 2.
                    if (_leftScore % 2 == 0)
                    {
                        PlaySoundEffect(_powerUpSound, _soundEffectsAudible);
                        _leftPaddle.ChangePaddleSize();
                    }
                }
                else
                {
                    _currentScreen = Screen.endMenu;
                }
            }
        }

        private void RedrawElements()
        {
            _leftScorePosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 100, -10);
            _rightScorePosition = new Vector2(GraphicsDevice.Viewport.Width/2 + 50, -10);
            _countInPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 45, 0);
            _winningPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 107, GraphicsDevice.Viewport.Height/2 - 100);
            _menuLine1Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 - 40);
            _menuLine2Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 + 40);
            _menuLine3Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 160,
                                             GraphicsDevice.Viewport.Height/2 + 80);
            _menuLine4Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 83,
                                             GraphicsDevice.Viewport.Height/2 + 120);
            _menuLine5Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 30,
                                             GraphicsDevice.Viewport.Height/2 + 160);
            _leftPaddle._paddlePosition.X = 0;
            _rightPaddle._paddlePosition.X = GraphicsDevice.Viewport.Width - _rightPaddle._paddleSprite.Width;
        }

        private void DrawMenu()
        {
            // Highlights the option selected in the menu.
            if (_optionSelected == 1)
            {
                _spriteBatch.DrawString(_winningFont, _menuLine1, _menuLine1Position, Color.Firebrick);
                _spriteBatch.DrawString(_winningFont, _menuLine2, _menuLine2Position, Color.Teal);
                _spriteBatch.DrawString(_winningFont, _menuLine3, _menuLine3Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine4, _menuLine4Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine5, _menuLine5Position, Color.White);
            }
            else if (_optionSelected == 2)
            {
                _spriteBatch.DrawString(_winningFont, _menuLine1, _menuLine1Position, Color.Firebrick);
                _spriteBatch.DrawString(_winningFont, _menuLine2, _menuLine2Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine3, _menuLine3Position, Color.Teal);
                _spriteBatch.DrawString(_winningFont, _menuLine4, _menuLine4Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine5, _menuLine5Position, Color.White);
            }
            else if (_optionSelected == 3)
            {
                _spriteBatch.DrawString(_winningFont, _menuLine1, _menuLine1Position, Color.Firebrick);
                _spriteBatch.DrawString(_winningFont, _menuLine2, _menuLine2Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine3, _menuLine3Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine4, _menuLine4Position, Color.Teal);
                _spriteBatch.DrawString(_winningFont, _menuLine5, _menuLine5Position, Color.White);
            }
            else if (_optionSelected == 4)
            {
                _spriteBatch.DrawString(_winningFont, _menuLine1, _menuLine1Position, Color.Firebrick);
                _spriteBatch.DrawString(_winningFont, _menuLine2, _menuLine2Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine3, _menuLine3Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine4, _menuLine4Position, Color.White);
                _spriteBatch.DrawString(_winningFont, _menuLine5, _menuLine5Position, Color.Teal);
            }
        }

        private void CheckMenuKeys()
        {
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Up) && _lastKeyboardState != kState)
            {
                if (_optionSelected > 1)
                {
                    _optionSelected--;
                    PlaySoundEffect(_menuChange, _soundEffectsAudible);
                }
            }
            if (kState.IsKeyDown(Keys.Down) && _lastKeyboardState != kState)
            {
                if (_optionSelected < 4)
                {
                    _optionSelected++;
                    PlaySoundEffect(_menuChange, _soundEffectsAudible);
                }
            }
            if (kState.IsKeyDown(Keys.Enter) && _optionSelected == 1)
            {
                _currentScreen = Screen.gameOnePlayer;
                StartGame();
            }
            else if (kState.IsKeyDown(Keys.Enter) && _optionSelected == 2)
            {
                _currentScreen = Screen.gameTwoPlayer;
                StartGame();
            }
            else if (kState.IsKeyDown(Keys.Enter) && _optionSelected == 3)
            {
                PlaySoundEffect(_menuSelect, _soundEffectsAudible);
                DisplayInstructions();
            }
            else if (kState.IsKeyDown(Keys.Enter) && _optionSelected == 4)
            {
                PlaySoundEffect(_menuSelect, _soundEffectsAudible);
                Thread.Sleep(200); // This put in so the sound effect doesn't get cut off.
                Environment.Exit(0);
            }
            _lastKeyboardState = kState;
        }

        private void StartGame()
        {
            _optionSelected = 1;
            _leftScore = 0;
            _rightScore = 0;
            _winningMessage = "";
            _ball.Restart();
            PlaySoundEffect(_menuSelect, _soundEffectsAudible);
            _winningSoundAvailable = true;
            _leftPaddle.ResetPaddleSize();
            _rightPaddle.ResetPaddleSize();
        }

        private void PlaySoundEffect(SoundEffect sound, bool audibleBool)
        {
            if (audibleBool)
            {
                sound.Play();
            }
        }

        private void DisplayInstructions()
        {
            _currentScreen = Screen.instructions;
            _winningMessage = "Instructions";
            _menuLine1 = "Keys to use during the game:";
            _menuLine2 = "A/Z - up/down for left-hand player.";
            _menuLine3 = "Up/Down - up/down for right-hand player.";
            _menuLine4 =
                "F9: Toggle Sound Effects On/Off\nF10: Toggle Count In Sound Effects On/Off\nF11: Toggle Full Screen Mode On/Off";
            _menuLine5 = "Press Space to return to the main menu.";
            _winningFont = Content.Load<SpriteFont>("instructionsFont");
            _winningPosition = new Vector2(GraphicsDevice.Viewport.Width/2 - 70, GraphicsDevice.Viewport.Height/2 - 170);
            _menuLine1Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 240,
                                             GraphicsDevice.Viewport.Height/2 - 100);
            _menuLine2Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 240,
                                             GraphicsDevice.Viewport.Height/2 - 40);
            _menuLine3Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 240, GraphicsDevice.Viewport.Height/2);
            _menuLine4Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 240,
                                             GraphicsDevice.Viewport.Height/2 + 40);
            _menuLine5Position = new Vector2(GraphicsDevice.Viewport.Width/2 - 240,
                                             GraphicsDevice.Viewport.Height/2 + 160);
        }

        private void CheckForSpace()
        {
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Space) && _lastKeyboardState != kState)
            {
                PlaySoundEffect(_menuSelect, _soundEffectsAudible);
                RemoveInstructions();
            }
            _lastKeyboardState = kState;
        }

        private void RemoveInstructions()
        {
            _winningMessage = "Welcome to Ultim8 Pong!";
            _menuLine1 = "Please select an option:";
            _menuLine2 = "Start One Player Game";
            _menuLine3 = "Start Two Player Game";
            _menuLine4 = "Instructions";
            _menuLine5 = "Exit";
            Initialize();
        }

        private void MoveLeftPaddleForComputerPlayer(GameTime gameTime)
        {
            if (_ball.Enabled)
            {
                double ratioOfComputerPaddleSpeedToBallSpeed = 0.7; // This affects how accurate the computer player will be.
                _leftPaddle._paddlePosition.Y += (float)(_ball._ballDirection.Y * ratioOfComputerPaddleSpeedToBallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}
