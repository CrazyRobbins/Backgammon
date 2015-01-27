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
    public partial class GameBoard : UserControl
    {
        Ellipse _pieceSelected = null;
        Point _posOfMouseOnHit;
        Point _posOfEllipseOnHit;
        Brush strokeColor;
        Model model = new Model();
        Player black = new Player();
        Player white = new Player();
        Player activePlayer, inactivePlayer;
        BlurEffect blur = new BlurEffect();
        Polygon[] polygons;
        Ellipse[] piecesInPolygon;
        private int dice1, dice2;
        private int start = 0;
        int _totalChildren = 0;
        int oldAmount = 0;
        TextBlock numberOfPieces;

        private int down, upp;

        ImageBrush piece_light = new ImageBrush();
        ImageBrush piece_dark = new ImageBrush();
        ImageBrush polygon_light = new ImageBrush();
        ImageBrush polygon_dark = new ImageBrush();
        ImageBrush home_image = new ImageBrush();

        void Active(Object sender, EventArgs args)
        {
        } // Active

        void DeActive(Object sender, EventArgs args)
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
                Canvas.SetLeft(_pieceSelected, _posOfEllipseOnHit.X);
                Canvas.SetTop(_pieceSelected, _posOfEllipseOnHit.Y);
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

            polygons = new Polygon[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23 };

            blackTurnArrow.Opacity = 0;
            whiteTurnArrow.Opacity = 0;

            for (int i = 0; i < 24; i++)
            {
                black._laces[i] = 0;
                white._laces[i] = 0;
            }
        } // GAMEBOARD

        private void lightup_pieces()
        {
            for (int i = 0; i < 24; i++)
            {
                if (activePlayer._laces[i] > 0 && DiceRoll.IsEnabled == false)
                {
                    int d1 = 0, d2 = 0, d3 = 0;
                    if (activePlayer == black)
                    {
                        d1 = i - dice1;
                        d2 = i - dice2;
                        d3 = i - dice1 - dice2;
                    }
                    else
                    {
                        d1 = i + dice1;
                        d2 = i + dice2;
                        d3 = i + dice1 + dice2;
                    }
                    if (activePlayer == black && ((d1 >= 0 && d1 != i && inactivePlayer._laces[d1] <= 1) || (d2 >= 0 && d2 != i && inactivePlayer._laces[d2] <= 1)))
                    {
                        Point p;
                        if (activePlayer._laces[i] > 5)
                        {
                            if (i < 12)
                                p = new Point(model.pointX[i] + 12, 24 * 5 - 12);
                            else
                                p = new Point(model.pointX[i] + 12, 320 - 24 * 5 + 12);
                        }
                        else
                        {
                            if (i < 12)
                                p = new Point( model.pointX[i] + 12, activePlayer._laces[i] * 24 - 12);
                            else
                                p = new Point( model.pointX[i] + 12, 320 - activePlayer._laces[i] * 24 + 12);
                        }
                        HitTestResult hr = VisualTreeHelper.HitTest( theCanvas, p );
                        Object obj = hr.VisualHit;
                        if (obj is Ellipse)
                        {
                            Ellipse el = (Ellipse)obj;
                            el.Stroke = Brushes.Red;
                        }
                    }
                    else if (activePlayer == white && ((d1 <= 23 && d1 != i && inactivePlayer._laces[d1] <= 1) || (d2 <= 23 && d2 != i && inactivePlayer._laces[d2] <= 1)))
                    {
                        Point p;
                        if (activePlayer._laces[i] > 5)
                        {
                            if (i < 12)
                                p = new Point( model.pointX[i] + 12, 24 * 5 - 12 );
                            else
                                p = new Point( model.pointX[i] + 12, 320 - 24 * 5 + 12 );
                        }
                        else
                        {
                            if (i < 12)
                                p = new Point( model.pointX[i] + 12, activePlayer._laces[i] * 24 - 12 );
                            else
                                p = new Point( model.pointX[i] + 12, 320 - activePlayer._laces[i] * 24 + 12 );
                        }
                        HitTestResult hr = VisualTreeHelper.HitTest( theCanvas, p );
                        Object obj = hr.VisualHit;
                        if (obj is Ellipse)
                        {
                            Ellipse el = (Ellipse)obj;
                            el.Stroke = Brushes.Red;
                        }
                    }
                }
            }
        }

        private void lightdown_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof( Ellipse ))
                {
                    Ellipse el = (Ellipse)theCanvas.Children[i];
                    el.Stroke = Brushes.Gray;
                }
            }
        }

        private void Canvas_MouseDown_1( object sender, MouseButtonEventArgs e )
        {
            Point pt = e.GetPosition(theCanvas);
            HitTestResult hr = VisualTreeHelper.HitTest(theCanvas, pt);
            Object obj = hr.VisualHit;
            start = 0;

            if (obj is Ellipse && DiceRoll.IsEnabled == false)
            {
                Ellipse el = (Ellipse)obj;
                if (el.Stroke == Brushes.Red)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        var p = e.GetPosition( polygons[i] );
                        Rect rect = VisualTreeHelper.GetDescendantBounds( polygons[i] );
                        if (rect.Contains( p ))
                        {
                            start = i;
                        }
                    }
                    _pieceSelected = el;
                    theCanvas.Children.Remove( _pieceSelected );
                    theCanvas.Children.Add( _pieceSelected );

                    _posOfMouseOnHit = pt;
                    _posOfEllipseOnHit.X = Canvas.GetLeft( _pieceSelected );
                    _posOfEllipseOnHit.Y = Canvas.GetTop( _pieceSelected );

                    lightdown_pieces();
                    _pieceSelected.Stroke = Brushes.Red;

                    if (dice1 != 0 || dice2 != 0)
                    {
                        if (activePlayer == black)
                        {
                           if(black._out>0)
                           {
                               if (24 - dice1 - dice2 >= 0 && white._laces[24 - dice1 - dice2] <= 1)
                               {
                                   polygons[24 - dice1 - dice2].Fill = Brushes.Yellow;
                               }
                               if (24 - dice1 >= 0 && dice1 != 0 && white._laces[24 - dice1] <= 1)
                               {
                                   polygons[24 - dice1].Fill = Brushes.Yellow;
                               }
                               if (24 - dice2 >= 0 && dice2 != 0 && white._laces[24 - dice2] <= 1)
                               {
                                   polygons[24 - dice2].Fill = Brushes.Yellow;
                               }
                           }
                           else
                           {
                            
                            

                            if (start - dice1 - dice2 >= 0 && white._laces[start - dice1 - dice2] <= 1)
                            {
                                polygons[start - dice1 - dice2].Fill = Brushes.Yellow;
                            }
                            if (black._goalReady && (start - dice1 - dice2) == -1)
                                blackHome.Background = Brushes.Red;
                            else if (black._goalReady && (start - dice1 - dice2) < -1)
                                blackHome.Background = Brushes.Red;
                            if (start - dice1 >= 0 && dice1 != 0 && white._laces[start - dice1] <= 1)
                            {
                                polygons[start - dice1].Fill = Brushes.Yellow;
                            }
                            if (black._goalReady && (start - dice1) == -1)
                                blackHome.Background = Brushes.Red;
                            else if (black._goalReady && (start - dice1 - dice2) < -1)
                                blackHome.Background = Brushes.Red;
                            if (start - dice2 >= 0 && dice2 != 0 && white._laces[start - dice2] <= 1)
                            {
                                polygons[start - dice2].Fill = Brushes.Yellow;
                            }
                            if (black._goalReady && (start - dice2) == -1)
                                blackHome.Background = Brushes.Red;
                            else if (black._goalReady && (start - dice1 - dice2) < -1)
                                blackHome.Background = Brushes.Red;
                           }
                        }
                        else
                        {
                            if (white._out > 0)
                            {
                                if (-1 + dice1 + dice2 <= 23 && black._laces[-1 + dice1 + dice2] <= 1)
                                {
                                    polygons[-1 + dice1 + dice2].Fill = Brushes.Yellow;
                                }
                                if (-1 + dice1 <= 23 && dice1 != 0 && black._laces[-1 + dice1] <= 1)
                                {
                                    polygons[-1 + dice1].Fill = Brushes.Yellow;
                                }
                                if (-1 + dice2 <= 23 && dice2 != 0 && black._laces[-1 + dice2] <= 1)
                                {
                                    polygons[-1 + dice2].Fill = Brushes.Yellow;
                                }   
                            }

                            else
                            {
                                if (start + dice1 + dice2 <= 23 && black._laces[start + dice1 + dice2] <= 1)
                                {
                                    polygons[start + dice1 + dice2].Fill = Brushes.Yellow;
                                }
                                if (white._goalReady && (start + dice1 + dice2) == 24)
                                    whiteHome.Background = Brushes.Red;
                                else if (white._goalReady && (start + dice1 + dice2) > 24)
                                    whiteHome.Background = Brushes.Red;
                                if (start + dice1 <= 23 && dice1 != 0 && black._laces[start + dice1] <= 1)
                                {
                                    polygons[start + dice1].Fill = Brushes.Yellow;
                                }
                                if (white._goalReady && (start + dice1) == 24)
                                    whiteHome.Background = Brushes.Red;
                                else if (white._goalReady && (start + dice1 + dice2) > 24)
                                    whiteHome.Background = Brushes.Red;
                                if (start + dice2 <= 23 && dice2 != 0 && black._laces[start + dice2] <= 1)
                                {
                                    polygons[start + dice2].Fill = Brushes.Yellow;
                                }
                                if (white._goalReady && (start + dice2) == 24)
                                    whiteHome.Background = Brushes.Red;
                                else if (white._goalReady && (start + dice1 + dice2) > 24)
                                    whiteHome.Background = Brushes.Red;
                            }
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
                ds.Color = Color.FromRgb(100, 100, 100);
                ds.ShadowDepth = 5;
                _pieceSelected.Effect = ds;
                Point pt = e.GetPosition(theCanvas);
                Canvas.SetLeft(_pieceSelected, (pt.X - _posOfMouseOnHit.X) + _posOfEllipseOnHit.X);
                Canvas.SetTop(_pieceSelected, (pt.Y - _posOfMouseOnHit.Y) + _posOfEllipseOnHit.Y);
            }
        } // Mouse Move

        private void Canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (_pieceSelected != null)
            {
                bool hit = false;
                polygon_light.ImageSource = new BitmapImage(new Uri(@"Grafik/metal-light.jpg", UriKind.Relative));
                polygon_dark.ImageSource = new BitmapImage(new Uri(@"Grafik/metal-dark.jpg", UriKind.Relative));
                home_image.ImageSource = new BitmapImage(new Uri(@"Grafik/wood-border2.jpg", UriKind.Relative));

                if (moveToFinish())
                {
                    activePlayer._goalReady = true;
                }
                else
                {
                    // activePlayer._goalReady = false;
                }

                for (int i = 0; i < 24; i++)
                {
                    if (polygons[i].Fill == Brushes.Yellow)
                    {
                        var p = e.GetPosition(polygons[i]);
                        Rect rect = VisualTreeHelper.GetDescendantBounds(polygons[i]);
                        if (rect.Contains(p))
                        {   
                            Canvas.SetLeft(_pieceSelected, model.pointX[i]);

                            if (activePlayer == black)
                            {
                                if (black._out == 0)
                                {
                                    black._laces[start]--;
                                }
                              
                                black._laces[i]++;
                                activePlayer = black;
                            }
                            else
                            {
                                if (white._out == 0)
                                {
                                    white._laces[start]--;
                                }    
                                white._laces[i]++;
                                activePlayer = white;
                            }

                            int spacing = 24;

                            if (activePlayer._laces[i] > 5)
                            {
                                if (i < 12)
                                    Canvas.SetTop( _pieceSelected, spacing * 4 );
                                else
                                    Canvas.SetTop( _pieceSelected, 320 - spacing * 5 );

                                numberOfPieces = new TextBlock();
                                Panel panel = (Panel)numberOfPieces.Parent;
                                theCanvas.Children.Remove( numberOfPieces );
                                theCanvas.Children.Add( numberOfPieces );
                                numberOfPieces.Height = 20;
                                numberOfPieces.Width = 20;
                                Canvas.SetLeft( numberOfPieces, Canvas.GetLeft( _pieceSelected ) );
                                Canvas.SetTop( numberOfPieces, Canvas.GetTop( _pieceSelected ) );
                                numberOfPieces.Foreground = Brushes.Red;
                                numberOfPieces.Text = activePlayer._laces[i].ToString();
                            }
                            else
                            {
                                if (i < 12)
                                    Canvas.SetTop( _pieceSelected, spacing * (activePlayer._laces[i] - 1) );
                                else
                                    Canvas.SetTop( _pieceSelected, 320 - spacing * activePlayer._laces[i] );
                            }

                            hit = true;

                            int distance = 0;
                            if (activePlayer._out > 0)
                            {
                                if (activePlayer == black)
                                    distance = 24 - i;
                                else
                                    distance = i - (-1);
                                activePlayer._out--;
                            }

                            else
                            {
                                if (activePlayer == black)
                                    distance = start - i;
                                else
                                    distance = i - start;
                            }
                            if (distance == dice1 + dice2)
                            {
                                dice1 = 0;
                                dice2 = 0;
                                DiceView1.Effect = blur;
                                DiceView2.Effect = blur;
                            }
                            if (dice1 != dice2)
                            {
                                if (distance == dice1)
                                {
                                    dice1 = 0;
                                    DiceView1.Effect = blur;
                                }
                                if (distance == dice2)
                                {
                                    dice2 = 0;
                                    DiceView2.Effect = blur;
                                }
                            }
                            else
                            {
                                dice1 = 0;
                                DiceView1.Effect = blur;
                            }
                        }
                        if (black._laces[i] == 1 && white._laces[i] == 1)
                        {
                            _totalChildren = theCanvas.Children.Count - 1;
                            for (int j = _totalChildren; j > 0; j--)
                            {
                                if (theCanvas.Children[j].GetType() == typeof( Ellipse ))
                                {
                                    Ellipse el = (Ellipse)theCanvas.Children[j];
                                    if (i < 12 && Canvas.GetLeft( el ) == model.pointX[i] && Canvas.GetTop( el ) == 0)
                                    {
                                        if (activePlayer == black && el.Fill == piece_light)
                                        {
                                            Canvas.SetLeft( el, 183 );
                                            Canvas.SetTop( el, 68 );
                                            white._laces[i]--;
                                            white._out++;
                                        }
                                        else if (activePlayer == white && el.Fill == piece_dark)
                                        {
                                            Canvas.SetLeft( el, 183 );
                                            Canvas.SetTop( el, 228 );
                                            black._laces[i]--;
                                            black._out++;
                                        }
                                    }
                                    else if (Canvas.GetLeft( el ) == model.pointX[i] && Canvas.GetTop( el ) == 296)
                                    {
                                        if (activePlayer == black && el.Fill == piece_light)
                                        {
                                            Canvas.SetLeft( el, 183 );
                                            Canvas.SetTop( el, 228 );
                                            white._laces[i]--;
                                            white._out++;
                                        }
                                        else if (activePlayer == white && el.Fill == piece_dark)
                                        {
                                            Canvas.SetLeft( el, 183 );
                                            Canvas.SetTop( el, 68 );
                                            black._laces[i]--;
                                            black._out++;
                                        }
                                    }
                                }
                            }
                        }

                        if (i % 2 == 0)
                            polygons[i].Fill = polygon_dark;
                        else
                            polygons[i].Fill = polygon_light;
                    }

                    else if (blackHome.Background == Brushes.Red || whiteHome.Background == Brushes.Red)
                    {
                        hit = true;

                        int distance = 0;
                        if (activePlayer == black)
                            distance = start - (-1);
                        else
                            distance = 24 - start;
                        if (distance <= dice1 + dice2 && distance > dice1 && distance > dice2)
                        {
                            dice1 = 0;
                            dice2 = 0;
                            DiceView1.Effect = blur;
                            DiceView2.Effect = blur;
                        }
                        if (dice1 != dice2)
                        {
                            if (distance == dice1)
                            {
                                dice1 = 0;
                                DiceView1.Effect = blur;
                            }
                            if (distance == dice2)
                            {
                                dice2 = 0;
                                DiceView2.Effect = blur;
                            }

                            if (distance < dice1 && distance < dice2)
                            {
                                if (dice1 > dice2)
                                {
                                    dice1 = 0;
                                    DiceView1.Effect = blur;

                                }
                                else
                                {
                                    dice2 = 0;
                                    DiceView2.Effect = blur;
                                }
                            } 
                        }
                        else
                        {
                            dice1 = 0;
                            DiceView1.Effect = blur;
                        }

                        activePlayer._laces[start]--;
                        activePlayer._bricksAmount--;
                    }

                }
              
                if (!hit)
                {
                    Canvas.SetTop( _pieceSelected, _posOfEllipseOnHit.Y );
                    Canvas.SetLeft( _pieceSelected, _posOfEllipseOnHit.X );
                }
                whiteHome.Background = home_image;
                blackHome.Background = home_image;

                _pieceSelected.Effect = null;
                _pieceSelected.Stroke = Brushes.Gray;
                _pieceSelected = null;

                if(activePlayer._bricksAmount==0)
                {
                //win
                }

                if (dice1 == 0 && dice2 == 0)
                {
                    changePlayer();
                    lightdown_pieces();
                }
                else
                {
                    lightdown_pieces();
                    lightup_pieces();
                }
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
            lightup_pieces();
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Ljud\roll.wav");
            //player.Play();
        } // diceRoll

        private void changePlayer()
        {
            if (activePlayer == white)
            {
                activePlayer = black;
                inactivePlayer = white;
                blackTurnArrow.Opacity = 1;
                whiteTurnArrow.Opacity = 0;
            }
            else
            {
                activePlayer = white;
                inactivePlayer = black;
                blackTurnArrow.Opacity = 0;
                whiteTurnArrow.Opacity = 1;
            }
            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
        } // changePlayer

        private bool moveToFinish()
        {
            if (activePlayer == black)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (black._laces[i] != 0 && i != 0 && i != 1 && i != 2 && i != 3 && i != 4 && i != 5)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    if (white._laces[i] != 0 && i != 18 && i != 19 && i != 20 && i != 21 && i != 22 && i != 23)
                    {
                        return false;
                    }
                }
                return true;
            }
        } // moveToFinish

        private Ellipse load_piece()
        {
            Ellipse _piece = new Ellipse();
            _piece.Height = 24;
            _piece.Width = 24;
            _piece.Stroke = Brushes.Gray;
            theCanvas.Children.Add(_piece);
            Canvas.SetLeft(_piece, 0);
            Canvas.SetTop(_piece, 0);
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
                    Canvas.SetLeft(_piece, 3);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    white._laces[11] = 5;
                }
                else if (i < 7)
                {
                    Canvas.SetLeft(_piece, 363);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    white._laces[0] = 2;
                }
                else if (i < 10)
                {
                    Canvas.SetLeft(_piece, 123);
                    Canvas.SetTop(_piece, 248 + (rad * (i % 7)));
                    white._laces[16] = 3;
                }
                else
                {
                    Canvas.SetLeft(_piece, 213);
                    Canvas.SetTop(_piece, 200 + (rad * (i % 5)));
                    white._laces[18] = 5;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_dark;
                if (i < 5)
                {
                    Canvas.SetLeft(_piece, 3);
                    Canvas.SetTop(_piece, 200 + (rad * (i % 5)));
                    black._laces[12] = 5;
                }
                else if (i < 7)
                {
                    Canvas.SetLeft(_piece, 363);
                    Canvas.SetTop(_piece, 272 + (rad * (i % 5)));
                    black._laces[23] = 2;
                }
                else if (i < 10)
                {
                    Canvas.SetLeft(_piece, 123);
                    Canvas.SetTop(_piece, rad * (i % 7));
                    black._laces[7] = 3;
                }
                else
                {
                    Canvas.SetLeft(_piece, 213);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    black._laces[5] = 5;
                }
            }
        } // insert_pieces

        private void remove_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof(Ellipse))
                {
                    Ellipse _piece = (Ellipse)theCanvas.Children[i];
                    theCanvas.Children.Remove(_piece);
                }
            }
        } // remove_pieces

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
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
            dice1 = 0;
            dice2 = 0;

            blackTurnArrow.Opacity = 1;
            whiteTurnArrow.Opacity = 0;

            piece_light.ImageSource = new BitmapImage(new Uri(@"Grafik/piece-white.jpg", UriKind.Relative));
            piece_dark.ImageSource = new BitmapImage(new Uri(@"Grafik/piece-black.jpg", UriKind.Relative));

            remove_pieces();
            insert_pieces();
            activePlayer = black;
            inactivePlayer = white;
        } // new_game

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
    }
}

