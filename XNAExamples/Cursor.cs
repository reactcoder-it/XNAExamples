/*
 * Created by SharpDevelop.
 * User: VMP
 * Date: 08.02.2013
 * Time: 21:24
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAExamples
{
	/// <summary>
	/// Description of Cursor.
	/// </summary>
	public class Cursor : DrawableGameComponent
	{
		#region Private Structures
		
		private struct TrailNode
		{
			public Vector2 Position;
			public Vector2 Velocity;
		}
		
		#endregion
		
		#region Fields and Properties
		
		private Texture2D cursorTexture;
		private Vector2 textureCenter;
		private SpriteBatch spriteBatch;
		private Vector2 position;
		private int trailNodeCount;
		private TrailNode[] trailNodes;
		
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}
		
		public float TrailStiffness { get; set; }
		public float TrailDamping { get; set; }
		public float TrailNodeMass { get; set; }
		public float CursorSpeed { get; set; }
		public float StartScale { get; set; }
		public float EndScale { get; set; }
		public float LerpExponent { get; set; }
		public Color FillColor { get; set; }
		public Color BorderColor { get; set; }
		public float BorderSize { get; set; }
		
		#endregion
		
		#region Creation and initialization
		
		public Cursor(Game game, int trailNodesNo, float stiffness, float damping)
			: base(game)
		{
			trailNodeCount = trailNodesNo;
			TrailStiffness = stiffness;
			TrailDamping = damping;
			
			trailNodes = new Cursor.TrailNode[trailNodeCount];
			CursorSpeed = 600;
			StartScale = 1.0f;
			EndScale = 0.3f;
			LerpExponent = 0.5f;
			TrailNodeMass = 11.2f;
			
			FillColor = Color.Black;
			BorderColor = Color.White;
			BorderSize = 10;
		}
		
		public Cursor(Game game, int trailNodesNo)
			: this(game, trailNodesNo, 30000, 600)
		{
		}
		
		public Cursor(Game game)
			: this(game, 50, 30000, 600)
		{
		}
		
		protected override void LoadContent()
		{
			cursorTexture = Game.Content.Load<Texture2D>("cursor");
			textureCenter = new Vector2(cursorTexture.Width / 2, cursorTexture.Height / 2);
			spriteBatch = new SpriteBatch(GraphicsDevice);
			
			base.LoadContent();
		}
		
		public override void Initialize()
		{
			base.Initialize();
			
			Viewport vp = GraphicsDevice.Viewport;
			
			position.X = vp.X + (vp.Width / 2);
			position.Y = vp.Y + (vp.Height / 2);
		}
		
		#endregion
		
		#region Draw
		
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			
			float borderStartScale = StartScale + BorderSize / cursorTexture.Width;
			float borderEndScale = EndScale + BorderSize / cursorTexture.Width;
			
			for (int i=0; i<trailNodeCount; i++)
			{
				TrailNode node = trailNodes[i];
				float lerpFactor = (float)i / (float)(trailNodeCount - 1);
				lerpFactor = (float)Math.Pow(lerpFactor, LerpExponent);
				float scale = MathHelper.Lerp(borderStartScale, borderEndScale, lerpFactor);
				
				spriteBatch.Draw(cursorTexture, node.Position, null, BorderColor, 0.0f, textureCenter, scale, SpriteEffects.None, 0.0f);
			}
			
			for (int i=0; i<trailNodeCount; i++)
			{
				TrailNode node = trailNodes[i];
				float lerpFactor = (float)i / (float)(trailNodeCount - 1);
				lerpFactor = (float)Math.Pow(lerpFactor, LerpExponent);
				float scale = MathHelper.Lerp(borderStartScale, borderEndScale, lerpFactor);
				
				spriteBatch.Draw(cursorTexture, node.Position, null, FillColor, 0.0f, textureCenter, scale, SpriteEffects.None, 0.0f);
			}
			
			spriteBatch.End();
		}
		
		#endregion
		
		#region Update
		
		Vector2 deltaMovement;
		
		private void UpdateTrailNodes(float elapsed)
		{
			for (int i=1; i<trailNodeCount; i++)
			{
				TrailNode tn = trailNodes[i];
				
				// Calculate spring force
				Vector2 stretch = tn.Position - trailNodes[i-1].Position;
				Vector2 force = -TrailStiffness * stretch - TrailDamping * tn.Velocity;
				
				// Apply acceleration
				Vector2 acceleration = force / TrailNodeMass;
				tn.Velocity += acceleration * elapsed;
				
				// Apply velocity
				tn.Position += tn.Velocity * elapsed;
				trailNodes[i] = tn;
			}
		}
		
		public override void Update(GameTime gameTime)
		{
			// First, use the GamePad to update the cursor position
			
			// Down on the thumbstick is -1. However, in screen coordinates, values
			// increase as they go down the screen. So, we have to flip the sign of the
			// y component of delta.
			deltaMovement = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
			deltaMovement.Y *= -1;
			
			#if !XBOX360
			
			// Use the mouse position as the cursor position
			MouseState mouseState = Mouse.GetState();
			position.X = mouseState.X;
			position.Y = mouseState.Y;
			
			#endif
			
			Console.WriteLine(position);
			
			// Modify position using delta, the CursorSpeed, and
			// the elapsed game time.
			position += deltaMovement * CursorSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			#if !XBOX360
			
			// Clamp the cursor position to the viewport, so that it can't move off the
			// screen.
			Viewport vp = GraphicsDevice.Viewport;
			position.X = MathHelper.Clamp(position.X, vp.X, vp.X + vp.Width);
			position.Y = MathHelper.Clamp(position.Y, vp.Y, vp.Y + vp.Height);
			
			#else
			
			// Set the new mouse position using the combination of mouse and gamepad data.
			Mouse.SetPosition((int)position.X, (int)position.Y);
			
			#endif
			
			// Set position of first trail node
			trailNodes[0].Position = position;
			
			// Update the trails
			UpdateTrailNodes((float)gameTime.ElapsedGameTime.TotalSeconds);
		}
		
		#endregion
	}
}
