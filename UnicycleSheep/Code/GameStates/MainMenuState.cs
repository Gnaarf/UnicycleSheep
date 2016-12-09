using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicycleSheep
{
    class MainMenuState : IGameState
    {
        Sprite backgroundSprite;
        Sound BackgroundMusic;
        Sound Spielanfang;

        public MainMenuState()
        {
            backgroundSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.MainMenuBackground));
            //BackgroundMusic = new Sound(new SoundBuffer("Assets/Audio/BackGround1.wav"));
            //BackgroundMusic.Loop = true;
            //BackgroundMusic.Volume = 100;
            //BackgroundMusic.Pitch = 1.0f; // Geschwindigkeit des liedes
            //BackgroundMusic.Play();
           // Spielanfang = new Sound(new SoundBuffer("Assets/Mah.m4a"));
        }

        public GameState update(float _deltaTime) 
        {
            foreach (var i in GamePadInputManager.connectedPadIndices)
            {
                if (GamePadInputManager.isClicked(GamePadButton.A, i))
                    return GameState.InGame;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                return GameState.InGame;
            }

            return GameState.MainMenu;
        }
        
        public void draw(RenderWindow win, View view) 
        {
            backgroundSprite.Scale = ((Vector2)win.Size) / ((Vector2)backgroundSprite.Texture.Size);
            win.Draw(backgroundSprite);
        }
        
        public void drawGUI(GUI gui) { }
    }
}
