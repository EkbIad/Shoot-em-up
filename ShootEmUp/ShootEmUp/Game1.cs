using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShootEmUp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D player;
        Rectangle playerRect;
        Color color;
        Texture2D bushTexture;
        Rectangle bushRect;
        Vector2 bulletScale;
        
        float speed;
        Vector2 dir;
        float scale = 0.3f;
        float rotation;
        Vector2 dirToLook;
        Vector2 position;
        float attackTimer;
        float attackSpeed;
        float bulletSpeed;

        List<Bullet> bullets;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
         
            IsMouseVisible = true;
            playerRect = player.Bounds;
            speed = 600;
            position = new Vector2(200, 200);



            playerRect.Size = (playerRect.Size.ToVector2() * scale).ToPoint();
            bushRect = bushTexture.Bounds;
            rotation = 0;
            attackSpeed = 0.25f;
            attackTimer = 0;
            bulletSpeed = 1000;
            bullets = new List<Bullet>();
            bulletScale = new Vector2(0.2f, 0.2f);

        }   

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player = Content.Load<Texture2D>("player");
            bushTexture = Content.Load<Texture2D>("bush");
            TextureLibrary.LoadTexture(Content,"bullet");
           


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

            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            attackTimer += deltaTime;
            float pixelsToMove = speed * deltaTime;
            dir = new Vector2();
            
            MouseState mouseState = Mouse.GetState();
            position = playerRect.Location.ToVector2();
            dirToLook = mouseState.Position.ToVector2() - position;
           

            KeyboardState keyPress = Keyboard.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed && attackTimer >= attackSpeed)
            {
                Debug.Print("Yo");
                attackTimer = 0;
                bullets.Add(new Bullet(dirToLook, bulletSpeed, TextureLibrary.GetTexture("bullet"), position));
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(deltaTime);
            }
            if (keyPress.IsKeyDown(Keys.D))
            {
                dir = new Vector2(1, 0);
            }
            if (keyPress.IsKeyDown(Keys.A))
            {
                dir = new Vector2(-1, 0);
            }
            if (keyPress.IsKeyDown(Keys.W))
            {
                dir.Y = -1;
            }
            if (keyPress.IsKeyDown(Keys.S))
            {
                dir.Y = 1;
            }
            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                playerRect.Location += (dir * pixelsToMove).ToPoint();
            }

            if (playerRect.Intersects(bushRect))
            {
                color = Color.Red;
            }
            else
            {
                color = Color.White;    
            }

            rotation = (float)Math.Atan2(dirToLook.Y, dirToLook.X);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(player, playerRect, null, color, rotation, new Vector2(player.Bounds.Width/3f, player.Bounds.Height/3f), SpriteEffects.None, 0);
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(bushTexture, bushRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
