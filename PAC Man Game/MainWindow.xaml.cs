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
