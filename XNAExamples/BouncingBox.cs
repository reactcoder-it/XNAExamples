/*
 * Created by SharpDevelop.
 * User: VMP
 * Date: 04.02.2013
 * Time: 18:05
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAExamples
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class BouncingBox : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont spriteFont;
		Texture2D texture;
		Vector2 position;
		Vector2 speed;
		Random randomizer;
		Color backColor;
		Cursor cursor;
		
		int frameRate = 0;
		int frameCounter = 0;
		TimeSpan elapsedTime = TimeSpan.Zero;
		
		public BouncingBox()
		{
			randomizer = new Random(DateTime.Now.TimeOfDay.Milliseconds);
			speed = new Vector2(5 + randomizer.Next(10), 5 + randomizer.Next(10));
			position = new Vector2(250, 400);
			GetNewColor();
			
			graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			
			cursor = new Cursor(this, 10);
			cursor.BorderColor = Color.White;
			cursor.FillColor = Color.Black;
			Components.Add(cursor);
			
			graphics.IsFullScreen = false;
		}
		
		private void GetNewColor()
		{
			backColor = new Color((byte)randomizer.Next(255),
			                      (byte)randomizer.Next(255),
			                      (byte)randomizer.Next(255), 255);
		}
		
		/// <summary>
		/// Allows the game to perform any initialization it need to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize() will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			
			base.Initialize();
		}
		
		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			
			texture = Content.Load<Texture2D>("monogameicon");
			spriteFont = Content.Load<SpriteFont>("spritefont");
		}
		
		/// <summary>
		/// Unload content will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}
		
		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides of snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape)) this.Exit();
			
			// FPS Counter
			elapsedTime += gameTime.ElapsedGameTime;
			if (elapsedTime > TimeSpan.FromSeconds(1))
			{
				elapsedTime -= TimeSpan.FromSeconds(1);
				frameRate = frameCounter;
				frameCounter = 0;
			}
			
			if (texture != null)
			{
				// Right
				if (position.X + texture.Width + speed.X > Window.ClientBounds.Width)
				{
					GetNewColor();
					speed.X = -speed.X;
				}
				
				// Bottom
				if (position.Y + texture.Height + speed.Y > Window.ClientBounds.Height)
				{
					GetNewColor();
					speed.Y = -speed.Y;
				}
				
				// Left
				if (position.X + speed.X < 0)
				{
					GetNewColor();
					speed.X = -speed.X;
				}
				
				// Top
				if (position.Y + speed.Y < 0)
				{
					GetNewColor();
					speed.Y = -speed.Y;
				}
				
				// Update position
				position += speed;
			}
			
			base.Update(gameTime);
		}
		
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides of snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(backColor);
			
			frameCounter++;
			
			string fps = string.Format("fps: {0}", frameRate);
			
			spriteBatch.Begin();
			if (texture != null)
				spriteBatch.Draw(texture, position, Color.White);
			spriteBatch.DrawString(spriteFont, fps, new Vector2(1,1), Color.Black);
			spriteBatch.DrawString(spriteFont, fps, new Vector2(0,0), Color.White);
			spriteBatch.End();
			
			base.Draw(gameTime);
		}
	}
}
