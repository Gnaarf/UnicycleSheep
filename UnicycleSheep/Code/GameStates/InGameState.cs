using SFML.Graphics;
using Box2DX.Common;
using Box2DX.Dynamics;
using Box2DX.Collision;

namespace UnicycleSheep
{
    class InGameState : IGameState
    {
        World physicsWorld;

        //test
        PlayerCharacter playerChar;
        Actors.PolygonActor polygonAct;

        DekoElements.RemoteControllHand dekoHand;

        public InGameState()
        {
            AABB aabb = new AABB();
            aabb.LowerBound.Set(0.0f, 0.0f);
            aabb.UpperBound.Set(Constants.worldSizeX, Constants.worldSizeX * Constants.screenRatio);

            physicsWorld = new World(aabb, new Vec2(0.0f, -9.81f), false);

            playerChar = new PlayerCharacter(physicsWorld, new Vec2(5.0f, 50.0f));
            polygonAct = new Actors.PolygonActor(physicsWorld, new Vec2(0.0f, 0.0f));

            dekoHand = new DekoElements.RemoteControllHand(1);
        }
        public GameState update() 
        {
            playerChar.KeyboardInput();
            playerChar.Move();
            physicsWorld.Step(1 / 60.0f, 8, 1);
            return GameState.InGame; 
        }
        public void draw(RenderWindow win, View view) 
        {
            playerChar.draw(win, view);
            polygonAct.draw(win, view);

            dekoHand.draw(win);
        }
        public void drawGUI(GUI gui) { }
    }
}
