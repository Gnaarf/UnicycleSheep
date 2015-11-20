using SFML.Graphics;
using SFML.Window;
using System;

namespace UnicycleSheep
{
    class Program
    {
        public static GameTime gameTime;

        public static GamePadInputManager gamePadInputManager;

        static bool running = true;

        static GameState currentGameState = GameState.MainMenu;
        static GameState prevGameState = GameState.MainMenu;
        static IGameState state;

        static RenderWindow win;
        static View view;
        static GUI gui;

        public static string rootPath = "../../"; // path for development version
        //public static string rootPath = ""; // path for shipping version

        static void Main(string[] args)
        {
            // initialize window and view
            win = new RenderWindow(new VideoMode(800, 600), "2D Game Project");
            view = new View();
            resetView();
            gui = new GUI(win, view);

            // exit Program, when window is being closed
            //win.Closed += new EventHandler(closeWindow);
            win.Closed += (sender, e) => { (sender as Window).Close(); };

            // initialize GamePadInputManager, in case, there are GamePads connected
            gamePadInputManager = new GamePadInputManager();

            // initialize GameState
            handleNewGameState();

            // initialize GameTime
            gameTime = new GameTime();
            gameTime.Start();

            while (running && win.IsOpen())
            {
                gamePadInputManager.update();
                // TODO: reevaluate gamepads every once in a while

                currentGameState = state.update();

                if (currentGameState != prevGameState)
                {
                    handleNewGameState();
                }

                // draw current frame
                win.Clear(new Color(100, 149, 237));    //cornflowerblue ftw!!! 1337
                state.draw(win, view);
                state.drawGUI(gui);

                win.SetView(view);
                win.Display();

                // check for window-events. e.g. window closed        
                win.DispatchEvents();

                // update GameTime
                gameTime.Update();
                int waitTime = (int)(16.667f - gameTime.EllapsedTime.Milliseconds);
                System.Threading.Thread.Sleep(waitTime >= 0 ? waitTime : 0 );
            }
        }

        static void handleNewGameState()
        {
            switch (currentGameState)
            {
                case GameState.None:
                    running = false;
                    break;

                case GameState.MainMenu:
                    state = new MainMenuState();
                    break;

                case GameState.InGame:
                    state = new InGameState();
                    break;
            }

            prevGameState = currentGameState;

            resetView();
        }

        static void resetView()
        {
            view.Center = new Vector2(win.Size.X / 2F, win.Size.Y / 2F);
            view.Size = new Vector2(win.Size.X, win.Size.Y);
        }
    }
}
