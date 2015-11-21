using SFML.Graphics;
using SFML.Window;


namespace UnicycleSheep
{
    class MainMenuState : IGameState
    {
        Sprite backgroundSprite;

        public MainMenuState()
        {
            backgroundSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.MainMenuBackground));

        }

        public GameState update() 
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
