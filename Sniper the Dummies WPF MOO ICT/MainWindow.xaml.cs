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

namespace Sniper_the_Dummies_WPF_MOO_ICT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageBrush backgroundImage = new ImageBrush();
        ImageBrush ghostSprite = new ImageBrush();

        DispatcherTimer DummyMoveTimer = new DispatcherTimer();
        DispatcherTimer showGhostTimer = new DispatcherTimer();

        int topCount = 0;
        int bottomCount = 0;

        int score;
        int miss;


        List<int> topLocation;
        List<int> bottomLocation;

        List<Rectangle> removeThis = new List<Rectangle>();

        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();

            this.Cursor = Cursors.None;

            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));
            MyCanvas.Background = backgroundImage;

            scopeImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/sniper-aim.png"));
            scopeImage.IsHitTestVisible = false;

            ghostSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ghost.png"));

            DummyMoveTimer.Tick += DummyMoveTick;
            DummyMoveTimer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            DummyMoveTimer.Start();

            showGhostTimer.Tick += GhostAnimation;
            showGhostTimer.Interval = TimeSpan.FromMilliseconds(20);
            showGhostTimer.Start();

            topLocation = new List<int> { 270, 540, 23, 540, 270, 23 };
            bottomLocation = new List<int> {128, 678, 420, 678, 128, 420 };


        }

        private void GhostAnimation(object sender, EventArgs e)
        {

            scoreText.Content = "Score: " + score;

            missText.Content = "Missed: " + miss;

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "ghost")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 5);

                    if (Canvas.GetTop(x) < -180)
                    {
                        removeThis.Add(x);
                    }

                }
            }

            foreach (Rectangle y in removeThis)
            {
                MyCanvas.Children.Remove(y);
            }


        }

        private void DummyMoveTick(object sender, EventArgs e)
        {
            removeThis.Clear();

            foreach (var i in MyCanvas.Children.OfType<Rectangle>())
            {

                if ((string)i.Tag == "top" || (string)i.Tag == "bottom")
                {
                    removeThis.Add(i);

                    topCount -= 1;
                    bottomCount -= 1;

                    miss += 1;

                }


            }


            if (topCount < 3)
            {
                ShowDummies(topLocation[rand.Next(0, 5)], 35, rand.Next(1, 4), "top");
                topCount += 1;
            }

            if (bottomCount < 3)
            {
                ShowDummies(bottomLocation[rand.Next(0, 5)], 230, rand.Next(1, 4), "bottom");
                bottomCount += 1;
            }


        }

        private void ShootDummy(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {

                Rectangle activeRec = (Rectangle)e.OriginalSource;



                if ((string)activeRec.Tag == "top" || (string)activeRec.Tag == "bottom")
                {
                    MyCanvas.Children.Remove(activeRec);

                    score++;

                    Rectangle ghostRec = new Rectangle
                    {
                        Width = 60,
                        Height = 100,
                        Fill = ghostSprite,
                        Tag = "ghost"
                    };


                    Canvas.SetLeft(ghostRec, Mouse.GetPosition(MyCanvas).X - 40);
                    Canvas.SetTop(ghostRec, Mouse.GetPosition(MyCanvas).Y - 60);

                    MyCanvas.Children.Add(ghostRec);
                }


                if ((string)activeRec.Tag == "top")
                {
                    topCount -= 1;
                }
                if ((string)activeRec.Tag == "bottom")
                {
                    bottomCount -= 1;
                }



            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            Point position = e.GetPosition(this);

            double pX = position.X;
            double pY = position.Y;


            Canvas.SetLeft(scopeImage, pX - (scopeImage.Width / 2));
            Canvas.SetTop(scopeImage, pY - (scopeImage.Height / 2));

        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {
            ImageBrush dummyBackground = new ImageBrush();

            switch (skin)
            {
                case 1:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy01.png"));
                    break;
                case 2:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy02.png"));
                    break;
                case 3:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy03.png"));
                    break;
                case 4:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy04.png"));
                    break;
            }

            Rectangle newRec = new Rectangle
            {
                Tag = tag,
                Width = 80,
                Height = 155,
                Fill = dummyBackground
            };

            Canvas.SetLeft(newRec, x);
            Canvas.SetTop(newRec, y);

            MyCanvas.Children.Add(newRec);

        }
    }
}
