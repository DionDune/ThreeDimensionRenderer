using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDimentionRenderer
{
    internal class Object
    {
        public Vector3 Position { get; set; }
        public List<ObjectFace> Faces { get; set; }

        public Object(Vector3 Position)
        {
            this.Position = Position;
            Faces = new List<ObjectFace>();

            transformCube(null);
        }

        private void transformCube(Color? Color)
        {
            if (Color == null)
            {
                Random random = new Random();
                Color = new Color(random.Next(0, 63) * 4, random.Next(0, 63) * 4, random.Next(0, 63) * 4);
            }
                

            // Front Upper
            Faces.Add(new ObjectFace(
                new Vector3(1, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, 1),
                (Color)Color
                ));
            // Fron Lower
            Faces.Add(new ObjectFace(
                new Vector3(1, 0, -1),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                (Color)Color
                ));

            // Left Upper
            Faces.Add(new ObjectFace(
                new Vector3(0, 0, -1),
                new Vector3(0, 1, -1),
                new Vector3(0, 0, 1),
                (Color)Color
                ));
            // Left Lower
            Faces.Add(new ObjectFace(
                new Vector3(0, 0, 1),
                new Vector3(0, 1, -1),
                new Vector3(0, 1, 1),
                (Color)Color
                ));

            // Back Upper
            Faces.Add(new ObjectFace(
                new Vector3(0, 1, -1),
                new Vector3(1, 1, -1),
                new Vector3(1, 1, 1),
                (Color)Color
                ));
            // Back Lower
            Faces.Add(new ObjectFace(
                new Vector3(0, 1, -1),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1),
                (Color)Color
                ));

            // Right Upper
            Faces.Add(new ObjectFace(
                new Vector3(1, 1, -1),
                new Vector3(1, 0, -1),
                new Vector3(1, 1, 1),
                (Color)Color
                ));
            // Right Lower
            Faces.Add(new ObjectFace(
                new Vector3(1, 1, 1),
                new Vector3(1, 0, -1),
                new Vector3(1, 0, 1),
                (Color)Color
                ));
        }


        public static List<Object> getRandom(int Count, Vector3 Range)
        {
            Random random = new Random();
            List<Object> Objects = new List<Object>();


            for (int i = 0; i < Count; i++)
            {
                Object Obj = new Object(new Vector3(random.Next(0, (int)Range.X), random.Next(0, (int)Range.Y), random.Next(0, (int)Range.Z)));
                Objects.Add(Obj);
            }


            return Objects;
        }
    }

    internal class ObjectFace
    {
        public Vector3 Vertex1 { get; set; }
        public Vector3 Vertex2 { get; set; }
        public Vector3 Vertex3 { get; set; }

        public Color Color { get; set; }

        public ObjectFace(Vector3 Vert1, Vector3 Vert2, Vector3 Vert3, Color Color)
        {
            Vertex1 = Vert1;
            Vertex2 = Vert2;
            Vertex3 = Vert3;

            this.Color = Color;
        }
    }
}
