using SFML.Graphics;
using Box2DX.Common;
using Box2DX.Dynamics;
using Box2DX.Collision;

namespace UnicycleSheep
{
    class InGameState : IGameState
    {
        World physicsWorld;

        public InGameState()
        {
            AABB aabb = new AABB();
            physicsWorld = new World(aabb, new Vec2(0.0f, 9.81f), false);
        }
        public GameState update() 
        {
 
            return GameState.InGame; 
        }
        public void draw(RenderWindow win, View view) { }
        public void drawGUI(GUI gui) { }
    }
}
