﻿using SFML.Graphics;
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

        public InGameState()
        {
            AABB aabb = new AABB();
            physicsWorld = new World(aabb, new Vec2(0.0f, 9.81f), false);

            playerChar = new PlayerCharacter(physicsWorld, new Vec2(30, 40));
        }
        public GameState update() 
        {
 
            return GameState.InGame; 
        }
        public void draw(RenderWindow win, View view) 
        {
            playerChar.draw(win, view);
        }
        public void drawGUI(GUI gui) { }
    }
}
