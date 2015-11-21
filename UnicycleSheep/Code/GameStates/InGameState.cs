using SFML.Graphics;
using Box2DX.Common;
using Box2DX.Dynamics;
using Box2DX.Collision;

namespace UnicycleSheep
{
    class InGameState : IGameState
    {
        //singletons
        World physicsWorld;
        Physics.ContactManager contactManager;

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

            contactManager = new Physics.ContactManager();
            physicsWorld.SetContactListener(contactManager);

            playerChar = new PlayerCharacter(physicsWorld, new Vec2(5.0f, 50.0f));
            polygonAct = new Actors.PolygonActor(physicsWorld, new Vec2(0.0f, 0.0f), 0xF0A58A4, Actors.FunctionType.GradientNoise, 4);

            dekoHand = new DekoElements.RemoteControllHand(1);

            //left and right borders of the map
            BodyDef bodydef = new BodyDef();
            bodydef.Position = new Vector2(0,0);
            bodydef.Angle = 0.0f;
            PolygonDef box = new PolygonDef();
            box.SetAsBox(1f, Constants.worldSizeY);

            Body leftEdge = physicsWorld.CreateBody(bodydef); leftEdge.CreateShape(box);
            bodydef.Position = new Vector2(Constants.worldSizeX-1, 0);
            Body rightEdge = physicsWorld.CreateBody(bodydef); rightEdge.CreateShape(box);
            bodydef.Position = new Vector2(0, Constants.worldSizeY);
            box.SetAsBox(Constants.worldSizeX, 1f);
            Body topEdge = physicsWorld.CreateBody(bodydef); topEdge.CreateShape(box);
        }
        public GameState update() 
        {
            playerChar.KeyboardInput();
            playerChar.Move();
            playerChar.update();
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
