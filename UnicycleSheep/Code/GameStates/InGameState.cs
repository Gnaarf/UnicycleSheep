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
        public readonly Vector2[] startPostitions = new Vector2[] { new Vec2(5.0f, 50.0f), new Vec2(75.0f, 50.0f), new Vec2(40.0f, 50.0f), new Vec2(60.0f, 50.0f) };
        
        List<DekoElements.RemoteControllHand> dekoHands;
        List<Sprite> flags;

        Sprite BackgroundBackSprite;
        Sprite BackgroundFrontSprite;

        int resetFrameCounter;
        const int resetFrameCount = 400;

        const int numPlayers = 2;

        Actors.PolygonActor groundPolygonAct;

        public InGameState()
        {
            AABB aabb = new AABB();
            aabb.LowerBound.Set(0.0f, 0.0f);
            aabb.UpperBound.Set(Constants.worldSizeX, Constants.worldSizeX * Constants.screenRatio);

            physicsWorld = new World(aabb, new Vec2(0.0f, -9.81f), false);

            contactManager = Physics.ContactManager.g_contactManager;
            physicsWorld.SetContactListener(contactManager);
            
             // Set new Players and appending dekoHands
            ResetPlayers();
            setDekoFlags();
            //0xF0A58A4
            groundPolygonAct = new Actors.PolygonActor(physicsWorld, new Vec2(0.0f, 15.0f), 0xFBA58A4, Actors.FunctionType.GradientNoise, 4);

            BackgroundBackSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.InGameBackGroundBack));
            BackgroundBackSprite.Scale = 0.5F * Vector2.One;
            BackgroundFrontSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.InGameBackGroundFront));
            BackgroundFrontSprite.Scale = 0.5F * Vector2.One;

            //left and right borders of the map
            BodyDef bodydef = new BodyDef();
            bodydef.Position = new Vector2(0,0);
            bodydef.Angle = 0.0f;
            PolygonDef box = new PolygonDef();
            box.SetAsBox(1f, Constants.worldSizeY);

            Body leftEdge = physicsWorld.CreateBody(bodydef);
            contactManager.addNonLethalShape(leftEdge.CreateShape(box));

            bodydef.Position = new Vector2(Constants.worldSizeX-1, 0);
            Body rightEdge = physicsWorld.CreateBody(bodydef); 
            contactManager.addNonLethalShape(rightEdge.CreateShape(box));

            bodydef.Position = new Vector2(0, Constants.worldSizeY);
            box.SetAsBox(Constants.worldSizeX, 1f);
            Body topEdge = physicsWorld.CreateBody(bodydef); 
            contactManager.addNonLethalShape(topEdge.CreateShape(box));
        }

        private void setDekoFlags()
        {
            flags = new List<Sprite>();
            float count = 4F;
            Vector2 start = new Vector2(130, 320);
            Vector2 end = new Vector2(Constants.windowSizeX - 130, 320);
            bool redGreenToggle = true;
            for (float t = 0; t <= 1F; t += 1F / (count - 1))
            {
                Sprite flagSprite = new Sprite(AssetManager.getTexture(redGreenToggle ? AssetManager.TextureName.FlagRed : AssetManager.TextureName.FlagGreen));
                redGreenToggle = !redGreenToggle;
                flagSprite.Origin = (Vector2)flagSprite.Texture.Size * 0.5F;
                flagSprite.Scale = new Vector2((redGreenToggle ? 1 : -1) * 60, 90) / (Vector2)flagSprite.Texture.Size;
                flagSprite.Position = Vector2.lerp(start, end, t);
                flags.Add(flagSprite);
            }
        }

        private void ResetPlayers()
        {
            PlayerCharacter.playerCount = 0;

            resetFrameCounter = 0;

            if (playerChars == null) { playerChars = new List<PlayerCharacter>(); }
            else { playerChars.Clear(); }

            if (dekoHands == null) { dekoHands = new List<DekoElements.RemoteControllHand>(); }
            else { dekoHands.Clear(); }

            // find a controller for the Players
            foreach (uint i in GamePadInputManager.connectedPadIndices)
            {
                PlayerCharacter player = new PlayerCharacter(physicsWorld, startPostitions[playerChars.Count], i);
                dekoHands.Add(new DekoElements.RemoteControllHand(player));
                playerChars.Add(player);
                
                if (playerChars.Count == numPlayers)
                    break;
            }

            // if there are more Players than GamePads, give them invalid indices, so they will use Keyboard-Input
            // Cord: nullable would be better code, I guess...
            while (playerChars.Count < numPlayers)
            {
                PlayerCharacter player = new PlayerCharacter(physicsWorld, startPostitions[playerChars.Count], uint.MaxValue);
                dekoHands.Add(new DekoElements.RemoteControllHand(player));
                playerChars.Add(player);
            }
        }
        public GameState update() 
        {
            int numDeadPlayers = 0;

            foreach (PlayerCharacter playerChar in playerChars)
            {
                if (playerChar.isDead)
                    numDeadPlayers++;

                playerChar.KeyboardInput();
                playerChar.Move();
                playerChar.update();
            }
            physicsWorld.Step(1 / 60.0f, 8, 1);

            if(numDeadPlayers >= playerChars.Count - 1)
            {
                int winnerIndex = -1;
                foreach (PlayerCharacter playerChar in playerChars)
                {
                    if (!playerChar.isDead)
                        winnerIndex = playerChar.playerIndex;
                }

                // Player 0 red, Player 1 green
                if(winnerIndex == 0)
                    winnerSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.FlagRed));
                else if(winnerIndex == 1)
                    winnerSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.FlagGreen));

                winnerSprite.Origin = ((Vector2)winnerSprite.Texture.Size) * 0.5F;
                winnerSprite.Position = new Vector2(Constants.windowSizeX * 0.5F, Constants.windowSizeY * 0.5F);


                resetFrameCounter++;
                if (resetFrameCounter >= resetFrameCount)
                    return GameState.Reset;
            }

            return GameState.InGame; 
        }

        Sprite winnerSprite;

        public void draw(RenderWindow win, View view)
        {
            win.Draw(BackgroundBackSprite);

            // Draw Flag
            foreach (Sprite flag in flags)
            {
                win.Draw(flag);
            }

            if (resetFrameCounter > 0)
                win.Draw(winnerSprite);

            //Draw Players
            foreach (PlayerCharacter playerChar in playerChars)
            {
                playerChar.draw(win, view);
            }
            // Draw Ground
            groundPolygonAct.draw(win, view);

            win.Draw(BackgroundFrontSprite);

            // Draw deko Hands
            foreach (DekoElements.RemoteControllHand hand in dekoHands)
            {
                hand.draw(win);
            }
        }

        public void drawGUI(GUI gui) { }
    }
}
