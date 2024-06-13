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

        public int FOV { get; set; }
        public float Direction { get; set; }

        public Camera(Settings settings, Vector3 WorldPosition)
        {
            this.WorldPosition = WorldPosition;
            FOV = settings.cameraFOV;
            Direction = 0;
        }



        public void renderWorld(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Settings settings, Screen Screen, Grid Grid, List<Object> Objects)
        {
            List<(GridSlot, float)> slotListOrdered = new List<(GridSlot, float)>();
            List<(Object, float)> objectListOrdered = new List<(Object, float)>();

            // Add initial slot
            if (Grid.SolidSlots.Count > 0)
            {
                slotListOrdered.Add((Grid.SolidSlots[0], Vector2.Distance(Grid.SolidSlots[0].Position.ToVector2(), new Vector2(WorldPosition.X, WorldPosition.Y))));
            }
            // Add initial object
            if (Grid.SolidSlots.Count > 0)
            {
                float objectDistanceX = Vector2.Distance(new Vector2(Objects[0].Position.X, Objects[0].Position.Y), new Vector2(WorldPosition.X, WorldPosition.Y));
                float objectDistance = Vector2.Distance(new Vector2(objectDistanceX, WorldPosition.Z - Objects[0].Position.Z), new Vector2(0,0));

                objectListOrdered.Add((Objects[0], objectDistance ));
            }


            // Create list of slots, ordered from farthest to closest
            for (int i = 1; i < Grid.SolidSlots.Count; i++)
            {
                float SlotDistance = Vector2.Distance(Grid.SolidSlots[i].Position.ToVector2(), new Vector2(WorldPosition.X, WorldPosition.Y));

                for (int y = 0; y < slotListOrdered.Count; y++)
                {
                    if (SlotDistance < slotListOrdered[y].Item2 || SlotDistance == slotListOrdered[y].Item2 || y == slotListOrdered.Count - 1)
                    {
                        slotListOrdered.Insert(y, (Grid.SolidSlots[i], SlotDistance));
                        break;
                    }
                }
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


            // Render faces
            for (int i = slotListOrdered.Count() - 1; i >= 0; i--)
            {
                List<List<Point>> Faces = GetFaces(Screen, slotListOrdered[i].Item1);

                if (settings.cameraRenderFaces)
                    foreach (List<Point> Face in Faces)
                    {
                        if (Face.Count == 3)
                            Game1.DrawTriangle(Game1._basicEffect, GraphicsDevice, new Vector3(Face[0].X, Face[0].Y, 0),
                                                                                new Vector3(Face[1].X, Face[1].Y, 0),
                                                                                new Vector3(Face[2].X, Face[2].Y, 0), slotListOrdered[i].Item1.Color);
                    }

                if (settings.cameraRenderWireFrames)
                    foreach (List<Point> Face in Faces)
                    {
                        if (Face.Count == 3)
                        {
                            Game1.DrawLineBetween(spriteBatch, Face[0].ToVector2(), Face[1].ToVector2(), Color.Pink, 1f);
                            Game1.DrawLineBetween(spriteBatch, Face[0].ToVector2(), Face[2].ToVector2(), Color.Pink, 1f);
                            Game1.DrawLineBetween(spriteBatch, Face[1].ToVector2(), Face[2].ToVector2(), Color.Pink, 1f);
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
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex1.X, Face.Vertex1.Y), new Vector2(Face.Vertex2.X, Face.Vertex2.Y), Color.Pink, 1f);
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex1.X, Face.Vertex1.Y), new Vector2(Face.Vertex3.X, Face.Vertex3.Y), Color.Pink, 1f);
                        Game1.DrawLineBetween(spriteBatch, new Vector2(Face.Vertex2.X, Face.Vertex2.Y), new Vector2(Face.Vertex3.X, Face.Vertex3.Y), Color.Pink, 1f);
                    }
                }
                    
            }
        }
        private List<List<Point>> GetFaces(Screen Screen, GridSlot Slot)
        {
            List<List<Point>> FaceScreenPositions = new List<List<Point>>();
            List<List<Vector3>> Faces = new List<List<Vector3>>()
            {
                // Front 1
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y, -1),
                    new Vector3(Slot.Position.X, Slot.Position.Y, -1),
                    new Vector3(Slot.Position.X, Slot.Position.Y, +1)
                },
                // Front 2
                new List<Vector3>
                {
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y, - 1),
                    new Vector3(Slot.Position.X, Slot.Position.Y, +1),
                    new Vector3(Slot.Position.X + 1 , Slot.Position.Y, +1)
                },
                //Left 1
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X, Slot.Position.Y, -1),
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X, Slot.Position.Y, +1)
                },
                //Left 2
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X, Slot.Position.Y, +1),
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, +1)
                },
                //Back 1
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, +1)
                },
                //Back 2
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, +1),
                    new Vector3(Slot.Position.X, Slot.Position.Y + 1, +1)
                },
                //Right 1
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, +1)
                },
                //Right 2
                new List<Vector3>()
                {
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y + 1, +1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y, -1),
                    new Vector3(Slot.Position.X + 1, Slot.Position.Y, +1)
                }
            };
            foreach (List<Vector3> Face in Faces)
            {
                FaceScreenPositions.Add(new List<Point>());

                foreach (Vector3 Vert in Face)
                {
                    float AngleOffset;
                    float WorldAngle = (float)Math.Atan2(Vert.Y - WorldPosition.Y, Vert.X - WorldPosition.X) * (float)(180 / Math.PI);
                    float ReletiveAngle;

                    if (WorldAngle < Direction)
                    {
                        AngleOffset = 360 - Direction;
                        ReletiveAngle = WorldAngle + AngleOffset;
                    }
                    else
                    {
                        AngleOffset = WorldAngle - Direction;
                        ReletiveAngle = AngleOffset;
                    }
                    ReletiveAngle /= FOV;

                    if (ReletiveAngle >= 0 && ReletiveAngle <= 1)
                    {
                        float HalfRenderHeight = (int)(180F / (Vector2.Distance(new Vector2(WorldPosition.X, WorldPosition.Y), new Vector2(Vert.X, Vert.Y)) / 100)) / 2;

                        FaceScreenPositions.Last().Add(new Point(
                            (int)(Screen.Dimentions.X * ReletiveAngle),
                            (Screen.Dimentions.Y / 2) + (int)(HalfRenderHeight * Vert.Z)
                            ));
                    }

                }

            }

            return FaceScreenPositions;
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
                    float AngleOffset;
                    float WorldAngle = (float)Math.Atan2(Vert.Y - WorldPosition.Y, Vert.X - WorldPosition.X) * (float)(180 / Math.PI);
                    float ReletiveAngle;

                    if (WorldAngle < Direction)
                    {
                        AngleOffset = 360 - Direction;
                        ReletiveAngle = WorldAngle + AngleOffset;
                    }
                    else
                    {
                        AngleOffset = WorldAngle - Direction;
                        ReletiveAngle = AngleOffset;
                    }
                    ReletiveAngle /= FOV;


                    if (ReletiveAngle >= 0 && ReletiveAngle <= 1)
                    {
                        float HalfRenderHeight = (int)(180F / (Vector2.Distance(new Vector2(WorldPosition.X, WorldPosition.Y), new Vector2(Vert.X, Vert.Y)) / 100)) / 2;

                        FaceVertPositions.Add(new Point(
                            (int)(Screen.Dimentions.X * ReletiveAngle),
                            (Screen.Dimentions.Y / 2) + (int)(HalfRenderHeight * Vert.Z)
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
            WorldPosition += new Vector3(settings.cameraMovementSpeed * (float)Math.Cos((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            0);
        }
        public void MoveBackward(Settings settings)
        {
            WorldPosition -= new Vector3(settings.cameraMovementSpeed * (float)Math.Cos((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            0);
        }
        public void Rotate(Settings settings, bool RotateLeft)
        {
            if (RotateLeft)
                Direction -= settings.cameraRotationSpeed;
            else
                Direction += settings.cameraRotationSpeed;

            correctRotation();
        }
        private void correctRotation()
        {
            if (Direction > 360)
            {
                Direction -= ((int)Direction / 360) * 360;
            }
            else if (Direction < 0)
            {
                while (Direction < 0)
                {
                    Direction += 360;
                }
            }
        }
    }
}
