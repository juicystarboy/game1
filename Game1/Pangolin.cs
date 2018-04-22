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
        Vector4 hitboxoffset;

        public Pangolin(Texture2D texture, Vector2 position, Color color, List<Rectangle> frames, Vector4 hitboxoffset, int framedelayamount) : base(texture, position, color, frames, hitboxoffset, framedelayamount)
        {
            this.hitboxoffset = hitboxoffset;
            initialY = (int)position.Y;

        }

        public override Rectangle hitbox
        {
            get { return new Rectangle((int)position.X + (int)hitboxoffset.X, (int)position.Y + (int)hitboxoffset.Y, frames[currentframe].Width - (int)hitboxoffset.X - (int)hitboxoffset.Z, frames[currentframe].Height - (int)hitboxoffset.Y - (int)hitboxoffset.W); }
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
            if(elapsedGameTime >= TimeSpan.FromMilliseconds(100) && grounded)
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
