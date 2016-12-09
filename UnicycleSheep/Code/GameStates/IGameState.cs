using SFML.Graphics;

namespace UnicycleSheep
{
    interface IGameState
    {
        GameState update(float _deltaTime);
        void draw(RenderWindow win, View view);
        void drawGUI(GUI gui);
    }
}
