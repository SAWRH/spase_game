using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    static class Screensaver
    {
        public static Texture2D Background { get; set; }
        static int time = 0;
        static Color color;
        public static Rectangle Size { get; set; }
        public static SpriteFont Font { get; set; }
        static Vector2 text_position = new Vector2(580, 360);
        static public void Draw(SpriteBatch _spriteBatch)
        {
            
            _spriteBatch.Draw(Background, Vector2.Zero,new Rectangle(0,0,1280,720), Color.White);
            
            _spriteBatch.DrawString(Font, "Press enter to continue", text_position, color);
        }
        static public void Update()
        {
            color = Color.FromNonPremultiplied(255,255,255, time % 256);
            time++;
        }

    }
}
