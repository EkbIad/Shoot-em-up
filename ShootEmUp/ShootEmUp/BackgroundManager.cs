using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ShootEmUp
{
    class BackgroundManager
    {
         Texture2D background1;
         Texture2D background2;
        Vector2 dir;
        Vector2 pos;
        Vector2 pos2;
       
        float speed;

        public void Initialize(Vector2 backgroundDir, Texture2D bg1, Texture2D bg2, Vector2 position, float bgSpeed)
        {
            background1 = bg1;
            background2 = bg2;
            dir = backgroundDir;
            pos = position;
            pos2 = new Vector2(position.X + bg1.Width, position.Y);
            speed = bgSpeed;
        }

        public void Update(float deltatime)
        {
            pos += dir * deltatime * speed;
            pos2 += dir * deltatime * speed;

            if(pos2.X < 0)
            {
                pos.X = pos2.X + background2.Width;
              
            }
            if(pos.X < 0)
            {
                pos2.X = pos.X + background1.Width;

            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background1, pos, Color.White);
            spriteBatch.Draw(background2, pos2, Color.White);
        }







    }
}
