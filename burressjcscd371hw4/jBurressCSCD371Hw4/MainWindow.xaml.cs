/* Name: John Burress
 * Description: This is a single play pong game. The objective is to stop the ball from hitting the left wall. The ball speeds up by .5 after every 
 * contact with the paddle. There is a high score filing system that stores your score and will tell you what the highest score is or if no high score has 
 * been made. There is a start, pause, and exit option inside the File pull down menu. There is also a help pull down menu with an about button that
 * will tell you the controls and features of the game. There is a restart button that when clicked closes the current window and opens a new one. 
 * Restarting does not save your score. If the ball hits the left wall, the paddle and ball stop moving and a game over message along with your current score
 * are displayed in the middle of the window. 
 * POSSIBLE EXTRA CREDIT ATTEMPTS:
 * -After the ball makes contact with the paddle, it speeds up by .5. The speed is displayed in the bottom right corner. 
 * -The paddle also speeds up along with the ball. 
 * -All scores are saved in a text file: "HighScore.txt" in which the highest number is pulled from and the high score is displayed in the bottom
 * left corner. 
*/

//Used the provided code from AnimateBall as a base
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace jBurressCSCD371Hw4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private double dx = 1;
        private double dy = 1;
        private double vertDirection = -1;
        private double horizDirection = 1;

        private double gameBallTop = 0;
        private double gameBallLeft = 0;

        private double gamePaddleTop = 0;
        private double gamePaddleLeft = 0;
        private double gamePaddleDy = 1;
        private int counter = 0;
        private double turn = 1;

        public MainWindow()
        {
            InitializeComponent();
            readHighScore();
            hitCounter();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            gameTimer.Tick += GameTimer_Tick;

            gameBallTop = Canvas.GetTop(gameBall);
            gameBallLeft = Canvas.GetLeft(gameBall);

            gamePaddleTop = Canvas.GetTop(gamePaddle);
            gamePaddleLeft = Canvas.GetLeft(gamePaddle);

            gameTimer.IsEnabled = true;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            //double xComponent = gameBall;
            //double yComponent = 0;
            if (gameBallTop <= 0 || gameBallTop >= (myGameCanvas.Height - gameBall.Height))
            {
                vertDirection *= -1;

            }
            if (gameBallLeft <= 0 || gameBallLeft >= myGameCanvas.Width - gameBall.Width)
            {
                horizDirection *= -1;

            }
            if (gamePaddleLeft + gamePaddle.Width == gameBallLeft
                && gameBallTop + gameBall.Height / 2 >= gamePaddleTop && gameBallTop + gameBall.Height / 2 <= gamePaddleTop + gamePaddle.Height)
            {//this is when the ball hits the paddle
                horizDirection = 1;
                hitCounter();

            }

            if (gameBallLeft <= 1)//when ball hits left wall???
            {
                endGame();
            }

            gameBallLeft += dx * horizDirection;
            gameBallTop += dy * vertDirection;

            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (gamePaddleTop - gamePaddleDy >= 0)
                    gamePaddleTop -= gamePaddleDy;
            }
            else if (e.Key == Key.Down)
            {
                if (gamePaddleTop + gamePaddleDy + gamePaddle.Height <= myGameCanvas.Height)
                    gamePaddleTop += gamePaddleDy;
            }
            Canvas.SetTop(gamePaddle, gamePaddleTop);
        }

        private void hitCounter()
        {
            labelUserScore.Content = "User Score: " + counter;
            counter = counter + 1;

            horizDirection = turn;
            gamePaddleDy = gamePaddleDy + 1;

            labelSpeed.Content = "Speed: " + horizDirection;
            turn = turn + .5;

        }

        private void endGame()
        {

            vertDirection = 0;
            horizDirection = 0;
            gamePaddleDy = 0;
            labelGameOver.Content = "GAME OVER \n" + "You Scored: " + (counter - 1) + "!";
            catchHighScore();
        }

        private void catchHighScore()//HighScore.txt is stored in jBurressCSCD371Hw4\bin\Debug
        {
            TextWriter writeFile = new StreamWriter("HighScore.txt", true);
            writeFile.WriteLine(counter - 1);
            writeFile.Close();

        }

        private void readHighScore()
        {
            if (File.Exists(@"HighScore.txt"))
            {
                var Max = File.ReadAllLines(@"HighScore.txt").Select(int.Parse).Max();
                labelHighScore.Content = "Absolute High Score: " + Max;
            }
            else
                labelHighScore.Content = "Absolute High Score: NO HIGH SCORE MADE YET";

        }

        private void buttonRestartClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void mnuFileStart_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.IsEnabled = true;
        }

        private void mnuFileStop_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.IsEnabled = false;
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void mnuHelpAbout_Click(object sender, RoutedEventArgs e)//this is a mess
        {
            String intro = "Welcome to Single Player Pong! \n";
            String help = "Use the UP and DOWN arrow keys to move the paddle to stop the ball from hitting the left wall! \n";
            String help2 = "The longer you last, the faster the ball becomes! \n";
            String help3 = "Click File -> Start to start the game again after pausing \n";
            String help4 = "Click File -> Pause to pause the game \n";
            String help5 = "Click Restart to restart the game. And of course Exit to exit the game \n";
            mnuFileStop_Click(sender, e);
            MessageBox.Show(intro + "" + help + "" + help2 + "" + help3 + "" + help4 + "" + help5);
            mnuFileStart_Click(sender, e);
        }
    }//end of main
}//end of nameSpace
