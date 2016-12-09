using SFML.Graphics;
using SFML.Window;
using System;

namespace UnicycleSheep
{
    class Program
    {
        public static GameTime gameTime;
        public static int inGameFrameCount {get; private set; }

        static bool running = true;

        static GameState currentGameState = GameState.MainMenu;
        static GameState prevGameState = GameState.InGame;//changed
        static IGameState state;

        static RenderWindow win;
        static View view;
        static GUI gui;

        static void Main(string[] args)
        {
            // initialize window and view
            win = new RenderWindow(new VideoMode(Constants.windowSizeX, Constants.windowSizeY), "Shøøp");
            view = new View();
            resetView();
            gui = new GUI(win, view);

            // exit Program, when window is being closed
            //win.Closed += new EventHandler(closeWindow);
            win.Closed += (sender, e) => { (sender as Window).Close(); };

            // initialize GameState
            handleNewGameState();

            // initialize GameTime
            gameTime = new GameTime();
			float deltaTime = 0f;
            gameTime.Start();
            while (running && win.IsOpen())
            {
		//		gameTime.Update();

                GamePadInputManager.update();
                KeyboardInputManager.update();

                if (currentGameState == GameState.InGame) { inGameFrameCount++; }
                currentGameState = state.update(deltaTime);

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

				System.Threading.Thread.Sleep(5);
				// update GameTime
				gameTime.Update();
				deltaTime = (float)gameTime.EllapsedTime.TotalSeconds;
			//	int waitTime = (int)(16.667f - gameTime.EllapsedTime.TotalMilliseconds);
			//	if (waitTime > 0) System.Threading.Thread.Sleep(waitTime);
			//	win.SetTitle(waitTime.ToString());
			//	win.SetTitle((count/60f).ToString("0.0") + " | " + gameTime.TotalTime.TotalSeconds.ToString("0.0"));
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
                    win.SetTitle("Shøøp" + " score{" + InGameState.WinCount0 + " : " + InGameState.WinCount1 + ")");
                    break;

                case GameState.Reset:
                    currentGameState = prevGameState;
                    prevGameState = GameState.Reset;
                    handleNewGameState();
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
