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
public class CanvasAutoSize : Canvas
{
    protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
    {
        base.MeasureOverride(constraint);
        double width = base
            .InternalChildren
            .OfType<UIElement>()
            .Max(i => i.DesiredSize.Width + (double)i.GetValue(Canvas.LeftProperty));

        double height = base
            .InternalChildren
            .OfType<UIElement>()
            .Max(i => i.DesiredSize.Height + (double)i.GetValue(Canvas.TopProperty));

        return new Size(width, height);
    }
}

namespace WpfApp1_Overlap_check
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    /*public static class custRect
    {
        public static void OnClick(this Rectangle objDotnet, EventArgs e)
        {
            // Change the color of the line when clicked.
            this.BorderColor = Color.Red;
            base.OnClick(e);
        }
    }*/

    //
    public class ShapeWB
    {
        public Shape sh;
        public Transform tr;
        public float timestamp;
        public int uID;
        public int shapeID;

    }

    

    public partial class MainWindow : Window
    {
        Brush Custombrush, Custbrush;
        Random r = new Random();
        int overlap = 0;
        IntersectionDetail d1;
        Dictionary<int, Shape> map_key_to_shape_onRclick = new Dictionary<int, Shape>();
        Dictionary<int, Shape> map_key_to_shape_onLclick = new Dictionary<int, Shape>();
        Dictionary<Shape, int> map_shape_angle = new Dictionary<Shape, int>();
        Dictionary<Shape, TranslateTransform> map_shape_translate = new Dictionary<Shape, TranslateTransform>();
        int RectIndex;

        T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }




        public MainWindow()
        {
            InitializeComponent();
        }

        

        /*
        //https://stackoverflow.com/questions/50397435/method-fillcontainswithdetail-not-working-as-expected
        private Geometry ConvertToGeometry(Shape s)
        {
            if (s is Rectangle)
            {
                return new RectangleGeometry(new Rect(new Point(s.Margin.Left, s.Margin.Top), new Point(s.Margin.Left + s.Width, s.Margin.Top + s.Height)));
            }
            else if (s is Ellipse)
            {
                return new EllipseGeometry(new Point(s.Width / 2 + s.Margin.Left, s.Height / 2 + s.Margin.Top), s.Width / 2, s.Height / 2);
            }
            if (s is Polygon)
            {
                Polygon p = (Polygon)s;
                List<PathSegment> ps = new List<PathSegment>();
                for (int i = 1; i < p.Points.Count; i++)
                {
                    ps.Add(new LineSegment(p.Points[i], true));
                }
                PathGeometry pg = new PathGeometry(new PathFigure[] { new PathFigure(p.Points[0], ps, true) });
                return pg;
            }
            return null;
        }*/


        //https://stackoverflow.com/questions/46758647/wpf-how-to-detect-geometry-intersection-on-canvas
        private static Transform GetFullTransform(UIElement e)
        {
            // The order in which transforms are applied is important!
            var transforms = new TransformGroup();

            if (e.RenderTransform != null)
                transforms.Children.Add(e.RenderTransform);

            var xTranslate = (double)e.GetValue(Canvas.LeftProperty);
            if (double.IsNaN(xTranslate))
                xTranslate = 0D;

            var yTranslate = (double)e.GetValue(Canvas.TopProperty);
            if (double.IsNaN(yTranslate))
                yTranslate = 0D;

            var translateTransform = new TranslateTransform(xTranslate, yTranslate);
            transforms.Children.Add(translateTransform);
            

            return transforms;
        }
        public Geometry GetGeometry(Shape s)
        {
            var g = s.RenderedGeometry.Clone();
            g.Transform = GetFullTransform(s);
            return g;
        }

        //IMPORTANT
        private void rotateRect_on_Rclick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                // if the click source is a rectangle then we will create a new rectangle
                // and link it to the rectangle that sent the click event
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                //activeRec.RenderTransform = Transform.Identity;
                
                if ( map_key_to_shape_onRclick.Values.Contains(activeRec)) {
                    Rectangle newRec = new Rectangle
                    {
                        Width = activeRec.Width,
                       Height = activeRec.Height,
                       StrokeThickness = activeRec.StrokeThickness + 1,
                        Fill = activeRec.Fill,
                        Stroke = Brushes.Black
                    };
                    ///
                    map_shape_angle[activeRec] = map_shape_angle[activeRec] + 45;
                    RotateTransform rotateTransform1 = new RotateTransform
                    {
                        Angle = map_shape_angle[activeRec],
                        CenterX = (activeRec.Width / 2), //topleft_x,
                        CenterY = (activeRec.Height / 2) //topleft_y
                    };

                    /*RotateTransform rotateTransform2 = new RotateTransform
                    {
                        Angle = 30,
                        CenterX = 20, //topleft_x,
                        CenterY = 20 //topleft_y
                    };*/
                    TranslateTransform t = new TranslateTransform
                    {
                        X = 50,
                        Y = 100
                    };


                    var KeY = map_key_to_shape_onRclick.FirstOrDefault(x => x.Value == activeRec).Key;

                    //RotateTransform rot_tf = new RotateTransform { Angle = 90, CenterX = newRec.Width / 2, CenterY = newRec.Height / 2 };
                    ///Canvas.SetLeft(newRec, map_shape_translate[activeRec].X); // set the left position of rectangle to mouse X
                    ///Canvas.SetTop(newRec, map_shape_translate[activeRec].Y); // set the top position of rectangle to mouse Y
                    ///map_shape_translate[newRec] = map_shape_translate[activeRec];
                    ///map_shape_angle[newRec] = map_shape_angle[activeRec] + 45;


                    ///undone
                    ///MyCan.Children.Remove(activeRec);
                    ///map_shape_translate.Remove(activeRec);
                    ///map_shape_angle.Remove(activeRec);
                    ///undone

                    ///map_key_to_shape_onRclick[KeY] = newRec;
                    ///newRec.RenderTransform = rotateTransform1;
                    ///MyCan.Children.Add(newRec);

                    

                    Canvas.SetLeft(newRec, t.X);
                    Canvas.SetTop(newRec, t.Y);
                    //newRec.RenderTransform = rotateTransform2;

                    MyCan.Children[KeY].RenderTransform = rotateTransform1;

                    ///
                    ((Shape)MyCan.Children[KeY]).StrokeThickness = newRec.StrokeThickness;
                    Canvas.SetLeft(MyCan.Children[KeY], t.X);
                    Canvas.SetTop(MyCan.Children[KeY], t.Y);
                    MyCan.Children[KeY].RenderTransform = rotateTransform1;


                    ///

                    map_key_to_shape_onLclick[KeY] = activeRec;
                    MessageBox.Show("See anything?");

                }
                else
                {
                    MessageBox.Show("Ummmmm");
                }
                //accum_angle += 45;

                //+ GetGeometry(activeRec).Bounds.Width / 2
                //+ GetGeometry(activeRec).Bounds.Height / 2

                /*
                var topleft_x = Canvas.GetLeft(activeRec);
                if (double.IsNaN(topleft_x))
                    topleft_x = 0D;

                var topleft_y = Canvas.GetTop(activeRec);
                if (double.IsNaN(topleft_y))
                    topleft_y = 0D;

                var transforms = new h();

                //Adding original Translational transform, as only a rotation is to be performed
                var translateTransform = new TranslateTransform(topleft_x, topleft_y);
                transforms.Children.Add(translateTransform);

                RotateTransform rotateTransform1 = new RotateTransform
                {
                    Angle = accum_angle,
                    CenterX = Canvas.GetLeft(activeRec) + (activeRec.ActualWidth/2) , //topleft_x,
                    CenterY = Canvas.GetTop(activeRec) + (activeRec.ActualHeight/2) //topleft_y
                };
                transforms.Children.Add(rotateTransform1);


                //MessageBox.Show(rotateTransform1.CenterX.ToString(), rotateTransform1.CenterY.ToString());
                MessageBox.Show(topleft_x.ToString(), topleft_y.ToString());
                MessageBox.Show(rotateTransform1.CenterX.ToString(), rotateTransform1.CenterY.ToString());

                Rectangle rotRec = new Rectangle
                {
                    Width = activeRec.Width,
                    Height = activeRec.Height,
                    StrokeThickness = activeRec.StrokeThickness,
                    Fill = activeRec.Fill,
                    Stroke = Brushes.Black
                };
                rotRec.RenderTransform = transforms;

                MyCan.Children.Remove(activeRec);
                MyCan.Children.Add(rotRec); */
            }

            else
            {
                Dictionary<int, Shape> map_key_to_shape = new Dictionary<int, Shape>();



                /* Implemented on WB side*/
                int Shape_ID = 42;
                Rectangle newRec_1 = new Rectangle
                {
                    Width = 25,
                    Height = 50,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };
                TranslateTransform lin_tf = new TranslateTransform { X = e.GetPosition(MyCan).X, Y = e.GetPosition(MyCan).Y };
                /* Implemented on WB side*/


                /* Implemented on UX side */
                Canvas.SetLeft(newRec_1, lin_tf.X); // set the left position of rectangle to mouse X
                Canvas.SetTop(newRec_1, lin_tf.Y); // set the top position of rectangle to mouse Y
                map_key_to_shape[Shape_ID] = newRec_1;
                //Addition of a random rectangle
                //Assigning a unique ID to the shape before applying render transforms
                MyCan.Children.Add(newRec_1);
                MessageBox.Show("New Rectangle Added");
                /* Implemented on UX side */



                /* Modification operation on Rectangle given by the WB for Shape ID 42*/
                /* Implemented on WB side*/
                int ShapeID_2 = 42;
                Rectangle newRec_2 = new Rectangle
                {
                    Width = 50,
                    Height = 70,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };
                TranslateTransform lin_tf_2 = new TranslateTransform { X = e.GetPosition(MyCan).X, Y = e.GetPosition(MyCan).Y };

                /* Implemented on WB side*/


                /* Implemented on UX side */
                /*Testing Deletion of Canvas entries with dictionary entry that doesn't depend on RenderTransform*/
                //Trying to delete child with the key
                MyCan.Children.Remove(map_key_to_shape[ShapeID_2]);
                Canvas.SetLeft(newRec_2, lin_tf.X); // set the left position of rectangle to mouse X
                Canvas.SetTop(newRec_2, lin_tf.Y); // set the top position of rectangle to mouse Y
                map_key_to_shape[ShapeID_2] = newRec_2;
                MyCan.Children.Add(newRec_2);
                MessageBox.Show("New resized Rectangle Added with deletion of old one");
                /* Implemented on UX side */



                /* Modification operation on Rectangle given by the WB for Shape ID 42*/
                /* Implemented on WB side*/
                int ShapeID_3 = 42;
                Rectangle newRec_3 = new Rectangle
                {
                    Width = 50,
                    Height = 70,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };
                TranslateTransform lin_tf_3 = new TranslateTransform { X = e.GetPosition(MyCan).X, Y = e.GetPosition(MyCan).Y };
                RotateTransform rot_tf = new RotateTransform { Angle = 90, CenterX = newRec_3.Width / 2, CenterY = newRec_3.Height / 2 };
                /* Implemented on WB side*/

                /* Implemented on UX side */
                //Shape curShape_2 = map_key_to_shape[ShapeID_3];
                //curShape_2.RenderTransform = Transform.Identity;
                //TranslateTransform lintf_UX = new TranslateTransform { X = Canvas.GetLeft(curShape_2), Y = Canvas.GetTop(curShape_2) };
                //Canvas.SetLeft(curShape_2, lintf_UX.X); // set the left position of rectangle to mouse X
                //Canvas.SetTop(curShape_2, lintf_UX.Y); // set the top position of rectangle to mouse Y
                MyCan.Children.Remove(map_key_to_shape[ShapeID_3]);
                Canvas.SetLeft(newRec_3, lin_tf.X); // set the left position of rectangle to mouse X
                Canvas.SetTop(newRec_3, lin_tf.Y); // set the top position of rectangle to mouse Y
                newRec_3.RenderTransform = rot_tf;
                map_key_to_shape[ShapeID_3] = newRec_3;
                
                MyCan.Children.Add(newRec_3);
                MessageBox.Show("Newest Rotated Rectangle added and old Removed?");
                /* Implemented on UX side */



                /* Modification operation on Rectangle given by the WB for Shape ID 42*/
                /* Implemented on WB side*/
                int ShapeID_4 = 42;
                Rectangle newRec_4 = new Rectangle
                {
                    Width = 50,
                    Height = 70,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };
                TranslateTransform lin_tf_4 = new TranslateTransform { X = e.GetPosition(MyCan).X, Y = e.GetPosition(MyCan).Y };
                RotateTransform rot_tf_2 = new RotateTransform { Angle = 80, CenterX = newRec_4.Width / 2, CenterY = newRec_4.Height / 2 };
                /* Implemented on WB side*/

                /* Implemented on UX side */
                //Shape curShape_2 = map_key_to_shape[ShapeID_3];
                //curShape_2.RenderTransform = Transform.Identity;
                //TranslateTransform lintf_UX = new TranslateTransform { X = Canvas.GetLeft(curShape_2), Y = Canvas.GetTop(curShape_2) };
                //Canvas.SetLeft(curShape_2, lintf_UX.X); // set the left position of rectangle to mouse X
                //Canvas.SetTop(curShape_2, lintf_UX.Y); // set the top position of rectangle to mouse Y
                MyCan.Children.Remove(map_key_to_shape[ShapeID_4]);
                Canvas.SetLeft(newRec_4, lin_tf.X); // set the left position of rectangle to mouse X
                Canvas.SetTop(newRec_4, lin_tf.Y); // set the top position of rectangle to mouse Y
                newRec_4.RenderTransform = rot_tf_2;
                map_key_to_shape[ShapeID_4] = newRec_4;

                MyCan.Children.Add(newRec_4);
                MessageBox.Show("Newest Rotated Rectangle added and old Removed yet again?");
                /* Implemented on UX side */



                //LARGE BUG::::
                //If in Canvas.SetLeft(newRec_4, lin_tf.X) or Canvas.SetTop(newRec_4, lin_tf.X), we use 'lin_tf_4' instead of 'lin_tf', the fookin logic just collapses for some reason?!?!?!

            }
        }


        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/d3ca2a1a-2d4f-4fd3-8112-37e99a59d4d2/intersection-method-for-systemwindowsshapesrectangle?forum=wpf
        //https://stackoverflow.com/questions/3685239/iteration-problem-with-canvas-children

        private void momentOfTruth(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                // if the click source is a rectangle then we will create a new rectangle
                // and link it to the rectangle that sent the click event
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle

                Brush Custbrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                (byte)r.Next(1, 255), (byte)r.Next(1, 233)));

                activeRec.Fill = Custbrush;


                //Noting cursor position with respect to canvas
                Point temp = e.GetPosition(MyCan);
                //assigning line equal to the point of cursor position
                Line p = new Line();
                p.X1 = temp.X;
                p.X2 = temp.X;
                p.Y1 = temp.Y;
                p.Y2 = temp.Y;

                MessageBox.Show(Canvas.GetLeft(activeRec).ToString(), Canvas.GetTop(activeRec).ToString());
                //Also moving the object by 10 units
                Canvas.SetLeft(activeRec, Canvas.GetLeft(activeRec) - 10);
                Canvas.SetTop(activeRec, Canvas.GetTop(activeRec) - 10);
                MessageBox.Show(Canvas.GetLeft(activeRec).ToString(), Canvas.GetTop(activeRec).ToString());

                //MyCanvas.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
            }

            // if we clicked on the canvas then we do the following
            else
            {
                // generate a random colour and save it inside the custom brush variable
                Custombrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                (byte)r.Next(1, 255), (byte)r.Next(1, 233)));

                // create a re rectangle and give it the following properties
                // height and width 50 pixels
                // border thickness 3 pixels, fill colour set to the custom brush created above
                // border colour set to black
                Rectangle newRec = new Rectangle
                {
                    Width = 50,
                    Height = 100,
                    StrokeThickness = 3,
                    Fill = Custombrush,
                    Stroke = Brushes.Black
                };
                

                // once the rectangle is set we need to give a X and Y position for the new object
                // we will calculate the mouse click location and add it there

                //ALTERNATE

                //Point topLeft = Mouse.GetPosition(MyCan);

                //Transform place = new TranslateTransform { X = topLeft.X ,Y =topLeft.Y };
                //newRec.RenderTransform = place;
                //MyCan.Children.Add(newRec);

                //ALTERNATE


                //ORIGINAL

                Canvas.SetLeft(newRec, Mouse.GetPosition(MyCan).X); // set the left position of rectangle to mouse X
                Canvas.SetTop(newRec, Mouse.GetPosition(MyCan).Y); // set the top position of rectangle to mouse Y

                map_shape_translate[newRec] = new TranslateTransform { X = Canvas.GetLeft(newRec), Y = Canvas.GetTop(newRec) };
                MyCan.Children.Add(newRec); // add the new rectangle to the canvas
                map_key_to_shape_onRclick[RectIndex] = newRec;
                map_key_to_shape_onLclick[RectIndex] = newRec;
                map_shape_angle[newRec] = 0;
                RectIndex++;
                //ORIGINAL
            }
        }
    }
}
