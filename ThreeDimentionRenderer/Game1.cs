using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ThreeDimentionRenderer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static BasicEffect _basicEffect;

        Random random;
        public static Texture2D TextureWhite;

        Settings settings;
        inputHandler inputHandler;
        Grid Grid;
        Screen Screen;
        Camera Camera;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random();
            settings = new Settings();
            inputHandler = new inputHandler();
            Grid = new Grid(settings);
            Screen = new Screen(new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), _spriteBatch);
            Camera = new Camera(settings, new Vector2(10, 10));

            _basicEffect = new BasicEffect(GraphicsDevice) { VertexColorEnabled = true };
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            TextureWhite = new Texture2D(GraphicsDevice, 1, 1);
            TextureWhite.SetData(new Color[1] { Color.White });
        }



        public static void DrawLine(SpriteBatch spritebatch, Vector2 point, float Length, float Angle, Color Color, float Thickness)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            spritebatch.Draw(TextureWhite, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }
        public static void DrawLineBetween(SpriteBatch spritebatch, Vector2 Point1, Vector2 Point2, Color Color, float Thickness)
        {
            Vector2 DistanceVector = new Vector2(
                Point2.X - Point1.X,
                Point2.Y - Point1.Y
                );
            float Angle = (float)Math.Atan2(DistanceVector.Y, DistanceVector.X);
            float DistanceValue = Math.Abs(Vector2.Distance(Point1, Point2));

            DrawLine(spritebatch, Point1, DistanceValue, Angle, Color, Thickness);
        }

        public static void DrawTriangle(BasicEffect _basicEffect, GraphicsDevice GraphicsDevice, Vector3 Pos1, Vector3 Pos2, Vector3 Pos3, Color tint)
        {
            VertexPositionColor[] _vertexPositionColors = new[]
            {
                new VertexPositionColor(Pos1, tint),
                new VertexPositionColor(Pos2, tint),
                new VertexPositionColor(Pos3, tint)
            };

            _basicEffect.World = Matrix.CreateOrthographicOffCenter(
                0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertexPositionColors, 0, 1);
        }





        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            inputHandler.execute(settings, Camera);



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();


            Camera.renderWorld(_spriteBatch, GraphicsDevice, settings, Screen, Grid);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}