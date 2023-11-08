using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGame
{
    class Asteroids
    {
        public static int Width, Height;
        public static Random rnd = new Random();
        static public  SpriteBatch SpriteBatch{ get; set;}
        static Stars[] stars;
        static public Ship StarShip {get;set;}
        static List<Fire> fires = new List<Fire>();        
        static Meteor[] meteor;
        public static SpriteFont ScoreFont { get; set; }
        private static int score = 0;
        private static int heatpoints = 10;
        public static int Hp
        {
            get
            {
                return heatpoints;
            }
            set{}
        }

        


        static public int GetIntRnd(int min, int max)
        {
            return rnd.Next( min, max);
        }
        static public void Shoot()
        {
            fires.Add(new Fire(StarShip.GetPosFire));
        }
       
        static public void Init(SpriteBatch SpriteBatch, int Width, int Height)
        {
            Asteroids.Width = Width;
            Asteroids.Height = Height;
            Asteroids.SpriteBatch = SpriteBatch;
            stars = new Stars[50];
            for(int i = 0; i < stars.Length; i++)
                stars[i] = new Stars(new Vector2(0,rnd.Next(1,10)));   // скорость движения Vector2(0,rnd.Next(1,10)
            StarShip = new Ship(new Vector2(640,620));
           
            
            meteor = new Meteor[7];
            for(int g = 0; g < meteor.Length; g++)
                meteor[g] = new Meteor(new Vector2(0,rnd.Next(1,10)));

            
        }
        

        static public void Update()
        {
            Collide();
            Collide2();
            StarShip.Update();
            foreach(Stars star in stars)
                star.Update();
            for(int i=0; i<fires.Count; i++)   
            {
                
                fires[i].Update();

                if(fires[i].Hide)
                {
                    fires.RemoveAt(i);
                    i--;
                }
             
            }  
            
            foreach(Meteor meteor in meteor)
                meteor.Update();
                 
            
        }
        static public void Collide()
        {
            
            for(int i=0; i<fires.Count; i++)   
            {
                
                for (int j=0; j<meteor.Length;j++)
                {
                    if (fires[i].CheckCollide(meteor[j].GetCollideRect()))
                    {
                        meteor[j]=new Meteor(new Vector2(0,rnd.Next(1,10)));
                        
                        fires[i].Hide=true;
                        score++;
                    }
                    
                }
            }
            
        }
        static public void Collide2()
        {
            for(int j=0; j<meteor.Length; j++)
            {
                if(StarShip.ShipCollide(meteor[j].GetCollideRect()))
                    {
                        meteor[j]=new Meteor(new Vector2(0,rnd.Next(1,10)));
                        heatpoints--;
    
                    }
            }
        }
        static public void Draw()
        {
            
            foreach(Stars star in stars)
                star.Draw();
            foreach(Fire fire in fires)
                fire.Draw();
            StarShip.Draw();
            foreach(Meteor meteor in meteor)
                meteor.Draw(); 
            SpriteBatch.DrawString(ScoreFont, $"Score:{score} HP:{heatpoints}", Vector2.Zero, Color.White);
            
        } 

        
    }
    class Stars
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;
        public static Texture2D Texture2D{ get; set; }
        

        public Stars(Vector2 Pos, Vector2 Dir)
        {
            this.Dir = Dir;
            this.Pos = Pos;
        }
        public Stars(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }
        public void Update()
        {
            Pos += Dir;
            if(Pos.Y > Asteroids.Height)
                RandomSet();

        }
        public void RandomSet()
        {
            
            Pos = new Vector2(Asteroids.GetIntRnd(0, Asteroids.Width), Asteroids.GetIntRnd(0,0)-300);
            color = Color.FromNonPremultiplied(Asteroids.GetIntRnd(200,255),Asteroids.GetIntRnd(200,255),Asteroids.GetIntRnd(200,255),200);
        }
        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(Texture2D, Pos,color);
        }
    }
    class Fire
    {
        Vector2 fire_position;
        Vector2 fire_direction;
        const float spdfire = 13f;
        Color color = Color.White;
        public static Texture2D fire_txt{ get; set; }
        Point size;
        int life;
        public bool fire_visible = true;
    
       
        
        public Fire(Vector2 fire_position)
        {
            life=1;
            this.fire_position = fire_position;
            this.fire_direction = new Vector2(0,spdfire);

        }
        public bool Hide // Свойство
        {
            get // Геттер - возвращает значение свойства
            {
                return (fire_position.Y < 0 || life<=0);
            }
            set // Устанавливает значение свойства. value само значение fire.Hide=true; =>  value = true
            { 
                if (value) life=0;
            }
        }
        public bool CheckCollide(Rectangle r)
        {
            Rectangle bRect = new Rectangle((int)fire_position.X,(int)fire_position.Y,fire_txt.Width,fire_txt.Height);
            if (bRect.Intersects(r))
            {
                return true;
            }
            return false;
        }
        public void Update()
        {
            
            if(fire_position.Y >= 0)
                fire_position -= fire_direction;
                
        }
        
        
        virtual public void Draw()
        {
            if(fire_visible) Asteroids.SpriteBatch.Draw(fire_txt, fire_position,color);
        }
    }
    class Ship

    {            
        
        Vector2 ship_position;
        Point ship_size;
        MouseState lastMouseState;

        Color color = Color.White;
        public static Texture2D ship_txt{ get; set; }
        public Ship(Vector2 ship_position)
        {
                           
            this.ship_position = ship_position;
        
        }

        public Vector2 GetPosFire => new Vector2(ship_position.X + 26, ship_position.Y);
        
        public void Update()
        {

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)
                ship_position = new Vector2(currentMouseState.X, currentMouseState.Y);
            lastMouseState = currentMouseState;

            if(this.ship_position.X < 0)
                this.ship_position.X = 0;
            if(this.ship_position.X > 1211)
                this.ship_position.X = 1211;
            if(this.ship_position.Y < 0)
                this.ship_position.Y = 0;
            if(this.ship_position.Y > 620)
                this.ship_position.Y = 620;
        }
        
        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(ship_txt, ship_position,color);
        
        }
        public bool ShipCollide(Rectangle r)
        {
            Rectangle shp_rec = new Rectangle((int)ship_position.X, (int)ship_position.Y,ship_txt.Width,ship_txt.Height);
            if (shp_rec.Intersects(r))
            {
                return true;
            }
            return false;
        }
    }
    class Meteor
    {
        Vector2 pos;
        
        Vector2 dir;
        Color color;
        Point size;
        public bool visible = true;
        
       
        public static Texture2D meteor_txt{ get; set; }
        
        
       
        public Meteor(Vector2 pos, Vector2 dir)
        {
            this.dir = dir;
            this.pos = pos;
        }
        public Meteor(Vector2 dir)
        {
            this.dir = dir;
            RandomSet();
        }
        public Rectangle GetCollideRect()
        {
            Rectangle r = new Rectangle((int)pos.X,(int)pos.Y,meteor_txt.Width,meteor_txt.Height);
            return r;
        }
        public void Update()
        {
            pos += dir;
            if(pos.Y > Asteroids.Height)
                RandomSet();

        }
        public void RandomSet()
        {
            
            pos = new Vector2(Asteroids.GetIntRnd(0, Asteroids.Width), Asteroids.GetIntRnd(0,0)-300);
            color = Color.White;
        }
        public void Draw()
        {
            if (visible) Asteroids.SpriteBatch.Draw(meteor_txt, pos,Color.White);
        }
        
    }
    
}
