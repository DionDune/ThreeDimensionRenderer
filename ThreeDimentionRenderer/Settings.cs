using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace ThreeDimentionRenderer
{
    internal class Settings
    {
        public Vector2 cameraFOV { get; set; }
        public bool cameraRenderWireFrames { get; set; }
        public bool cameraRenderFaces { get; set; }
        public float cameraMovementSpeed { get; set; }
        public float cameraRotationSpeed { get; set; }

        public bool objectsRandomPopulation { get; set; }
        public int objectsRandomCount { get; set; }
        public Vector3 objectSpawnRange { get; set; }

        public Settings()
        {
            cameraFOV = new Vector2(120, 66);
            cameraMovementSpeed = 0.5f;
            cameraRotationSpeed = 1.2f;

            cameraRenderWireFrames = true;
            cameraRenderFaces = true;

            objectsRandomPopulation = true;
            objectsRandomCount = 1000;
            objectSpawnRange = new Vector3(300, 300, 0);
        }
    }
}
