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
        public Vector2 WorldPosition { get; set; }

        public int FOV { get; set; }
        public float Direction { get; set; }

        public Camera(Settings settings, Vector2 WorldPosition)
        {
            this.WorldPosition = WorldPosition;
            FOV = settings.cameraFOV;
            Direction = 0;
        }



        public void renderWorld(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Settings settings, Screen Screen, Grid Grid)
        {
            List<(GridSlot, float)> slotListOrdered = new List<(GridSlot, float)>();

            // Add initial slot
            if (Grid.SolidSlots.Count > 0)
            {
                slotListOrdered.Add((Grid.SolidSlots[0], Vector2.Distance(Grid.SolidSlots[0].Position.ToVector2(), WorldPosition)));
            }

            // Create list of slots, ordered from farthest to closest
            for (int i = 1; i < Grid.SolidSlots.Count; i++)
            {
                float SlotDistance = Vector2.Distance(Grid.SolidSlots[i].Position.ToVector2(), WorldPosition);

                for (int y = 0; y < slotListOrdered.Count; y++)
                {
                    if (SlotDistance < slotListOrdered[y].Item2 || SlotDistance == slotListOrdered[y].Item2 || y == slotListOrdered.Count - 1)
                    {
                        slotListOrdered.Insert(y, (Grid.SolidSlots[i], SlotDistance));
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
                        float HalfRenderHeight = (int)(180F / (Vector2.Distance(WorldPosition, new Vector2(Vert.X, Vert.Y)) / 100)) / 2;

                        FaceScreenPositions.Last().Add(new Point(
                            (int)(Screen.Dimentions.X * ReletiveAngle),
                            (Screen.Dimentions.Y / 2) + (int)(HalfRenderHeight * Vert.Z)
                            ));
                    }

                }

            }

            return FaceScreenPositions;
        }


        public void MoveForward(Settings settings)
        {
            WorldPosition += new Vector2(settings.cameraMovementSpeed * (float)Math.Cos((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction + (FOV / 2)) * (Math.PI / 180)));
        }
        public void MoveBackward(Settings settings)
        {
            WorldPosition -= new Vector2(settings.cameraMovementSpeed * (float)Math.Cos((Direction + (FOV / 2)) * (Math.PI / 180)),
                                            settings.cameraMovementSpeed * (float)Math.Sin((Direction + (FOV / 2)) * (Math.PI / 180)));
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
