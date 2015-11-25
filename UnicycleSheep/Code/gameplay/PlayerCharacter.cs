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

        private float maxJump = 84f;
        private float Jumptime = Program.inGameFrameCount;
        public float JumptimePassed;
        bool jump = false;
        float jumpStrength = 0.0f;

        public float jumpLoadPercentage { get { return jumpStrength / maxJump; } }

        //state vars
        bool isOnGround;

        public bool isDead { get; private set; }

        public uint controllerIndex { get; private set; }

        public int playerIndex { get; private set; }
        public static int playerCount = 0;

        public PlayerCharacter(World _world, Vector2 _position, uint controllerIndex)
            :base(_world, _position)
        {
            playerIndex = playerCount++;

            isDead = false;

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
            circleDef.Radius = 0.75f;
            circleDef.LocalPosition.Set(0, 3);
            head = chest.CreateShape(circleDef);

            rotationHead = _position;

            PolygonDef Boxdef = new PolygonDef();
            Boxdef.SetAsBox(1, 1.5f);
            Boxdef.Density = 0.25f;
            Boxdef.Friction = 0.4f;
            Box2DX.Collision.Shape s2 = chest.CreateShape(Boxdef);
            chest.SetMassFromShapes();
            chest.SetUserData(this);

            //Jointshit
            RevoluteJointDef jointDefKW = new RevoluteJointDef();
            jointDefKW.Body2 = chest;
            jointDefKW.Body1 = body;
            jointDefKW.CollideConnected = false;
            jointDefKW.LocalAnchor2 = new Vector2(-0.0f, -3.8f);
            jointDefKW.LocalAnchor1 = new Vector2(0, 0);
            jointDefKW.EnableLimit = false;

            _world.CreateJoint(jointDefKW);

            Texture wheelTexture = AssetManager.getTexture(AssetManager.TextureName.ShoopWheel);
            wheelSprite = new AnimatedSprite(wheelTexture, 1.0f, 1, (Vector2)wheelTexture.Size);
            wheelSprite.Scale = new Vector2(0.2f, 0.2f); //(Vector2.One / (Vector2)wheelTexture.Size * 2F * circleDef.Radius).toScreenCoord() - Vector2.Zero.toScreenCoord();//new Vector2(0.08f, 0.08f);
            wheelSprite.Origin = ((Vector2)wheelSprite.spriteSize) / 2F;

            sheepSprite = new Sprite(AssetManager.getTexture(playerIndex == 0 ? AssetManager.TextureName.ShoopRed : AssetManager.TextureName.ShoopGreen));
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
                jumpButtonIsPressed = false;
                if (playerIndex == 0)
                {
                    if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                        rotation += 1F;
                    if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                        rotation += -1F;

                    if (KeyboardInputManager.isPressed(Keyboard.Key.F))
                        wantsToBalance = 1;

                    if (KeyboardInputManager.isPressed(Keyboard.Key.H))
                        wantsToBalance = -1;


                    jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.Space);
                }
                if (playerIndex == 1)
                {
                    if (KeyboardInputManager.isPressed(Keyboard.Key.Left))
                        rotation += 1F;
                    if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
                        rotation += -1F;

                    if (KeyboardInputManager.isPressed(Keyboard.Key.Numpad4))
                        wantsToBalance = 1;

                    if (KeyboardInputManager.isPressed(Keyboard.Key.Numpad6))
                        wantsToBalance = -1;


                    jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.Numpad0);
                }

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
            if (isDead) return;

            if (!isOnGround)
            {
                JumptimePassed = Program.inGameFrameCount - Jumptime;
            }

            if((this.angVelocity < 100) && this.rotation == 1)
                this.angVelocity += RotationFakt;
            else if ((this.angVelocity > -100) && this.rotation == -1)
                this.angVelocity -= RotationFakt;

            if (jump && /*isOnGround &&*/ jumpStrength > 0f)
            {
                Vector2 jumpDir = chest.GetWorldCenter() - body.GetWorldCenter();
                jumpDir.normalize(); jumpDir *= jumpStrength;
                body.ApplyImpulse(jumpDir, body.GetWorldCenter());
                chest.ApplyImpulse(jumpDir * 0.01f, chest.GetWorldCenter());
                jump = false;
                jumpStrength = 0f;
            }
            if (JumptimePassed < 40 /* && wantsToBalance == 0 */)
            {
                Console.WriteLine(JumptimePassed);
                float scalfact = 0;
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                theAngVec.normalize();
                scalfact = (float)Math.Acos(Math.Abs((double)theAngVec.X));
                //if(float.IsNaN(scalfact))
                //{
                //    Console.WriteLine(theAngVec.X);
                //}
                float angVel = chest.GetAngularVelocity();

                if (theAngVec.X > 0 && !float.IsNaN(scalfact))
                {
                    if (angVel < -2.5f)
                    {
                        chest.ApplyTorque(scalfact * 55 * Math.Abs(angVel * 12));
                    }
                    else
                        chest.ApplyTorque(scalfact * 55);

                }
                if (theAngVec.X < 0 && !float.IsNaN(scalfact))
                {
                    if (angVel > 2.5f)
                    {
                        chest.ApplyTorque(scalfact * -55 * Math.Abs(angVel * 12));
                    }
                    else
                        chest.ApplyTorque(scalfact * -55);
                }
            }
            if (wantsToBalance != 0)
            {
                //Vector2 targetVel = (Vector2)head.GetWorldCenter() - location;//optPos - headPos;
                //targetVel = wantsToBalance == 1 ? new Vector2(targetVel.X, -targetVel.Y) : new Vector2(-targetVel.X, targetVel.Y);
                //targetVel.normalize();
                Vector2 theAngVec = chest.GetPosition() - body.GetPosition();
                //Vector2 targetVec = wantsToBalance == 1 ? new Vector2 ((float) -(Math.Sin((double) head.GetAngle())),(float) Math.Cos((double) head.GetAngle())) : new Vector2((float)Math.Sin((double)head.GetAngle()), (float)-Math.Cos((double)head.GetAngle()));
                Vector2 targetVec = wantsToBalance > 0 ? new Vector2(-theAngVec.Y, theAngVec.X) : new Vector2(theAngVec.Y, -theAngVec.X);
                
                    targetVec.normalize();
                    float scalfact = (float) Math.Acos(Math.Abs((double)targetVec.X));

                //head.ApplyForce(targetVec * Counterfactf * scalfact, head.GetWorldCenter());
                
                chest.ApplyTorque(JumptimePassed < 30 ? 80 * scalfact * wantsToBalance : 60 * wantsToBalance);

                wantsToBalance = 0;
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
            if (isDead)
                sheepSprite.Color = new SFML.Graphics.Color(255, 255, 255, 100);

            wheelSprite.Position = location.toScreenCoord();
            wheelSprite.Rotation = -body.GetAngle() * Helper.RadianToDegree;

            win.Draw(wheelSprite);
            //draw after to overlap
            win.Draw(sheepSprite);

       //     DebugDraw(win);
        }

        private void DebugDraw(RenderWindow win)
        {
            SFML.Graphics.CircleShape wheel_Debug = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - Vector2.One.toScreenCoord().X);
            wheel_Debug.Origin = Vector2.One * wheel_Debug.Radius;
            wheel_Debug.Position = ((Vector2)body.GetPosition()).toScreenCoord();
            wheel_Debug.FillColor = isDead ? SFML.Graphics.Color.Black : SFML.Graphics.Color.Red;
            win.Draw(wheel_Debug);

            //debugDraw for sheepBody
            SFML.Graphics.RectangleShape body_Debug = new SFML.Graphics.RectangleShape(new Vector2(2,3).toScreenCoord() - Vector2.Zero.toScreenCoord());
            body_Debug.Origin = (Vector2)body_Debug.Size / 2F;
            body_Debug.Position = ((Vector2)chest.GetPosition()).toScreenCoord();
            body_Debug.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            body_Debug.FillColor = isDead ? SFML.Graphics.Color.Black : SFML.Graphics.Color.Red;
            win.Draw(body_Debug);

            //the head
            SFML.Graphics.CircleShape headDeb = new SFML.Graphics.CircleShape(Vector2.Zero.toScreenCoord().X - (0.75f * Vector2.One).toScreenCoord().X);
            headDeb.Origin = Vector2.One * headDeb.Radius;
            headDeb.Position = ((Vector2)chest.GetPosition() + (new Vector2(0, 3)).rotate(chest.GetAngle())).toScreenCoord();
            headDeb.Rotation = -chest.GetAngle() * Helper.RadianToDegree;
            headDeb.FillColor = isDead ? SFML.Graphics.Color.Black : SFML.Graphics.Color.Red;

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
                Jumptime = Program.inGameFrameCount;
                JumptimePassed = 0;
            }
            else if(head == _self && Physics.ContactManager.g_contactManager.isLethal(_other))
            {
                isDead = true;
            }
        }

        public void OnContactRemove(Box2DX.Collision.Shape _other, Box2DX.Collision.Shape _self, ContactPoint _point)
        {
      //      if (wheel == _self)
            {
                //only when the tile is left which was just hit
                if (_lastContact == _other)
                {
                    isOnGround = false;
                }
            }
        }

    }
}
