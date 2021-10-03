using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfApp1_Overlap_check
{

    class WBtoUX
    {
        Shape sh;
        TransformGroup tr;
        int RenType; //to determine additon/deletion/modification of shapes in the canvas
        float time;
        int user_ID;
        int shape_ID;
    }
    
    //[Serializable]
    class WBtofromServer
    {
        char Shape_Name;
        float Shape_topLeft_x;
        float Shape_topLeft_y;
        float Shape_Height;
        float Shape_Width;
        int RenderAngle;
        int ShapeFillR;
        int ShapeFillG;
        int ShapeFillB;
        double ShapeStrokeThickness;
        double Timestamp;
        int UserID;
        int ShapeID;
    }

    class WBModule
    {
        enum WBTools
        {
            Initial, //Initialised value, never to be used again
            Selection,
            NewLine,
            NewRectangle,
            NewEllipse,
            Rotate,
            Move,
            Eraser
            //FreeHand
        };

        //UX sets this enum to different options when user clicks on the appropriate tool icon
        WBTools ActiveTool = WBTools.Initial;

        //Priority Queue maintained with timestamp, this priority queue is accessed by a UX thread continuously, UX keeps popping and rendering the shapes in this priority queue continuously
        PriorityQueue<WBtoUX> WB_to_UXRender_PQ;

        //Shape/ List of shapes currently selected with the 'Select' tool of the Whiteboard
        Shape selectedShape;

        void ShapeOp(float start_X, float start_Y, float end_X, float end_Y, float timestamp, int OpType)
        {
            //use 'ActiveTool' variable to determine whether operation is drawing new shape OR modifying existing shape
            switch(ActiveTool):
                case WBTools.NewEllipse: case WBTools.NewLine: case WBTools.NewRectangle:

                    //Write code to first insert server updates which satisfy (WBtofromServer.Timestamp < timestamp) to ensure that UX always recieves updates in ascending order of time
                    //ASSUMING server updates fetching to have minimal delay

                    //Write code to make 'newShape' with 'drawShape' function

                    //

                    //finally adding currently requested Shape to priority queue for rendering by the UX
                    WB_to_UXRender_PQ.add(new WBtoUX(newShape, new TransformGroup(), RenType=0));  //adding empty transform, as new shapes are in default orientation
                    break;

                case WBTools.Rotate: case WBTools.Move:
                    //Write code to first insert server updates which satisfy (WBtofromServer.Timestamp < timestamp) to ensure that UX always recieves updates in ascending order of time
                    //ASSUMING server updates fetching to have minimal delay

                    //Write code to make 'newTransforms' with 'modifyShape' function

                    //finally adding currently requested Shape to priority queue for rendering by the UX
                    WB_to_UXRender_PQ.add(new WBtoUX(self.selectedShape, newTransforms, RenType = 1));  //adding same selected shape with updated Transform, RenType=1 since a modification operation
                    break;

            //if 
        }

        Shape drawShape(float start_X, float start_Y, float end_X, float end_Y, WBTools ActiveTool)
        {
            if ActiveTool is WBTools.NewLine:
                    //write code to find topLeft_X, topLeft_Y, Shape_Length, Shape_Height

                    //write code to make Shape object with above variables

                    //return newShape

            /*
             * 
             * Handle other shapes
             * 
             * */
        }

        TransformGroup modifyShape(Shape curShape, float start_X, float start_Y, float end_X, float end_Y) 
        {
            if ActiveTool is WBTools.Rotate or ActiveTool is WBTools.Move:
                    //find the angle made by 'start' and 'end' on the center of curShape/

                    //OR creating a new TransformsGroup object 'modifTransform' (which would have different 'TranslateTransform' for Move operation and 'RotateTransform' for Rotate operation)
                    //NOTE: TranslateTransform takes the center point of the shape as reference

                    //EXTRA INFO: UX team will use:
                    //Canvas.SetLeft(curShape, modifTransform[0].XProperty - curShape.Height/2);
                    //Canvas.SetTop(curShape, modifTransform[0].YProperty - curShape.Width/2); to actually render the updated shape after deleting outdated shape

                    //return modifTransform
        }


    }
}
