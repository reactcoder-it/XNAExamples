/*
 * Created by SharpDevelop.
 * User: VMP
 * Date: 17.02.2013
 * Time: 21:07
 */
using System;

using SharpDX;
using SharpDX.Toolkit;

namespace CastleGameV
{
	using SharpDX.Toolkit.Graphics;
	
	/// <summary>
	/// Description of Game.
	/// </summary>
	public class MiniGame : Game
	{
		private GraphicsDeviceManager graphicsDeviceManager;
		private BasicEffect basicEffect;
		private Buffer<VertexPositionColor> vertices;
		private VertexInputLayout inputLayout;
		
		public MiniGame() {
			graphicsDeviceManager = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";
		}
		
		protected override void LoadContent() {
			// Creates a basic effect
			basicEffect = new BasicEffect(GraphicsDevice) {
				VertexColorEnabled = true,
				View = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY),
				Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
				World = Matrix.Identity
			};
			// Creates vertices for the cube
			vertices = Buffer.Vertex.New(
				GraphicsDevice,
				new[] {
					new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
					new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed), // Top
                    new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed), // Bottom
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange), // Left
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange), // Right
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
                    new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange)
				}
			);
			// Create an input layout from the vertices
			inputLayout = VertexInputLayout.FromBuffer(0, vertices);
			base.LoadContent();
		}
		
		protected override void Initialize() {
			Window.Title = "MiniCube demo";
			base.Initialize();
		}
		
		protected override void Update(GameTime gameTime) {
			// Rotate the cube
			var time = (float)gameTime.TotalGameTime.TotalSeconds;
			basicEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
			// Handle base.Update
			base.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime) {
			// Clears the screen with the Color.CornflowerBlue
			GraphicsDevice.Clear(Color.CornflowerBlue);
			// Setup the vertices
			GraphicsDevice.SetVertexBuffer(vertices);
			GraphicsDevice.SetVertexInputLayout(inputLayout);
			// Apply the basic effect technique and draw the rotating cube
			basicEffect.CurrentTechnique.Passes[0].Apply();
			GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
			// Handle base.Draw
			base.Draw(gameTime);
		}
	}
}
