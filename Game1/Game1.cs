using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D lowgrass;
        Texture2D pangolinsprite;
        Texture2D tigersprite;
        SpriteFont font;
        List<LowGrass> lowGrass;
        List<AnimatedSprite> tigers;
        KeyboardState lastKS;
        bool lost = false;
        Random rand = new Random();
        int randresult;
        int speedX = 10;
        int tigerspeed = 15;
        int score = 0;
        int scoredelay = 0;
        Pangolin pangolin;
        List<Rectangle> frames;
        List<Rectangle> tigerframes;

        TimeSpan elapsedGameTime;
        TimeSpan addNewGrass = TimeSpan.FromMilliseconds(100);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 200;
            graphics.PreferredBackBufferWidth = 1920;
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            frames = new List<Rectangle>();
            frames.Add(new Rectangle(0, 0, 154, 48));
            frames.Add(new Rectangle(154, 0, 154, 48));
            tigerframes = new List<Rectangle>();
            tigerframes.Add(new Rectangle(0, 0, 178, 100));
            tigerframes.Add(new Rectangle(178, 0, 178, 100));
            tigerframes.Add(new Rectangle(356, 0, 178, 100));
            tigerframes.Add(new Rectangle(0, 100, 178, 100));
            tigerframes.Add(new Rectangle(178, 100, 178, 100));
            tigerframes.Add(new Rectangle(356, 100, 178, 100));
            tigerframes.Add(new Rectangle(0, 200, 178, 100));
            tigerframes.Add(new Rectangle(178, 200, 178, 100));
            tigerframes.Add(new Rectangle(356, 200, 178, 100));
            pangolinsprite = Content.Load<Texture2D>("pangolin grey sprite sheet");
            tigersprite = Content.Load<Texture2D>("tigr sprite sheet");
            lowgrass = Content.Load<Texture2D>("dry grass black");
            lowGrass = new List<LowGrass>();
            tigers = new List<AnimatedSprite>();
            pangolin = new Pangolin(pangolinsprite, new Vector2(0, GraphicsDevice.Viewport.Height - pangolinsprite.Height), Color.White, frames, new Vector4(0, 0, 0, 0));
            font = Content.Load<SpriteFont>("font");
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

            KeyboardState ks = Keyboard.GetState();

            elapsedGameTime += gameTime.ElapsedGameTime;

            foreach (LowGrass b in lowGrass)
            {
                b.move();
                if (pangolin.hitbox.Intersects(b.hitbox))
                {
                    lost = true;
                }
            }
            if (lost)
            {
                pangolin.speedY = 0;
                tigerspeed = 0;
                foreach (LowGrass b in lowGrass)
                {
                    b.speed = 0;
                }
                ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.Enter))
                {
                    lowGrass.Clear();
                    tigers.Clear();
                    speedX = 10;
                    pangolin.y = 0;
                    tigerspeed = 15;
                    score = 0;
                    lost = false;
                }
            }
            if (elapsedGameTime >= addNewGrass && !lost)
            {

                lowGrass.Add(new LowGrass(lowgrass, new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - lowgrass.Height), speedX, Color.White));

                randresult = rand.Next(0, 3);
                if (randresult == 1)
                {
                    lowGrass.Add(new LowGrass(lowgrass, new Vector2(lowGrass[lowGrass.Count - 1].position.X + lowgrass.Width, lowGrass[lowGrass.Count - 1].position.Y), speedX, Color.White));
                }
                if (randresult == 2 && score >= 0)
                {
                    tigers.Add(new AnimatedSprite(tigersprite, new Vector2(1831, 130), Color.White, tigerframes, new Vector4(30, 20, 40, 30)));
                }
                elapsedGameTime = TimeSpan.Zero;
                addNewGrass = TimeSpan.FromMilliseconds(rand.Next(1000, 2000));
            }
            lastKS = ks;
            foreach (AnimatedSprite b in tigers)
            {
                if (!lost)
                {
                    b.currentframe++;
                    if (b.currentframe >= b.frames.Count)
                    {
                        b.currentframe = 0;
                    }
                }
                if (b.hitbox.Intersects(pangolin.hitbox))
                {
                    lost = true;
                }
                b.position.X -= tigerspeed;
            }
            if (!lost)
            {
                if(scoredelay >= 8)
                {
                    score++;
                    scoredelay = 0;
                }
                speedX = (score / 100) + 10;
                foreach(LowGrass b in lowGrass)
                {
                    b.speed = speedX;
                }
                tigerspeed = speedX + 10;
                pangolin.Update(gameTime);
            }
            scoredelay++;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            foreach (LowGrass b in lowGrass)
            {
                b.draw(spriteBatch);
            }
            pangolin.draw(spriteBatch);
            foreach (AnimatedSprite b in tigers)
            {
                b.draw(spriteBatch);
            }
            spriteBatch.DrawString(font, score.ToString(), new Vector2(0, 0), Color.Black);
            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
