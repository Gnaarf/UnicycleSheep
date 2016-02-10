using SFML.Window;

namespace UnicycleSheep
{
    class PlayerController : Controller
    {
        public uint controllerIndex { get; private set; }

        public PlayerController(PlayerCharacter _character, uint _controllerIndex) :
            base(_character)
        {
            controllerIndex = _controllerIndex;
        }

        protected override void process()
        {
            //input

            bool jumpButtonIsPressed;

            if (GamePadInputManager.isConnected(controllerIndex))
            {
                rotation = -GamePadInputManager.getLeftStick(controllerIndex).X;
                wantsToBalance = -GamePadInputManager.getRightStick(controllerIndex).X;

                jumpButtonIsPressed = GamePadInputManager.isPressed(GamePadButton.RB, controllerIndex);
            }
            else
            {
                // Fallback, if no Controller is connected
                jumpButtonIsPressed = ProcessKeyboardInput();

            }

            if (jumpButtonIsPressed)
            {
                isLoadingJump = true;
                jump = false;
            }
            else if (isLoadingJump)
            {
                jump = true;
                isLoadingJump = false;
            }
        }

        private bool ProcessKeyboardInput()
        {
            bool jumpButtonIsPressed;
            rotation = 0F;
            wantsToBalance = 0F;
            jumpButtonIsPressed = false;
            if (character.playerIndex == 0)
            {
                if (KeyboardInputManager.isPressed(Keyboard.Key.S))
                    rotation += 1F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.W))
                    rotation += -1F;

                if (KeyboardInputManager.isPressed(Keyboard.Key.A))
                    wantsToBalance = 1;
                if (KeyboardInputManager.isPressed(Keyboard.Key.D))
                    wantsToBalance = -1;


                jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.Space);
            }
            else if (character.playerIndex == 1)
            {
                if (KeyboardInputManager.isPressed(Keyboard.Key.Up))
                    rotation += 1F;
                if (KeyboardInputManager.isPressed(Keyboard.Key.Down))
                    rotation += -1F;

                if (KeyboardInputManager.isPressed(Keyboard.Key.Left))
                    wantsToBalance = 1;
                if (KeyboardInputManager.isPressed(Keyboard.Key.Right))
                    wantsToBalance = -1;


                jumpButtonIsPressed = KeyboardInputManager.isPressed(Keyboard.Key.RControl);
            }
            return jumpButtonIsPressed;
        }
    }
}
