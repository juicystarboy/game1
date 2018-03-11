﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Pangolin : AnimatedSprite
    {
        TimeSpan elapsedGameTime;
        bool grounded = true;
        public float speedY = 0.0f;
        float remainingJump = 2.0f;
        public int y = 0;
        int initialY = 0;
        public override Rectangle hitbox
        {
            get { return new Rectangle((int)position.X + 75, (int)position.Y, frames[currentframe].Width - 75, frames[currentframe].Height); }
        }

        public Pangolin(Texture2D texture, Vector2 position, Color color, List<Rectangle> frames, Vector4 hitboxoffset) : base(texture, position, color, frames, hitboxoffset)
        {
            initialY = (int)position.Y;
            
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            elapsedGameTime += gameTime.ElapsedGameTime;

            if (ks.IsKeyDown(Keys.Space))
            {
                if (grounded)
                {
                    speedY = -6f;
                    grounded = false;
                }
                else if (remainingJump > 0.0f && speedY < 0)
                {
                    remainingJump -= 0.1f;
                    speedY -= 0.4f;
                }
            }

            if (!grounded)
            {
                speedY += 0.4f;
            }

            y += (int)speedY;

            if (y >= 0 && !grounded && speedY > 0)
            {
                remainingJump = 2.0f;
                grounded = true;
                y = 0;
                speedY = 0;
            }
            if(elapsedGameTime >= TimeSpan.FromMilliseconds(250))
            {
                if(currentframe == 0)
                {
                    currentframe++;
                }
                else
                {
                    currentframe = 0;
                }

                elapsedGameTime = TimeSpan.Zero;
            }

            position.Y = initialY + y;
        }
    }
}
