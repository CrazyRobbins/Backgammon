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
        Grid[] grids;
        private int dice1, dice2;
        private int start = 0;
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
            if (_pieceSelected != null)
            {
                Canvas.SetLeft( _pieceSelected, _posOfEllipseOnHit.X );
                Canvas.SetTop( _pieceSelected, _posOfEllipseOnHit.Y );
                _pieceSelected.Effect = null;
                _pieceSelected.Stroke = strokeColor;
                _pieceSelected = null;
            }
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

            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;

            polygons = new Polygon[] {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23};
            grids = new Grid[] { g0, g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12, g13, g14, g15, g16, g17, g18, g19, g20, g21, g22, g23 };

            blackTurnArrow.Opacity = 0;
            whiteTurnArrow.Opacity = 0;

            black._laces[23] = 2;
            black._laces[12] = 5;
            black._laces[7] = 3;
            black._laces[5] = 5;
            white._laces[0] = 2;
            white._laces[11] = 5;
            white._laces[16] = 3;
            white._laces[18] = 5;
            
        } // GAMEBOARD

        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(theCanvas);
            HitTestResult hr = VisualTreeHelper.HitTest(theCanvas, pt);
            Object obj = hr.VisualHit;
            start = 0;
            
            if (obj is Ellipse && DiceRoll.IsEnabled == false)
            {
                Ellipse el = (Ellipse)obj;
                for (int i = 0; i < 24; i++)
                {
                    var p = e.GetPosition( polygons[i] );
                    Rect rect = VisualTreeHelper.GetDescendantBounds( polygons[i] );
                    if (rect.Contains(p))
                    {
                        start = i;
                    }
                }
                
                if ((activePlayer == black && el.Fill == piece_dark) || (activePlayer == white && el.Fill == piece_light))
                {
                    _pieceSelected = el;
                    theCanvas.Children.Remove( _pieceSelected );
                    theCanvas.Children.Add( _pieceSelected );

                    _posOfMouseOnHit = pt;
                    _posOfEllipseOnHit.X = Canvas.GetLeft( _pieceSelected );
                    _posOfEllipseOnHit.Y = Canvas.GetTop( _pieceSelected );

                    strokeColor = _pieceSelected.Stroke;
                    _pieceSelected.Stroke = Brushes.Red;

                    if (dice1 != 0 || dice2 != 0)
                    {
                        if (activePlayer == black)
                        {
                            if (start - dice1 - dice2 >= 0)
                                polygons[start - dice1 - dice2].Fill = Brushes.Yellow;
                            if (start - dice1 >= 0 && dice1 != 0)
                                polygons[start - dice1].Fill = Brushes.Yellow;
                            if (start - dice2 >= 0 && dice2 != 0)
                                polygons[start - dice2].Fill = Brushes.Yellow;
                        }
                        else
                        {
                            if (start + dice1 + dice2 <= 23)
                                polygons[start + dice1 + dice2].Fill = Brushes.Yellow;
                            if (start + dice1 <= 23 && dice1 != 0)
                                polygons[start + dice1].Fill = Brushes.Yellow;
                            if (start + dice2 <= 23 && dice2 != 0)
                                polygons[start + dice2].Fill = Brushes.Yellow;
                        }
                    }
                }
            }
        } // Mouse Down

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (_pieceSelected != null)
            {
                DropShadowEffect ds = new DropShadowEffect();

                if ((activePlayer == black && _pieceSelected.Fill == piece_dark) || (activePlayer == white && _pieceSelected.Fill == piece_light))
                {
                    ds.Color = Color.FromRgb( 100, 100, 100 );
                    ds.ShadowDepth = 5;
                    _pieceSelected.Effect = ds;
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
                bool hit = false;
                polygon_light.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-light.jpg", UriKind.Relative ) );
                polygon_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-dark.jpg", UriKind.Relative ) );

                for (int i = 0; i < 24; i++)
                {
                    if (polygons[i].Fill == Brushes.Yellow)
                    {
                        var p = e.GetPosition( polygons[i] );
                        Rect rect = VisualTreeHelper.GetDescendantBounds( polygons[i] );
                        if (rect.Contains( p ))
                        {
                            model.changeArray( newY, newX, _posOfEllipseOnHit.Y, _posOfEllipseOnHit.X, activePlayer );
                            double posX = model.fixPositionX( newX );
                            Canvas.SetLeft( _pieceSelected, posX );
                            hit = true;
                            if (dice1 != 0 || dice2 != 0)
                            {
                                if (activePlayer == black)
                                {
                                    if (start - dice1 - dice2 >= 0)
                                    {
                                        if (polygons[i] == polygons[start - dice1 - dice2])
                                        {
                                            dice1 = 0;
                                            dice2 = 0;
                                            DiceView1.Effect = blur;
                                            DiceView2.Effect = blur;
                                        }
                                    }
                                    if (dice1 != dice2)
                                    {
                                        if (start - dice1 >= 0)
                                        {
                                            if (polygons[i] == polygons[start - dice1])
                                            {
                                                dice1 = 0;
                                                DiceView1.Effect = blur;
                                            }
                                        }
                                        if (start - dice2 >= 0)
                                        {
                                            if (polygons[i] == polygons[start - dice2])
                                            {
                                                dice2 = 0;
                                                DiceView2.Effect = blur;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (start - dice1 >= 0)
                                            if (polygons[i] == polygons[start - dice1])
                                            {
                                                dice1 = 0;
                                                DiceView1.Effect = blur;
                                            }
                                    }
                                }
                                else
                                {
                                    if (start + dice1 + dice2 <= 23)
                                    {
                                        if (polygons[i] == polygons[start + dice1 + dice2])
                                        {
                                            dice1 = 0;
                                            dice2 = 0;
                                            DiceView1.Effect = blur;
                                            DiceView2.Effect = blur;
                                        }
                                    }
                                    if (dice1 != dice2)
                                    {
                                        if (start + dice1 <= 23)
                                        {
                                            if (polygons[i] == polygons[start + dice1])
                                            {
                                                dice1 = 0;
                                                DiceView1.Effect = blur;
                                            }
                                        }
                                        if (start + dice2 <= 23)
                                        {
                                            if (polygons[i] == polygons[start + dice2])
                                            {
                                                dice2 = 0;
                                                DiceView2.Effect = blur;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (start + dice1 >= 0)
                                            if (polygons[i] == polygons[start + dice1])
                                            {
                                                dice1 = 0;
                                                DiceView1.Effect = blur;
                                            }
                                    }
                                }
                            }
                        }
                    if (i % 2 == 0)
                    {
                        polygons[i].Fill = polygon_dark;
                    }
                    else
                    {
                        polygons[i].Fill = polygon_light;
                    }
                    } 
                }
                if (!hit)
                {
                    Canvas.SetTop( _pieceSelected, _posOfEllipseOnHit.Y );
                    Canvas.SetLeft( _pieceSelected, _posOfEllipseOnHit.X );
                }
                if (dice1 == 0 && dice2 == 0)
                {
                    changePlayer();
                }
                _pieceSelected.Effect = null;
                _pieceSelected.Stroke = strokeColor;
                _pieceSelected = null;
            }
        } // Mouse Up

        private void diceRoll(object sender, RoutedEventArgs e)
        {
            dice1 = model.dice();
            dice2 = model.dice();
            DiceView1.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice1.ToString() + ".png", UriKind.Relative));
            DiceView2.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice2.ToString() + ".png", UriKind.Relative));
            DiceView1.Opacity = 1;
            DiceView2.Opacity = 1;
            DiceView1.Effect = null;
            DiceView2.Effect = null;
            DiceRoll.Opacity = 0;
            DiceRoll.IsEnabled = false;
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Ljud\roll.wav");
            //player.Play();
        } // diceRoll

        private void changePlayer()
        {
            if (activePlayer == white)
            {
                activePlayer = black;
                inActivePlayer = white;
                blackTurnArrow.Opacity = 1;
                whiteTurnArrow.Opacity = 0;
            }
            else
            {
                activePlayer = white;
                inActivePlayer = black;
                blackTurnArrow.Opacity = 0;
                whiteTurnArrow.Opacity = 1;
            }
            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
        } // changePlayer

        private Ellipse load_piece()
        {
            Ellipse _piece = new Ellipse();
            _piece.Height = 24;
            _piece.Width = 24;
            _piece.Fill = Brushes.Azure;
            theCanvas.Children.Add( _piece );
            Canvas.SetLeft( _piece, 0);
            Canvas.SetTop( _piece, 0);
            return _piece;
        } // load_piece

        private void insert_pieces()
        {
            int rad = 24;
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_light;
                if (i < 5)
                {
                    Canvas.SetLeft( _piece, 3 );
                    Canvas.SetTop( _piece, rad * (i % 5) );
                }
                else if (i < 7)
                {
                    Canvas.SetLeft( _piece, 363 );
                    Canvas.SetTop( _piece, rad * (i % 5) );
                }
                else if (i < 10)
                {
                    Canvas.SetLeft( _piece, 123 );
                    Canvas.SetTop( _piece, 248 + (rad * (i % 7)) );
                }
                else
                {
                    Canvas.SetLeft( _piece, 213 );
                    Canvas.SetTop( _piece, 200 + (rad * (i % 5)) );
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_dark;
                if (i < 5)
                {
                    Canvas.SetLeft( _piece, 3 );
                    Canvas.SetTop( _piece, 200 + (rad * (i % 5)) );
                }
                else if (i < 7)
                {
                    Canvas.SetLeft( _piece, 363 );
                    Canvas.SetTop( _piece, 272 + (rad * (i % 5)) );
                }
                else if (i < 10)
                {
                    Canvas.SetLeft( _piece, 123 );
                    Canvas.SetTop( _piece, rad * (i % 7) );
                }
                else
                {
                    Canvas.SetLeft( _piece, 213 );
                    Canvas.SetTop( _piece, rad * (i % 5) );
                }
            }
        } // insert_pieces

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

        private void resume_game( object sender, RoutedEventArgs e )
        {
            bool noEllipse = true;
            int _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof( Ellipse ))
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

        private void new_game( object sender, RoutedEventArgs e )
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
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
            dice1 = 0;
            dice2 = 0;

            blackTurnArrow.Opacity = 1;
            whiteTurnArrow.Opacity = 0;
            activePlayer = black;
            inActivePlayer = white;

            piece_light.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-white.jpg", UriKind.Relative ) );
            piece_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-black.jpg", UriKind.Relative ) );

            remove_pieces();
            insert_pieces();
        } // new_game

        private void exit_game( object sender, RoutedEventArgs e )
        {
            Application.Current.Shutdown();
        } // exit_game

        private void menu_action( object sender, RoutedEventArgs e )
        {
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.IsHitTestVisible = false;
            theCanvas.Effect = blur;
            menuGrid.IsHitTestVisible = true;
            menuGrid.IsEnabled = true;
            menuGrid.Opacity = 1;
        } // menu_action
    }
}
