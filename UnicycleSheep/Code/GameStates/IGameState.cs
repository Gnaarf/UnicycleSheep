using SFML.Graphics;

namespace UnicycleSheep
{
    interface IGameState
    {
        GameState update();
        void draw(RenderWindow win, View view);
        void drawGUI(GUI gui);
    }
}
