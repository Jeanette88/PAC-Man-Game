using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace PAC_Man_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); // create a new instance of the dispatcher timer called game timer

        bool goLeft, goRight, goDown, goUp; // 4 boolean created to move player in 4 direction
        bool noLeft, noRight, noDown, noUp; // 4 more boolean created to stop player moving in that direction

        int speed = 8; // player speed

        Rect pacmanHitBox; // player hit box, this will be used to check for collision between player to walls, ghost and coints

        int ghostSpeed = 10; // ghost image speed
        int ghostMoveStep = 160; // ghost step limits
        int currentGhostStep; // current movement limit for the ghosts
        int score = 0; // score keeping integer

        public MainWindow()
        {
            InitializeComponent();

            GameSetUp();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)  
        {
            // Rules on how Pac man can move in the Game

            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false; 
                noRight = noUp = noDown = false;

                goLeft = true; 

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2); //Pac man face left
            }

            if (e.Key == Key.Right && noRight == false) 
            {
                noLeft = noUp = noDown = false; 
                goLeft = goUp = goDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); // Pac man face right
            }

            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false; 

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); // Pac man face up
            }

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false; 
                goUp = goLeft = goRight = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); // Pac man face down
            }
        }

        private void GameSetUp()
        {
            MyCanvas.Focus(); 

            gameTimer.Tick += GameLoop; // link the game loop event to the time tick
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // set time to tick every 20 milliseconds
            gameTimer.Start(); // start the time
            currentGhostStep = ghostMoveStep; // set current ghost step to the ghost move step


            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;

        }

        private void GameLoop(object sender, EventArgs e)
        {
            // this is the game loop event, this event will control all of the movements, outcome, collision and score for the game

            txtScore.Content = "Score: " + score; // show the score

            // Rules on how fast Pac man can go

            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }


            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height) // Rules on where Pac man can go within the Canvas
            {
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); // Updates where Pac man are on Canvasen            


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                // If Pac man touches the wall then there will be a stop
                if ((string)x.Tag == "wall")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }

                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }

                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }

                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }

                // Pac man takes the coins - the coins disappear and become points
                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }
                }
            }
        }

        private void GameOver(string message)
        {
            gameTimer.Stop(); // stop the game timer
            MessageBox.Show(message, "The Pac Man Game - GAME OVER!"); // show a message box with the message Game over.

            // when the player clicks ok on the message box - restart the application
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }




    }
}
