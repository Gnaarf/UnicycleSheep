using SFML.Graphics;
using Box2DX.Common;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;

namespace UnicycleSheep
{
    class InGameState : IGameState
    {
        //singletons
        World physicsWorld;
        Physics.ContactManager contactManager;

        //test
        List<PlayerCharacter> playerChars;
        Vector2[] startPostitions = new Vector2[] { new Vec2(5.0f, 50.0f), new Vec2(75.0f, 50.0f), new Vec2(40.0f, 50.0f), new Vec2(60.0f, 50.0f) };
        
        List<DekoElements.RemoteControllHand> dekoHands;

        Actors.PolygonActor groundPolygonAct;

        public InGameState()
        {
            AABB aabb = new AABB();
            aabb.LowerBound.Set(0.0f, 0.0f);
            aabb.UpperBound.Set(Constants.worldSizeX, Constants.worldSizeX * Constants.screenRatio);

            physicsWorld = new World(aabb, new Vec2(0.0f, -9.81f), false);

            contactManager = new Physics.ContactManager();
            physicsWorld.SetContactListener(contactManager);
            
             // Set new Players
            ResetPlayers(2);
            
            groundPolygonAct = new Actors.PolygonActor(physicsWorld, new Vec2(0.0f, 0.0f), 0xF0A58A4, Actors.FunctionType.GradientNoise, 4);

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

        private void ResetPlayers(int numPlayers)
        {
            if (playerChars == null) { playerChars = new List<PlayerCharacter>(); }
            else { playerChars.Clear(); }

            if (dekoHands == null) { dekoHands = new List<DekoElements.RemoteControllHand>(); }
            else { dekoHands.Clear(); }

            // find a controller for the Players
            foreach (uint i in GamePadInputManager.connectedPadIndices)
            {
                playerChars.Add(new PlayerCharacter(physicsWorld, startPostitions[playerChars.Count], i));
                dekoHands.Add(new DekoElements.RemoteControllHand(i));
                
                if (playerChars.Count == numPlayers)
                    break;
            }

            // if there are more Players than GamePads, give them invalid indices, so they will use Keyboard-Input
            // Cord: nullable would be better code, I guess...
            while (playerChars.Count < numPlayers)
            {
                playerChars.Add(new PlayerCharacter(physicsWorld, startPostitions[playerChars.Count], uint.MaxValue));
                dekoHands.Add(new DekoElements.RemoteControllHand(uint.MaxValue));
            }
        }
        public GameState update() 
        {
            foreach (PlayerCharacter playerChar in playerChars)
            {
                playerChar.KeyboardInput();
                playerChar.Move();
                playerChar.update();
            }
            physicsWorld.Step(1 / 60.0f, 8, 1);
            return GameState.InGame; 
        }
        public void draw(RenderWindow win, View view) 
        {
            foreach (PlayerCharacter playerChar in playerChars)
            {
                playerChar.draw(win, view);
            }
            groundPolygonAct.draw(win, view);

            foreach (DekoElements.RemoteControllHand hand in dekoHands)
            {
                hand.draw(win);
            }
        }

        public void drawGUI(GUI gui) { }
    }
}
