using SFML.Graphics;
using SFML.Window;


namespace UnicycleSheep
{
    class MainMenuState : IGameState
    {
        Sprite backgroundSprite;

        public MainMenuState()
        {
            backgroundSprite = new Sprite(new Texture("Assets/Textures/Background.png"));
        }

        public GameState update() 
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return) || (Program.gamePadInputManager.isConnected(0) && Program.gamePadInputManager.isClicked(GamePadButton.A, 0)))
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
