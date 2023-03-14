using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CarDriveSimulator
{

    public class PenModel
    {
        public Color Color;
        public int Width;

        public PenModel(Color color, int width = 1)
        {
            Color = color;
            Width = width;
        }
    }

    class AxisModel
    {
        public bool Display = true;
        public int Spacing = 100;
        public PenModel PenModel = new PenModel(Color.Black);
    }

    class ScenarioModel
    {
        public AxisModel Axis1 = new AxisModel { Display = false, Spacing = 100, PenModel = new PenModel(Color.LightGreen, 1) };
        public AxisModel Axis2 = new AxisModel { Display = true, Spacing = 1000, PenModel = new PenModel(Color.LightGray, 2) };

        public Point OriginalPointInDevice = new Point(0, 0);
        public double Scale = 0.1; // DevicePoint = Scale * LogicPoint 

        public List<VehicleModel> VehicleModels = new List<VehicleModel>();

        public List<ParkingModel> ParkingModels = new List<ParkingModel>();

        public static string SerializeToJson(ScenarioModel o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public static ScenarioModel DeserializeFromJson(string v)
        {
            return JsonConvert.DeserializeObject<ScenarioModel>(v);
        }
    }


    /*
     * 
     * () <- Position
     * ==================================================
     * ||              ||      ^       ||              ||
     * ||              ||      |       ||              ||
     * ||              ||    Cube_W    ||              ||
     * ||              ||      |       ||              ||
     * ||              ||      v       ||              ||
     * ||              ||<-  Cube_L  ->||              ||
     * ==================================================
     * 
     */
    class ParkingModel
    {
        public int Cube_L = 6000; // Or 2500
        public int Cube_W = 2500; // Or 5000
        public int Cube_Number = 10;

        public Point Position = new Point(0, 0);
        public double Angle = 90;
        public double Radian => GeometryUtils.AngleToRadian(Angle);

        public PenModel Pen = new PenModel(Color.Black, 3);

        public Point RelativePointToLogic(Point pt)
        {
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, Angle);
            pt = pt + new Size(Position);
            return pt;
        }
    }

    /*
     *    /
     *   /
     *  /   Angle = 60  (or Radian = Angle / 180 * Pi)
     * ---------------------------
     * 
     * |<-                            Dimension_L                                 ->|
     * |<-   Back           ->|<-      Wheel Base                  ->|<-  Front   ->|
     *       Overhand                                                     Overhand
     *                                                                   
     *                                                   || Rearview mirrow
     * |----------------------------------------------------------------------------|
     * |                    ===== 3                                =====  0         |
     * |                    =====                                  =====            |
     * |                                         () Driver Position                 |
     * |                     ^
     * |                    Wheel        (0,0) <- Vehicle Coordinate Origin         (Vehicle Head)
     * |                    Track              Logic Position
     * |                     v
     * |                    ===== 2                                ===== 1          |
     * |                    =====                                  =====            |
     * |----------------------------------------------------------------------------|
     *                                                   || Rearview mirrow
     */
    class VehicleModel
    {
        public string Name = "My Car 1";
        public string Model = "ModelY";

        // Fixed dimensions
        public int Dimension_L = 4750;
        public int Dimension_W = 1921;
        public int Dimension_H = 1624;
        public int FrontOverhang = 950;
        public int BackOverhang { get { return Dimension_L - FrontOverhang - WheelBase; } }

        public int WheelBase = 2890;
        public int WheelTrack = 1636;
        public int Wheel_L = 800;
        public int Wheel_W = 194;

        public int RearviewMirrow_Position_X = 800;
        public int RearviewMirrow_L = 100;
        public int RearviewMirrow_W = 200;

        public Point DriverPosition = new Point(300, 500);
        public int DriverSize = 300;

        public Point[] BodyRelativePoints => GeometryUtils.GenerateRectanglePoints(Dimension_L, Dimension_W);

        public Point[] WheelRelativePositions
        {
            get
            {
                return new Point[] {
                    new Point(   Dimension_L / 2 - FrontOverhang,   WheelTrack / 2),
                    new Point(   Dimension_L / 2 - FrontOverhang, - WheelTrack / 2),
                    new Point(- (Dimension_L / 2 - BackOverhang), - WheelTrack / 2),
                    new Point(- (Dimension_L / 2 - BackOverhang),   WheelTrack / 2),
                };
            }
        }

        public Point[] RearviewMirrowRelativePositions
        {
            get
            {
                return new Point[] {
                    new Point(RearviewMirrow_Position_X,   (Dimension_W + RearviewMirrow_W) / 2),
                    new Point(RearviewMirrow_Position_X, - (Dimension_W + RearviewMirrow_W) / 2),
                };
            }
        }

        // Position and Angle
        public Point Position = new Point(0, 0);
        public double VehicleAngle = 90;    // Default towards to Up.
        public double WheelAngle = 0;

        public double VehicleRadian => GeometryUtils.AngleToRadian(VehicleAngle);
        public double WheelRadian => GeometryUtils.AngleToRadian(WheelAngle);

        // Draw Vehicle
        public PenModel PenBody = new PenModel(Color.Black, 5);
        public PenModel PenWheel = new PenModel(Color.Red, 3);

        // Draw extention
        public bool FrontBackExtionsionLine_Draw = false;
        public int FrontBackExtionsionLine_Length = 10000;
        public PenModel FrontBackExtionsionLine_Pen = new PenModel(Color.Yellow, 5);

        public bool TurningRadius_Draw = false;
        public PenModel TurningRadius_Pen = new PenModel(Color.Yellow, 5);
        public Point TurningRadius_RelativePoint = new Point(0, 0);

        public bool GuideLine_Body_Draw = false;
        public PenModel GuideLine_Body_Pen = new PenModel(Color.Green, 2);

        public bool GuideLine_Wheel_Draw = false;
        public PenModel GuideLine_Wheel_Pen = new PenModel(Color.Red, 2);

        public VehicleModel()
        {
            TurningRadius_RelativePoint = new Point(-(Dimension_L / 2 - BackOverhang), int.MaxValue / 2);
        }

        public void Rotate(int vehicleAngleDelta, int wheelAngleDelta)
        {
            VehicleAngle += vehicleAngleDelta;

            WheelAngle += wheelAngleDelta;
            if (WheelAngle < -30)
            {
                WheelAngle = -30;
            }
            if (WheelAngle > 30)
            {
                WheelAngle = 30;
            }

            /*
             *      ( ) Turning Radius Point
             *       ^
             *       |
             *      TurningRadius
             *       |
             *       v
             *     Wheel3       WheelBase       Wheel0  (WheelAngle > 0) Tan(WheelAngle) = WheelBase / TurningRadius
             *     
             *     
             *     Wheel2                       Wheel1
             */

            if (Math.Abs(WheelAngle) <= 0.01)
            {
                TurningRadius_RelativePoint.Y = int.MaxValue / 2;
            }
            else
            {
                var TurningRadius = WheelBase / Math.Tan(WheelRadian);
                TurningRadius_RelativePoint.Y = (int)(TurningRadius + WheelTrack / 2 * (WheelAngle > 0 ? 1 : -1));
            }
        }

        public Point RelativePointToLogic(Point pt)
        {
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, VehicleAngle);
            pt = pt + new Size(Position);
            return pt;
        }

        public Point RelativeWheelPointToLogic(Point wheelPosition, Point pt)
        {
            pt = GeometryUtils.RotatePoint(wheelPosition, pt, WheelAngle);
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, VehicleAngle);
            pt = pt + new Size(Position);
            return pt;
        }
    }
}
