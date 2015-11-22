using System;
using SFML.Graphics;
using SFML.Window;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using System.Collections.Generic;
using Physics;

namespace UnicycleSheep
{
    class PlayerCharacter : PhysicsActor, IContactEvent
    {
        //remove after manager exists
        AnimatedSprite wheelSprite;
        Sprite sheepSprite;

        Box2DX.Collision.Shape head;
        Box2DX.Collision.Shape wheel;
        Body chest;
        Vector2 rotationHead;

        //movement input and constant
        private float wheelToSheepRot = 0.0f; //basicly the rotation of the unicycle's frame
        protected float rotation = 0;
        private float wantsToBalance = 0;
        private float RotationFakt = 1f;
        //verstärkung der gegensteuerung
        public float Counterfactf = 10f;

        private float maxJump = 42f;

        bool jump = false;
        float jumpStrength = 0.0f;

        //state vars
        bool isOnGround;

        bool bIsDead = false;
        public bool isDead { get { return bIsDead; } private set { bIsDead = value; } }

        uint controllerIndex;

        public PlayerCharacter(World _world, Vector2 _position, uint controllerIndex)
            :base(_world, _position)
        {
            this.controllerIndex = controllerIndex;

            this.angVelocity = 0;
            //build the unicycle
            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 1;
            circleDef.Density = 1f;
            circleDef.Friction = 1.0f;
            circleDef.Restitution = 0.0f;
            circleDef.LocalPosition.Set(0, 0);

            wheel = body.CreateShape(circleDef);
            body.SetMassFromShapes();
            body.SetUserData(this); // link body and this to register collisions in this

            //build the head and connect with the wheel
            BodyDef bodydef = new BodyDef();
            bodydef.Position = _position + new Vector2(0.0f, 3.5f);
            bodydef.Angle = 0f;

            chest = _world.CreateBody(bodydef);

            //add the head
            circleDef.Density = 0.0001f;
            circleDef.Radius = 0.5f;
            circleDef.LocalPosition.Set(0, 3);
            head = chest.CreateShape(circleDef);

            rotationHead = _position;

            PolygonDef Boxdef = new PolygonDef();
            Boxdef.SetAsBox(1, 1.5f);
            Boxdef.Density = 0.25f;
            Boxdef.Friction = 0.0f;
            Box2DX.Collision.Shape s2 = chest.CreateShape(Boxdef);
            chest.SetMassFromShapes();
            chest.SetUserData(this);

            //Jointshit
            RevoluteJointDef jointDefKW = new RevoluteJointDef();
            jointDefKW.Body2 = chest;
            jointDefKW.Body1 = body;
            jointDefKW.CollideConnected = false;
            jointDefKW.LocalAnchor2 = new Vector2(0, -3.5f);
            jointDefKW.LocalAnchor1 = new Vector2(0, 0);
            jointDefKW.EnableLimit = false;

            _world.CreateJoint(jointDefKW);

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = (Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord() - Vector2.Zero.toScreenCoord();
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(AssetManager.TextureName.Shoop));
            sheepSprite.Scale = new Vector2(0.2f, 0.2f);
            if(_position.X > Constants.worldSizeX / 2F)
                sheepSprite.Scale = new Vector2(-0.2f, 0.2f);
            sheepSprite.Origin = ((Vector2)sheepSprite.Texture.Size) / 2F;
        }

        public void KeyboardInput()
        {
            //discard any input when dead
            if (isDead) return;

            bool jumpButtonIsPressed;

            if(GamePadInputManager.isConnected(controllerIndex))
            {
                rotation = -GamePadInputManager.getLeftStick(controllerIndex).X;
                wantsToBalance = -GamePadInputManager.getRightStick(controllerIndex).X;

                jumpButtonIsPressed = GamePadInputManager.isPressed(GamePadButton.RB, controllerIndex);
            }
            else
            {
                rotation = 0F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                    rotation += 1F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                    rotation += -1F;

                if(KeyboardInputManager.isPressed(Keyboard.Key.Left))
                    wantsToBalance = 1;

                if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
                    wantsToBalance = -1;
                

                jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.Space);
            }

