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
        int pb = 0;
        int scoredelay = 0;
        Pangolin pangolin;
        List<Rectangle> frames;
        List<Rectangle> tigerframes;
        TimeSpan frameupdate;
        TimeSpan elapsedGameTime;
        TimeSpan addNewGrass = TimeSpan.FromMilliseconds(100);
        int tigerscore = 400;
        int nightscore = 700;
        Color color;
        Color background = Color.White;
        public float lerpAmount = .01f;
        Color nightColor;
        Color originalColor;
        Color nightBackgroundColor;
        Color originalBackgroundColor;

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
            pangolinsprite = Content.Load<Texture2D>("pangolin white sprite sheet");
            tigersprite = Content.Load<Texture2D>("tigr sprite sheet");
            lowgrass = Content.Load<Texture2D>("dry grass white");
            lowGrass = new List<LowGrass>();
            tigers = new List<AnimatedSprite>();
            pangolin = new Pangolin(pangolinsprite, new Vector2(0, GraphicsDevice.Viewport.Height - pangolinsprite.Height), Color.White, frames, new Vector4(75, 0, 20, 10), 0);
            font = Content.Load<SpriteFont>("font");
            color = new Color(53, 53, 53);
            originalColor = color;
            nightColor = Color.White;
            originalBackgroundColor = background;
            nightBackgroundColor = Color.Black;
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
            frameupdate += gameTime.ElapsedGameTime;

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
                    if (score > pb)
                    {
                        pb = score;
                    }
                    lowGrass.Clear();
                    tigers.Clear();
                    speedX = 10;
                    pangolin.y = 0;
                    tigerspeed = 15;
                    score = 0;
                    lost = false;
                    color = new Color(53, 53, 53);
                    background = Color.White;
                    lerpAmount = 0.1f;
                }
            }
            if (elapsedGameTime >= addNewGrass && !lost)
            {

                randresult = rand.Next(0, 3);
                if(randresult == 0 || randresult == 1)
                {
                    lowGrass.Add(new LowGrass(lowgrass, new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - lowgrass.Height), speedX, color));
                }
                if (randresult == 1)
                {
                    lowGrass.Add(new LowGrass(lowgrass, new Vector2(lowGrass[lowGrass.Count - 1].position.X + lowgrass.Width, lowGrass[lowGrass.Count - 1].position.Y), speedX, color));
                }
                else if (randresult == 2 && score >= tigerscore)
                {
                    tigers.Add(new AnimatedSprite(tigersprite, new Vector2(1831, 130), color, tigerframes, new Vector4(30, 20, 40, 30), 5 - score/200));
                }
                elapsedGameTime = TimeSpan.Zero;
                addNewGrass = TimeSpan.FromMilliseconds(rand.Next(1000, 2000));
            }
            lastKS = ks;
            foreach (AnimatedSprite b in tigers)
            {
                if (!lost && b.framedelay >= b.framedelayamount)
                {
                    b.currentframe++;
                    if (b.currentframe >= b.frames.Count)
                    {
                        b.currentframe = 0;
                    }
                    b.framedelay = 0;
                }
                if (b.hitbox.Intersects(pangolin.hitbox))
                {
                    lost = true;
                }
                b.position.X -= tigerspeed;
                b.framedelay++;
            }
            if (!lost)
            {
                if(scoredelay >= 8)
                {
                    score++;
                    scoredelay = 0;
                }
                speedX = (score / 50) + 10;
                foreach(LowGrass b in lowGrass)
                {                    
                    b.speed = speedX;
                }
                tigerspeed = speedX + 5;
                pangolin.color = color;
                pangolin.Update(gameTime);
            }
            if(score >= nightscore)
            { 
                if (lerpAmount <= 1)
                {
                    color = Color.Lerp(originalColor, nightColor, lerpAmount);
                    background = Color.Lerp(originalBackgroundColor, nightBackgroundColor, lerpAmount);
                    lerpAmount += .01f;
                }
                //pangolin.texture = Content.Load<Texture2D>("pangolin white sprite sheet");
                foreach (LowGrass b in lowGrass)
                {
                    //b.texture = Content.Load<Texture2D>("dry grass white");
                    b.color = color;
                }
                /*foreach(AnimatedSprite b in tigers)
                {
                    b.texture = Content.Load<Texture2D>("tigr sprite sheet inverted");
                    
                }*/                
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
            GraphicsDevice.Clear(background);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            foreach(LowGrass b in lowGrass)
            {
                b.draw(spriteBatch);
                /*if (b.position.X != b.prevpos.X || b.speed == 0)
                {
                    b.draw(spriteBatch);
                }*/
            }
            foreach (AnimatedSprite b in tigers)
            {
                b.draw(spriteBatch);
            }
            pangolin.draw(spriteBatch);
            spriteBatch.DrawString(font, "Score:", new Vector2(1700, 0), color);
            spriteBatch.DrawString(font, score.ToString(), new Vector2(1700 + font.MeasureString("Score:").X + 2, 0), color);
            spriteBatch.DrawString(font, "Best:", new Vector2(1800, 0), color);
            spriteBatch.DrawString(font, pb.ToString(), new Vector2(1800 + font.MeasureString("Best:").X + 2, 0), color);
            //spriteBatch.Draw(lowgrass, pangolin.hitbox, Color.White);
            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
