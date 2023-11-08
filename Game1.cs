using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    enum Stat
    {
        Screensaver,
        Game,
        Pause,
        Final
    }
    public class Game1 : Game
    {
        Stat Stat = Stat.Screensaver;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState lastMouseState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            _graphics.IsFullScreen = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Screensaver.Background = Content.Load<Texture2D>("Background");
            Screensaver.Font = Content.Load<SpriteFont>("Text");
            Asteroids.ScoreFont = Content.Load<SpriteFont>("Score");
            Stars.Texture2D = Content.Load<Texture2D>("Star");
            Ship.ship_txt = Content.Load<Texture2D>("Ship");
            Fire.fire_txt = Content.Load<Texture2D>("Fireball");
            Meteor.meteor_txt = Content.Load<Texture2D>("Meteor");
            
            Asteroids.Init(_spriteBatch,_graphics.PreferredBackBufferWidth,_graphics.PreferredBackBufferHeight);
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KeyboardState = Keyboard.GetState();
            
            switch (Stat)
            {
                case Stat.Screensaver:
                    Screensaver.Update();
                    if(KeyboardState.IsKeyDown(Keys.Enter))
                        
                        Stat = Stat.Game;
                    
                    break;
                case Stat.Game:
                    Asteroids.Update();
                    
                    if(KeyboardState.IsKeyDown(Keys.Escape)||Asteroids.Hp<=0)
                        
                        Stat = Stat.Screensaver;
                    break;
            }
            MouseState currentMouseState = Mouse.GetState();
            if(lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                Asteroids.Shoot();
                lastMouseState = currentMouseState;
     
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin();
            switch (Stat)
            {
                case Stat.Screensaver:
                    Screensaver.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    Asteroids.Draw();
                    break;
            }
            
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
