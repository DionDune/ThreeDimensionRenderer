using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDimentionRenderer
{
    internal class inputHandler
    {
        private List<Keys> PreviouseKeys;

        public inputHandler()
        {
            PreviouseKeys = new List<Keys>();
        }



        public void execute(Settings settings, Camera camera)
        {
            List<Keys> CurrentKeys = Keyboard.GetState().GetPressedKeys().ToList();



            if (CurrentKeys.Contains(Keys.W))
                camera.MoveForward(settings);
            else if (CurrentKeys.Contains(Keys.S))
                camera.MoveBackward(settings);

            if (CurrentKeys.Contains(Keys.Q))
                camera.Rotate(settings, true);
            else if (CurrentKeys.Contains(Keys.E))
                camera.Rotate(settings, false);
            
            if (isNewPress(Keys.T, CurrentKeys) == true)
                settings.cameraRenderWireFrames = !settings.cameraRenderWireFrames;
            if (isNewPress(Keys.Y, CurrentKeys) == true)
                settings.cameraRenderFaces = !settings.cameraRenderFaces;



            PreviouseKeys = CurrentKeys;
        }

        private bool? isNewPress(Keys Key, List<Keys> CurrentKeys)
        {
            if (CurrentKeys.Contains(Key) && PreviouseKeys.Contains(Key))
                return false;
            if (CurrentKeys.Contains(Key) && !PreviouseKeys.Contains(Key))
                return true;
            else
                return null;
        }
    }
}
