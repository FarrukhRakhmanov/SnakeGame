using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Components
{
    public static class Fonts
    {
        public static SpriteFont MyFont { get; private set; }

        public static void Load(ContentManager content)
        {
            MyFont = content.Load<SpriteFont>("Fonts/MyFont");
        }
    }
}
