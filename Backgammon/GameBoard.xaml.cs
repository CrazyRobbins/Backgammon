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
using System.Windows.Media.Effects;

namespace Backgammon
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        Ellipse _pieceSelected = null;
        Point _posOfMouseOnHit;
        Point _posOfEllipseOnHit;
        Brush strokeColor;
        Model model = new Model();
        Player black = new Player();
        Player white = new Player();
        Player activePlayer;
        Player inActivePlayer;
        BlurEffect blur = new BlurEffect();
        Polygon[] polygons;
        private int dice1, dice2;
        private int polygon1, polygon2;
        private bool changePlayerBool;
        private int turn;
        int _totalChildren = 0;

        ImageBrush piece_light = new ImageBrush();
        ImageBrush piece_dark = new ImageBrush();
        ImageBrush polygon_light = new ImageBrush();
        ImageBrush polygon_dark = new ImageBrush();

        void Active( Object sender, EventArgs args )
        {
        } // Active

        void DeActive( Object sender, EventArgs args )
        {
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.IsHitTestVisible = false;
            theCanvas.Effect = blur;
            menuGrid.IsHitTestVisible = true;
            menuGrid.IsEnabled = true;
            menuGrid.Opacity = 1;
        } // DeActive

        public GameBoard()
        {
            App.Current.Activated += Active;
            App.Current.Deactivated += DeActive;
            InitializeComponent();
            menuGrid.Opacity = 1;
            menuGrid.IsEnabled = true;
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.Effect = blur;

            DiceView.Opacity = 0;
            DiceView2.Opacity = 0;

            polygons = new Polygon[] {p0, p1, p2, p3, p4, p5, p6, p7,
                                      p8, p9, p10, p11, p12, p13, p14,
                                      p15, p16, p17, p18, p19, p20,
                                      p21, p22, p23};

            activePlayer = black;
            inActivePlayer = white;
            changePlayerBool = true;
            turn = 0;
            polygon1 = -1;
            polygon2 = -1;

            black._laces[23] = 2;
            black._laces[12] = 5;
            black._laces[7] = 3;
            black._laces[5] = 5;
            white._laces[0] = 2;
            white._laces[11] = 5;
            white._laces[16] = 3;
            white._laces[18] = 5;  
        } // GAMEBOARD

        // resume button
        private void resume_game(object sender, RoutedEventArgs e)
        {
            bool noEllipse = true;
            int _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof(Ellipse))
                {
                    noEllipse = false;
                }
            }

            if (!noEllipse)
            {
                menuGrid.IsHitTestVisible = false;
                menuGrid.IsEnabled = false;
                menuGrid.Opacity = 0;
                theCanvas.Opacity = 1;
                theCanvas.IsEnabled = true;
                theCanvas.IsHitTestVisible = true;
                theCanvas.Effect = null;
            }
        } // resume_game

        private void remove_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof( Ellipse ))
                {
                    Ellipse _piece = (Ellipse)theCanvas.Children[i];
                    theCanvas.Children.Remove( _piece );
                }
            }
        } // remove_pieces

        private void insert_pieces()
        {
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_light;
                if (i < 5)
                {
                    Canvas.SetLeft( _piece, 1 );
                    Canvas.SetTop( _piece, 25 * (i % 5) );
                }
                else if (i < 7)
                {
                    Canvas.SetLeft( _piece, 364 );
                    Canvas.SetTop( _piece, 25 * (i % 5) );
                }
                else if (i < 10)
                {
                    Canvas.SetLeft( _piece, 115 );
                    Canvas.SetTop( _piece, 245 + (25 * (i % 7)) );
                }
                else
                {
                    Canvas.SetLeft( _piece, 222 );
                    Canvas.SetTop( _piece, 195 + (25 * (i % 5)) );
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_dark;
                if (i < 5)
                {
                    Canvas.SetLeft( _piece, 1 );
                    Canvas.SetTop( _piece, 195 + (25 * (i % 5)) );
                }
                else if (i < 7)
                {
                    Canvas.SetLeft( _piece, 364 );
                    Canvas.SetTop( _piece, 270 + (25 * (i % 5)) );
                }
                else if (i < 10)
                {
                    Canvas.SetLeft( _piece, 115 );
                    Canvas.SetTop( _piece, 25 * (i % 7) );
                }
                else
                {
                    Canvas.SetLeft( _piece, 222 );
                    Canvas.SetTop( _piece, 25 * (i % 5) );
                }
            }
        } // insert_pieces

        // new game button
        private void new_game(object sender, RoutedEventArgs e)
        {
            menuGrid.IsHitTestVisible = false;
            menuGrid.IsEnabled = false;
            menuGrid.Opacity = 0;
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = true;
            theCanvas.IsHitTestVisible = true;
            theCanvas.Effect = null;

            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView.Opacity = 0;
            DiceView2.Opacity = 0;

            piece_light.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-white.jpg", UriKind.Relative ) );
            piece_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-black.jpg", UriKind.Relative ) );

            remove_pieces();
            insert_pieces();
        } // insert_pieces

        private void exit_game(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } // exit_game

        private void menu_action(object sender, RoutedEventArgs e)
        {
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.IsHitTestVisible = false;
            theCanvas.Effect = blur;
            menuGrid.IsHitTestVisible = true;
            menuGrid.IsEnabled = true;
            menuGrid.Opacity = 1;
        } // menu_action

        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(theCanvas);
            HitTestResult hr = VisualTreeHelper.HitTest(theCanvas, pt);
            Object obj = hr.VisualHit;

            if (obj is Ellipse)
            {
                _pieceSelected = (Ellipse)obj;

                strokeColor = _pieceSelected.Stroke;
                _pieceSelected.Stroke = Brushes.Red;

                theCanvas.Children.Remove(_pieceSelected);
                theCanvas.Children.Add(_pieceSelected);

                _posOfMouseOnHit = pt;
                _posOfEllipseOnHit.X = Canvas.GetLeft(_pieceSelected);
                _posOfEllipseOnHit.Y = Canvas.GetTop(_pieceSelected);
            }

            if (activePlayer == black)
            {
                polygon1 = model.lightUp( dice1, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, true );
                polygons[polygon1].Fill = Brushes.Yellow;
                polygon2 = model.lightUp( dice2, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, true );
                polygons[polygon2].Fill = Brushes.Yellow;
            }
            else if (activePlayer == white)
            {
                polygon1 = model.lightUp( dice1, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, false );
                polygons[polygon1].Fill = Brushes.Yellow;
                polygon2 = model.lightUp( dice2, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, false );
                polygons[polygon2].Fill = Brushes.Yellow;
            }
        } // Mouse Down

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (_pieceSelected != null)
            {
                if ((activePlayer == black && _pieceSelected.Fill == piece_dark) || (activePlayer == white && _pieceSelected.Fill == piece_light))
                {
                    Point pt = e.GetPosition( theCanvas );
                    Canvas.SetLeft( _pieceSelected, (pt.X - _posOfMouseOnHit.X) + _posOfEllipseOnHit.X );
                    Canvas.SetTop( _pieceSelected, (pt.Y - _posOfMouseOnHit.Y) + _posOfEllipseOnHit.Y );
                }
            }
        } // Mouse Move

        private void Canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (_pieceSelected != null)
            {
                double newY = Canvas.GetTop(_pieceSelected);
                double newX = Canvas.GetLeft(_pieceSelected);
                if (!model.check(newY, newX, inActivePlayer) || newX >= 160 && newX <= 197 || newX > 376)
                {
                    Canvas.SetTop(_pieceSelected, _posOfEllipseOnHit.Y);
                    Canvas.SetLeft(_pieceSelected, _posOfEllipseOnHit.X);
                }
                else
                {
                    model.changeArray(newY, newX, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, activePlayer);
                    double position = model.fixPosition( newX );
                    Canvas.SetLeft( _pieceSelected, position );
                    turn = turn + 1;
                    if (turn == 2)
                    {
                        changePlayer();
                        turn = 0;
                    }
                }
                _pieceSelected.Stroke = strokeColor;
                _pieceSelected = null;
            }
            if (polygon1 >= 0 && polygon2 >= 0)
            {
                lightDown();
            }
        } // Mouse Up

        private void diceRoll(object sender, RoutedEventArgs e)
        {
            
            dice1 = model.dice();
            dice2 = model.dice();
            DiceView.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice1.ToString() + ".png", UriKind.Relative));
            DiceView2.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice2.ToString() + ".png", UriKind.Relative));
            DiceView.Opacity = 1;
            DiceView2.Opacity = 2;

            DiceRoll.Opacity = 0;
            DiceRoll.IsEnabled = false;

            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Ljud\roll.wav");
            //player.Play();
        } // diceRoll

        private Ellipse load_piece()
        {
            Ellipse _piece = new Ellipse();

            _piece.Height = 25;
            _piece.Width = 25;
            _piece.Fill = Brushes.Azure;
            theCanvas.Children.Add(_piece);
            Canvas.SetLeft(_piece, 20);
            Canvas.SetTop(_piece, 20);
            
            return _piece;
        } // load_piece

        private void lightDown()
        {
            polygon_light.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-light.jpg", UriKind.Relative ) );
            polygon_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-dark.jpg", UriKind.Relative ) );

            if (polygon1 % 2 == 0)
            {
                polygons[polygon1].Fill = polygon_dark;
            }
            else
            {
                polygons[polygon1].Fill = polygon_light;
            }
            if (polygon2 % 2 == 0)
            {
                polygons[polygon2].Fill = polygon_dark;
            }
            else
            {
                polygons[polygon2].Fill = polygon_light;
            }
            polygon1 = -1;
            polygon2 = -1;
        } // lightDown

        private void changePlayer()
        {
            if (changePlayerBool == false)
            {
                activePlayer = black;
                inActivePlayer = white;
                changePlayerBool = true;
                DiceRoll.IsEnabled = true;
                DiceRoll.Opacity = 1;
                DiceView.Opacity = 0;
                DiceView2.Opacity = 0;
            }
            else if (changePlayerBool == true)
            {
                activePlayer = white;
                inActivePlayer = black;
                changePlayerBool = false;
                DiceRoll.IsEnabled = true;
                DiceRoll.Opacity = 1;
                DiceView.Opacity = 0;
                DiceView2.Opacity = 0;
            }
        } // changePlayer
    }
}
