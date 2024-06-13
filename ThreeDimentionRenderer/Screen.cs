using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDimentionRenderer
{
    internal class Screen
    {
        public Point Position { get; set; }
        public Point Dimentions { get; set; }

        public SpriteBatch spriteBatch { get; set; }

        public Screen(Point Dimentions, SpriteBatch _spritebatch)
        {
            this.spriteBatch = _spritebatch;
            this.Dimentions = Dimentions;

            Position = new Point(0, 0);
        }
    }
}