            if (jumpButtonIsPressed)
            {
                if (jumpStrength < maxJump)
                    jumpStrength += 0.8f;
                jump = false;
            }
            else if (jumpStrength > 0) jump = true;  

        }
        public void Move()
        {
            if((this.angVelocity < 100) && this.rotation == 1)
                this.angVelocity += RotationFakt;
            else if ((this.angVelocity > -100) && this.rotation == -1)
                this.angVelocity -= RotationFakt;

            if (jump && /*isOnGround &&*/ jumpStrength > 0f)
            {
                body.ApplyImpulse(new Vector2(0, jumpStrength), body.GetWorldCenter());
                chest.ApplyImpulse(new Vector2(0, jumpStrength * 0.01f), body.GetWorldCenter());
                jump = false;
                jumpStrength = 0f;
            }

            if(wantsToBalance != 0)
            {
                //Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                //targetVel = wantsToBalance == 1 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);
                //targetVel.normalize();
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                //Vector2 targetVec = wantsToBalance == 1 ? new Vector2 ((float) -(Math.Sin((double) head.GetAngle())),(float) Math.Cos((double) head.GetAngle())) : new Vector2((float)Math.Sin((double)head.GetAngle()), (float)-Math.Cos((double)head.GetAngle()));
                Vector2 targetVec = wantsToBalance > 0 ? new Vector2(-theAngVec.Y, theAngVec.X) : new Vector2(theAngVec.Y, -theAngVec.X);
                
                    targetVec.normalize();
                    float scalfact = (float) Math.Acos(Math.Abs((double)targetVec.X));
                Console.WriteLine(scalfact);

                //head.ApplyForce(targetVec * Counterfactf * scalfact, head.GetWorldCenter());
                
                chest.ApplyTorque(80* scalfact * wantsToBalance);

                wantsToBalance = 0;
            }
            if(isOnGround)
            {
                float scalfact = 0;
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                theAngVec.normalize();
                scalfact = (float)Math.Acos(Math.Abs((double)theAngVec.X));
                if(float.IsNaN(scalfact))
                {
                    Console.WriteLine(theAngVec.X);
                }
                float angVel = chest.GetAngularVelocity();
                Console.WriteLine(scalfact);

                if (theAngVec.X > 0 && !float.IsNaN(scalfact))
                {
                   chest.ApplyTorque(scalfact * 50);
                }
                if (theAngVec.X < 0 && !float.IsNaN(scalfact))
                {
                   chest.ApplyTorque(scalfact * -50);
                }
                ////some auto correction to make it easier to not fall over
                //Vector2 headPos = head.GetWorldCenter();
                //Vector2 optPos = location + new Vector2(0.0f, 4.0f);
                //Vector2 currentVel = head.GetLinearVelocity();

                //Vector2 targetDir = optPos - headPos;
                //Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                //targetVel = targetDir.Y < 0 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);

                //float len = targetDir.lengthSqr;
                //if (len < 0.05f * 0.05f) return;

                //targetVel.normalize();

                //// Cord: this caused a DevideByZero and the variable seems to be unused, so I commented it
                ////float dif = (currentVel.normalize() - targetVel).length;

                //head.ApplyImpulse(targetVel, head.GetWorldCenter());//(Vector2)(body.GetLinearVelocity()*/
            }
        }


        public override void update()
        {
            base.update();

            Vector2 radius = (Vector2)chest.GetWorldCenter() - location;
            wheelToSheepRot = (float)System.Math.Atan2(radius.X, radius.Y) * Helper.RadianToDegree;
        }


        public override void draw(RenderWindow win, View view)
        {
            Vector2 sheepLoc = chest.GetWorldCenter();

            sheepSprite.Position = sheepLoc.toScreenCoord();
            sheepSprite.Rotation = wheelToSheepRot;

            wheelSprite.Position = location.toScreenCoord();
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(wheelSprite);
            //draw after to overlap
            win.Draw(sheepSprite);

            DebugDraw(win);
        }

        private void DebugDraw(RenderWindow win)
        {
            SFML.Graphics.CircleShape wheel_Debug = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - Vector2.One.toScreenCoord().X);
            wheel_Debug.Origin = Vector2.One * wheel_Debug.Radius;
            wheel_Debug.Position = ((Vector2)body.GetPosition()).toScreenCoord();
            wheel_Debug.FillColor = SFML.Graphics.Color.Red;
            win.Draw(wheel_Debug);

            //debugDraw for sheepBody
            SFML.Graphics.RectangleShape body_Debug = new SFML.Graphics.RectangleShape(new Vector2(2,3).toScreenCoord() - Vector2.Zero.toScreenCoord());
            body_Debug.Origin = (Vector2)body_Debug.Size / 2F;
            body_Debug.Position = ((Vector2)chest.GetPosition()).toScreenCoord();
            body_Debug.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            body_Debug.FillColor = SFML.Graphics.Color.Red;
            win.Draw(body_Debug);

            //the head
            SFML.Graphics.CircleShape headDeb = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - Vector2.One.toScreenCoord().X);
            headDeb.Origin = Vector2.One * headDeb.Radius;
            headDeb.Position = ((Vector2)chest.GetPosition() + new Vector2(0, 2)).toScreenCoord();
            headDeb.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            headDeb.FillColor = SFML.Graphics.Color.Red;

            win.Draw(headDeb);    
        }

        // ********************************************************** //

        // ********************************************************** //

        Box2DX.Collision.Shape _lastContact;

        public void OnContact(Box2DX.Collision.Shape _other, Box2DX.Collision.Shape _self, ContactPoint _point)
        {
            if (wheel == _self)
            {
                _lastContact = _other;
                isOnGround = true;
            }
            else if(head == _self)
            {
                isDead = true;
            }
        }

        public void OnContactRemove(Box2DX.Collision.Shape _other, Box2DX.Collision.Shape _self, ContactPoint _point)
        {
            if (wheel == _self)
            {
                //only when the tile is left which was just hit
                if (_lastContact == _other)
                    isOnGround = false;
            }
        }

    }
}
