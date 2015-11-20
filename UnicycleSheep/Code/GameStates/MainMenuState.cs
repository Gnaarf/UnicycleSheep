using SFML.Graphics;


namespace UnicycleSheep
{
    class MainMenuState : IGameState
    {
        public GameState update() { return GameState.MainMenu; }
        public void draw(RenderWindow win, View view) { }
        public void drawGUI(GUI gui) { }
    }
}
