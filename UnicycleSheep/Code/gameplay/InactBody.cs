using System;
using SFML.Graphics;
using SFML.Window;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;

namespace UnicycleSheep
{
    class InactBody : PhysicsActor
    {
        float _restitution;
        float _friction;
        Texture _texture;
        Sprite _sprite;
        Vector2 _position;

        public InactBody(World _world, Vector2 _position, string _texture, Vector2 _scale, float _restitution, float _friction) : base(_world, _position)
        {
            this._friction = _friction;
            this._restitution = _friction;
            this._position = _position;
            this._texture = new Texture(_texture);
            this._sprite = new Sprite(this._texture);
            this._sprite.Scale = new Vector2f(10, 10);
        }



        public override void draw(RenderWindow win, View view)
        {
            this._sprite.Position = location.toScreenCoord();

            win.Draw(this._sprite);
        }
    }
}