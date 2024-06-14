using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDimentionRenderer
{
    internal class Camera
    {
        public Vector3 WorldPosition { get; set; }

        public Vector2 FOV { get; set; }
        public Vector2 Direction { get; set; }

        public Camera(Settings settings, Vector3 WorldPosition)
        {
            this.WorldPosition = WorldPosition;
            FOV = settings.cameraFOV;
            Direction = new Vector2(0,90);
        }



        public void renderWorld(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Settings settings, Screen Screen, List<Object> Objects)
        {
            List<(Object, float)> objectListOrdered = new List<(Object, float)>();

            // Add initial object
            if (Objects.Count > 0)
            {
                float objectDistanceX = Vector2.Distance(new Vector2(Objects[0].Position.X, Objects[0].Position.Y), new Vector2(WorldPosition.X, WorldPosition.Y));
                float objectDistance = Vector2.Distance(new Vector2(objectDistanceX, WorldPosition.Z - Objects[0].Position.Z), new Vector2(0,0));

                objectListOrdered.Add((Objects[0], objectDistance ));
            }

            // Create list of objects, ordered from farthest to closest
            for (int i = 1; i < Objects.Count; i++)
            {
                float objectDistanceX = Vector2.Distance(new Vector2(Objects[i].Position.X, Objects[i].Position.Y), new Vector2(WorldPosition.X, WorldPosition.Y));
                float objectDistance = Vector2.Distance(new Vector2(objectDistanceX, WorldPosition.Z - Objects[i].Position.Z), new Vector2(0, 0));

                for (int y = 0; y < objectListOrdered.Count; y++)
                {
                    if (objectDistance < objectListOrdered[y].Item2 || objectDistance == objectListOrdered[y].Item2 || y == objectListOrdered.Count - 1)
                    {
                        objectListOrdered.Insert(y, (Objects[i], objectDistance));
                        break;
                    }
                }
            }

            // Render object faces
            for (int i = objectListOrdered.Count() - 1; i >= 0; i--)
            {
                List<ObjectFace> Faces = GetObjectFaces(Screen, objectListOrdered[i].Item1);

                if (settings.cameraRenderFaces)
                    foreach (ObjectFace Face in Faces)
                    {
                        Game1.DrawTriangle(Game1._basicEffect, GraphicsDevice, new Vector3(Face.Vertex1.X, Face.Vertex1.Y, 0),
                                                                            new Vector3(Face.Vertex2.X, Face.Vertex2.Y, 0),
                                                                            new Vector3(Face.Vertex3.X, Face.Vertex3.Y, 0), Face.Color);
                    }

                if (settings.cameraRenderWireFrames)
                {
                    foreach (ObjectFace Face in Faces)
                    {
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex1.X, Face.Vertex1.Y), new Vector2(Face.Vertex2.X, Face.Vertex2.Y), Color.White, 1f);
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex1.X, Face.Vertex1.Y), new Vector2(Face.Vertex3.X, Face.Vertex3.Y), Color.White, 1f);
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex2.X, Face.Vertex2.Y), new Vector2(Face.Vertex3.X, Face.Vertex3.Y), Color.White, 1f);
                    }
                }
            }
        }
        private List<ObjectFace> GetObjectFaces(Screen Screen, Object Object)
        {
            List<ObjectFace> FaceScreenPositions = new List<ObjectFace>();

            foreach (ObjectFace Face in Object.Faces)
            {
                List<Vector3> FaceVerts = new List<Vector3>()
                {
                    Face.Vertex1 + Object.Position,
                    Face.Vertex2 + Object.Position,
                    Face.Vertex3 + Object.Position
                };
                List<Point> FaceVertPositions = new List<Point>();




                foreach (Vector3 Vert in FaceVerts)
                {
                    //X
                    float WorldAngleX = (float)Math.Atan2(Vert.Y - WorldPosition.Y, Vert.X - WorldPosition.X) * (float)(180 / Math.PI);

                    float AngleOffset;
                    float ReletiveAngle;

                    if (WorldAngleX < Direction.X)
                    {
                        AngleOffset = 360 - Direction.X;
                        ReletiveAngle = WorldAngleX + AngleOffset;
                    }
                    else
                    {
                        AngleOffset = WorldAngleX - Direction.X;
                        ReletiveAngle = AngleOffset;
                    }
                    ReletiveAngle /= FOV.X;


                    // Y
                    float DistanceX = Vector2.Distance(new Vector2(Vert.X, Vert.Y), new Vector2(WorldPosition.X, WorldPosition.Y));
                    float DifferenceZ = WorldPosition.Z - Vert.Z;
                    float WorldAngleY = (float)Math.Acos((double)(DifferenceZ / DistanceX)) * (float)(180 / Math.PI);

                    float AngleOffsetY = (Direction.Y - (FOV.Y / 2));
                    float ReletiveAngleY = WorldAngleY - AngleOffsetY;

                    ReletiveAngleY /= FOV.Y;


                    if (ReletiveAngle >= -0.3 && ReletiveAngle <= 1.3 &&
                        ReletiveAngleY >= -0.3 && ReletiveAngleY <= 1.3)
                    {
                        FaceVertPositions.Add(new Point(
                            (int)(Screen.Dimentions.X * ReletiveAngle),
                            (int)(Screen.Dimentions.Y * ReletiveAngleY)
                            ));
                    }
                }




                if (FaceVertPositions.Count == 3)
                    FaceScreenPositions.Add(new ObjectFace(
                        new Vector3(FaceVertPositions[0].X, FaceVertPositions[0].Y, 0),
                        new Vector3(FaceVertPositions[1].X, FaceVertPositions[1].Y, 0),
                        new Vector3(FaceVertPositions[2].X, FaceVertPositions[2].Y, 0),
                        Face.Color));
            }

            return FaceScreenPositions;
        }


        public void MoveForward(Settings settings)
        {
            WorldPosition += new Vector3(settings.cameraMovementSpeed * (float)Math.Cos((Direction.X + (FOV.X / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction.X + (FOV.X / 2)) * (Math.PI / 180)),
                                            0);
        }
        public void MoveBackward(Settings settings)
        {
            WorldPosition -= new Vector3(settings.cameraMovementSpeed * (float)Math.Cos((Direction.X + (FOV.X / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction.X + (FOV.X / 2)) * (Math.PI / 180)),
                                            0);
        }
        public void MoveUpward(Settings settings)
        {
            WorldPosition -= new Vector3(0,0,settings.cameraMovementSpeed);
        }
        public void MoveDownward(Settings settings)
        {
            WorldPosition += new Vector3(0, 0, settings.cameraMovementSpeed);
        }
        public void RotateHorizontal(Settings settings, bool RotateLeft)
        {
            if (RotateLeft)
                Direction -= new Vector2(settings.cameraRotationSpeed, 0);
            else
                Direction += new Vector2(settings.cameraRotationSpeed, 0);

            correctRotation();
        }
        public void RotateVertical(Settings settings, bool RotateUp)
        {
            if (RotateUp)
                Direction -= new Vector2(0, settings.cameraRotationSpeed);
            else
                Direction += new Vector2(0, settings.cameraRotationSpeed);

            correctRotation();
        }
        private void correctRotation()
        {
            if (Direction.X > 360)
            {
                Direction -= new Vector2( ((int)Direction.X / 360) * 360, 0);
            }
            else if (Direction.X < 0)
            {
                while (Direction.X < 0)
                {
                    Direction += new Vector2(360, 0);
                }
            }

            if (Direction.Y > 180)
                Direction = new Vector2(Direction.X, 180);
            else if (Direction.Y < 0)
                Direction = new Vector2(Direction.X, 0);
        }
    }
}
