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
        public int cameraFOV { get; set; }
        public bool cameraRenderWireFrames { get; set; }
        public bool cameraRenderFaces { get; set; }
        public float cameraMovementSpeed { get; set; }
        public float cameraRotationSpeed { get; set; }

        public Point gridDimentions { get; set; }
        public bool gridRandomPopulated { get; set; }
        public uint gridRandomPlaceChange { get; set; }
        public bool gridHadDefault { get; set; }

        public Settings()
        {
            cameraFOV = 120;
            cameraMovementSpeed = 0.5f;
            cameraRotationSpeed = 1.2f;

            cameraRenderWireFrames = true;
            cameraRenderFaces = true;


            gridDimentions = new Point(1000, 1000);
            gridRandomPopulated = true;
            gridRandomPlaceChange = 1000;
            gridHadDefault = true;
        }
    }
}
