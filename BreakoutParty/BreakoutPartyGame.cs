﻿using BreakoutParty.Gamestates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty
{
    /// <summary>
    /// The BreakoutParty game class.
    /// </summary>
    sealed class BreakoutPartyGame
        : Game
    {
        /// <summary>
        /// Number of pixels per meter.
        /// </summary>
        public const float PixelsPerMeter = 32f;

        /// <summary>
        /// Number of meters per pixel.
        /// </summary>
        public const float MeterPerPixel = 1f / PixelsPerMeter;

        /// <summary>
        /// The <see cref="SpriteBatch"/> used for drawing the game.
        /// </summary>
        public SpriteBatch Batch;

        /// <summary>
        /// The <see cref="GraphicsDeviceManager"/>.
        /// </summary>
        public GraphicsDeviceManager Graphics;

        /// <summary>
        /// The <see cref="GamestateManager"/>.
        /// </summary>
        public GamestateManager GameManager;

        /// <summary>
        /// The <see cref="RenderTarget2D"/> the game gets drawn into. After
        /// it has been drawn into the texture it will be scaled up and
        /// drawn into the window.
        /// </summary>
        private RenderTarget2D _GameBuffer;

        /// <summary>
        /// Background texture.
        /// </summary>
        private Texture2D _Background;

        /// <summary>
        /// Creates a new <see cref="BreakoutPartyGame"/> instance.
        /// </summary>
        public BreakoutPartyGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;
            
        }

        protected override void Initialize()
        {
            base.Initialize();

            Batch = new SpriteBatch(Graphics.GraphicsDevice);
            GameManager = new GamestateManager(this);
            GameManager.Add(new BreakoutState());
        }

        /// <summary>
        /// Loads game content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Low res buffer for drawing the game
            _GameBuffer = new RenderTarget2D(GraphicsDevice,
                320,
                240,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            _Background = Content.Load<Texture2D>("Background");
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            InputManager.Update();

            GameManager.Update(gameTime);
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw game into low res buffer
            GraphicsDevice.SetRenderTarget(_GameBuffer);
            Batch.Begin();
            Batch.Draw(_Background, Vector2.Zero, Color.White);
            Batch.End();
            GameManager.Draw(gameTime);

            // Draw low res buffer onto screen
            GraphicsDevice.SetRenderTarget(null);
            Batch.Begin(SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone);
            Batch.Draw(_GameBuffer,
                new Rectangle(0,
                    0,
                    Graphics.PreferredBackBufferWidth,
                    Graphics.PreferredBackBufferHeight),
                Color.White);
            Batch.End();
        }
    }
}