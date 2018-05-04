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
    class Bullet
    {
        Vector2 dir;
        float speed;
        Texture2D texture;
        Rectangle rectangle;
        Rectangle rockRectangle;
        Vector2 position;
        Vector2 scale;
        Vector2 offset;
       

        
        float damage;
        public Bullet(Vector2 Bulletdir, float bulletSpeed, Texture2D bulletTexture, Vector2 startPosition, float bulletDamage, Rectangle rockRect)
        {
            
            dir = Bulletdir;
            rockRectangle = rockRect;
            damage = bulletDamage;
            
            dir.Normalize();
            speed = bulletSpeed;
            texture = bulletTexture;
            position = startPosition;
            scale = new Vector2(0.03f, 0.03f);
            offset = texture.Bounds.Size.ToVector2() * 0.2f;
            rectangle.Size = (texture.Bounds.Size.ToVector2().ToPoint());
           



        }

        public void Update(float deltaTime)
        {
            position += dir * speed * deltaTime;
            rectangle.Location = position.ToPoint();
            if (rectangle.Intersects(rockRectangle))
            {
                Game1.ChangeRockColor(Color.Red);
                Game1.RemoveBullet(this);
                
            }
            else
            {
                Game1.ChangeRockColor(Color.White);
            }
            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, offset,scale,SpriteEffects.None, 0);

        }

















    }
}
