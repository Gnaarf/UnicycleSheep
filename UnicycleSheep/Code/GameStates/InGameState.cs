using SFML.Graphics;


namespace UnicycleSheep
{
    class InGameState : IGameState
    {
        public GameState update() { return GameState.InGame; }
        public void draw(RenderWindow win, View view) { }
        public void drawGUI(GUI gui) { }
    }
}
