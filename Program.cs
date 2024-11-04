using System;
using static Flappy_Bird.Program;

namespace Flappy_Bird
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setting the window width and height
            Console.WindowWidth = 80;
            Console.WindowHeight = 30;
            // Setting the buffer width and height to the same value.
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            // Hiding the console cursor
            Console.CursorVisible = false;

            Game myGame = new Game();
            myGame.RunGame();

            Console.ReadKey(true);
        }
        public class Pipe
        {
            public int PositionX { get; set; }
            public int Height { get; set; }

            public Pipe(int positionX, int height)
            {
                PositionX = positionX;
                Height = height;
            }
        }

        public class Game
        {
            bool GameOver = false;
            int LastFrameSpacePressed = 0;
            int FrameCount = 0;
            int FlappyPositionY;
            int FlappyPositionX = 8;
            int Score = 0;
            int PipePositionX;
            int PipeHeight;
            int PipeGap = 5;

            Random rand = new Random();
            List<Pipe> pipes = new List<Pipe>();

            public void RunGame()
            {
                Console.WriteLine("Game Running");
                FlappyPositionY = Console.WindowHeight / 5;
                PipePositionX = Console.WindowWidth - 1; // Start position for pipes
                PipeHeight = rand.Next(5, Console.WindowHeight - 5); // Random height for the gap (initialized once)

                // Initialize multiple pipes
                for (int i = 0; i < 4; i++) // You can adjust the number of pipes here
                {
                    int initialPositionX = Console.WindowWidth + (i * 20); // Space them out
                    int pipeHeight = rand.Next(5, Console.WindowHeight - 5);
                    pipes.Add(new Pipe(initialPositionX, pipeHeight));
                }

                while (!GameOver)
                {
                    Console.Clear(); // Clear only once per frame

                    HandleInput();
                    UpdateBirdPosition();

                    // Draw the bird
                    Console.SetCursorPosition(FlappyPositionX, FlappyPositionY);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("o");
                    Console.ForegroundColor = ConsoleColor.White;

                    // Draw the pipes
                    DrawPipe();

                    // Check for game over conditions
                    if (FlappyPositionY <= 0 || FlappyPositionY >= Console.WindowHeight - 1 || CheckCollision())
                    {
                        GameOver = true;
                    }

                    // Wait 100 milliseconds
                    System.Threading.Thread.Sleep(100);
                    FrameCount++;
                }

                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Game Over");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("You got " + Score + "!");
            }

            private void HandleInput()
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
                    if (KeyInfo.Key == ConsoleKey.Spacebar)
                    {
                        LastFrameSpacePressed = FrameCount;
                    }
                }
            }

            private void UpdateBirdPosition()
            {
                if (FrameCount - LastFrameSpacePressed < 2)
                {
                    FlappyPositionY--; // Move up
                }
                else
                {
                    FlappyPositionY++; // Move down
                }
            }

            private void DrawPipe()
            {
                Console.BackgroundColor = ConsoleColor.Green;

                foreach (var pipe in pipes)
                {
                    // Draw top pipe
                    for (int i = 0; i < pipe.Height; i++)
                    {
                        if (0 < pipe.PositionX && pipe.PositionX < 80)
                        {
                        Console.SetCursorPosition(pipe.PositionX, i);
                        Console.Write(" ");
                        }
                    }

                    // Draw bottom pipe
                    for (int i = pipe.Height + PipeGap; i < Console.WindowHeight; i++)
                    {
                        if (0 < pipe.PositionX && pipe.PositionX < 80)
                        {
                        Console.SetCursorPosition(pipe.PositionX, i);
                        Console.Write(" ");
                        }
                    }

                    pipe.PositionX--; // Move the pipe to the left
                }

                // Reset pipes that go off screen
                for (int i = pipes.Count - 1; i >= 0; i--)
                {
                    if (pipes[i].PositionX < 0)
                    {
                        pipes[i].PositionX = Console.WindowWidth - 1;
                        pipes[i].Height = rand.Next(5, Console.WindowHeight - 5); // Generate new height
                        Score++; // Increase score for passing a pipe
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black; // Reset background color
            }

            private bool CheckCollision()
            {
                // Check collision with each pipe
                foreach (var pipe in pipes)
                {
                    // Check if the bird is aligned with the pipe
                    if (pipe.PositionX == FlappyPositionX)
                    {
                        // Check if the bird is outside the gap
                        return FlappyPositionY < pipe.Height || FlappyPositionY > pipe.Height + PipeGap;
                    }
                }

                return false;
            }
        }
    }
}

