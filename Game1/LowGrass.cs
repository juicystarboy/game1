using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class LowGrass : Sprite
    {                
        

        public float speed;

        

        public LowGrass(Texture2D image, Vector2 position, float speed, Color color) : base(image, position, color)
        {
            
            this.speed = speed;
        }

        public void move()
        {
            position.X -= speed;
        }

    }
}
