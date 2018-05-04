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
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D player;
        Rectangle playerRect;
        Color color;
        Texture2D rockTexture;
        Rectangle rockRect;
        Texture2D background1;
        Texture2D background2;
        Vector2 bulletScale;
        Vector2 rockPos;
        Vector2 playerPos;
        Vector2 playerOffset;

        float speed;
        Vector2 dir;
        float scale = 0.15f;
        float rotation;
        Vector2 dirToLook;
        Vector2 position;
        static Color rockColor;
        float attackTimer;
        float attackSpeed;
        float bulletSpeed;
        float bulletDamage;
        BackgroundManager backgroundManager = new BackgroundManager();
        float backgroundSpeed;
        float rockHealth;
        Vector2 rockScale;

        static List<Bullet> bullets;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
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


            rockPos = new Vector2(100, 100);
            playerPos = new Vector2(0, 0);
            playerOffset = player.Bounds.Size.ToVector2() * 0.5f;
            rockRect = rockTexture.Bounds;
            rockRect.Size = (rockTexture.Bounds.Size.ToVector2() * 0.1f).ToPoint();
            backgroundSpeed = 500;
            rotation = 0;
            attackSpeed = 0.25f;
            attackTimer = 0;
            bulletSpeed = 1000;
            bulletDamage = 10;
            rockHealth = 50;
            bullets = new List<Bullet>();
            bulletScale = new Vector2(0.2f, 0.2f);
            rockScale = new Vector2(0.07f, 0.07f);
            rockColor = Color.White;
            backgroundManager.Initialize(new Vector2(-1, 0), background1, background2, new Vector2(0, 0), backgroundSpeed);

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
            rockTexture = Content.Load<Texture2D>("rock");

            TextureLibrary.LoadTexture(Content, "bullet");
            TextureLibrary.LoadTexture(Content, "background");
            TextureLibrary.LoadTexture(Content, "background2");
            background1 = Content.Load<Texture2D>("background");
            background2 = Content.Load<Texture2D>("background2");



        }

        public static void RemoveBullet(Bullet bullet)
        {
            bullets.Remove(bullet);

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
            backgroundManager.Update(deltaTime);
            MouseState mouseState = Mouse.GetState();
            dirToLook = mouseState.Position.ToVector2() - playerPos;



            KeyboardState keyPress = Keyboard.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && attackTimer >= attackSpeed)
            {
                Debug.Print("Yo");
                attackTimer = 0;
                bullets.Add(new Bullet(dirToLook, bulletSpeed, TextureLibrary.GetTexture("bullet"), playerPos, bulletDamage, rockRect));
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
                playerPos += (dir * pixelsToMove);
                playerRect.Location = (playerPos - playerOffset * scale).ToPoint();
            }

            if (playerRect.Intersects(rockRect))
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

        public static void ChangeRockColor(Color color)
        {
            rockColor = color;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            backgroundManager.Draw(spriteBatch);
            spriteBatch.Draw(player, playerPos, null, color, rotation + 90 * 0.0174532925f, playerOffset, scale, SpriteEffects.None, 0);
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(rockTexture, rockRect, null, rockColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
