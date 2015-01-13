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

namespace Backgammon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse _pieceSelected = null;
        Point _posOfMouseOnHit;
        Point _posOfEllipseOnHit;
        Brush strokeColor;
        Model model = new Model();
        
        public MainWindow()
        {
            InitializeComponent();

        }

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

                Panel parentPanel = (Panel)_pieceSelected.Parent;
                parentPanel.Children.Remove(_pieceSelected);
                theCanvas.Children.Add(_pieceSelected);

                _posOfMouseOnHit = pt;
                _posOfEllipseOnHit.X = Canvas.GetLeft(_pieceSelected);
                _posOfEllipseOnHit.Y = Canvas.GetTop(_pieceSelected);
            }
        }

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (_pieceSelected != null)
            {
                Point pt = e.GetPosition(theCanvas);
                Canvas.SetLeft(_pieceSelected, (pt.X - _posOfMouseOnHit.X) + _posOfEllipseOnHit.X);
                Canvas.SetTop(_pieceSelected, (pt.Y - _posOfMouseOnHit.Y) + _posOfEllipseOnHit.Y);
            }
        }

        private void Canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (_pieceSelected != null)
            {
                _pieceSelected.Stroke = strokeColor;
                _pieceSelected = null;
            }
        }
    }
}
