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
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return) || (GamePadInputManager.isConnected(0) && GamePadInputManager.isClicked(GamePadButton.A, 0)))
            {
                return GameState.InGame;
            }

            return GameState.MainMenu;
        }
        
        public void draw(RenderWindow win, View view) 
        {
            win.Draw(backgroundSprite);
        }
        
        public void drawGUI(GUI gui) { }
    }
}
