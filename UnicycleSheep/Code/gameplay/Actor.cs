using Box2DX.Collision;
using Box2DX.Dynamics;
using SFML.Graphics;
using System;

namespace UnicycleSheep
{
    /*
     * The basic type of objects existing in the world.
     * 
     */
    class PhysicsActor
    {
        public PhysicsActor(World _world, Vector2 _position, float _angle = 0.0f, bool _isStatic = false)
        {
            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position;
            bodydef.Angle = 0.0f;

            body = _world.CreateBody(bodydef);
        }
        public Body body { get; protected set; }

        public Vector2 location { get { return body.GetPosition(); } protected set { throw new Exception("can't set Position of a PhysicObject"); } }

        public Vector2 velocity { get { return body.GetLinearVelocity(); } protected set { body.SetLinearVelocity(value); } }

        public float angVelocity { get { return body.GetAngularVelocity(); } protected set { body.SetAngularVelocity(value); } }

        public virtual void update(float _deltaTime)
        {
        }
		public virtual void update()
		{
		}

        public virtual void draw(RenderWindow win, View view) {}
    }
}
