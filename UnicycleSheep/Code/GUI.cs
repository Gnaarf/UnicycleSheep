using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace UnicycleSheep
{
    class GUI
    {
        Queue<Sprite> cachedSprites = new Queue<Sprite>();
        RenderWindow win;
        View view;

        public GUI(RenderWindow win, View view)
        {
            this.win = win;
            this.view = view;
        }

        public void draw(Sprite sprite)
        {
            // work on a copy, instead of the original, for the original could be reused outside this scope
            Sprite spriteCopy = new Sprite(sprite);

            // modify sprite, to fit it in the gui
            float viewScale = (float)view.Size.X / win.Size.X;

            spriteCopy.Scale *= viewScale;
            spriteCopy.Position = view.Center - view.Size / 2F + spriteCopy.Position * viewScale;

            // draw the sprite
            win.Draw(spriteCopy);
        }

        public void draw(Text text)
        {
            // work on a copy, instead of the original, for the original could be reused outside this scope
            Text textCopy = new Text(text);

            // modify sprite, to fit it in the gui
            float viewScale = (float)view.Size.X / win.Size.X;

            textCopy.Scale *= viewScale;
            textCopy.Position = view.Center - view.Size / 2F + textCopy.Position * viewScale;

            // draw the sprite
            win.Draw(textCopy);
        }
    }
}