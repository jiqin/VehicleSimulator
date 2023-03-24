using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator.Models
{
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

        // Position and Angle
        public Point Position = new Point(0, 0);
        public double VehicleAngle = 90;    // Default towards to Up.
        public double WheelAngle = 0;
        public double MaxWheelAngle = 37;

        // Draw Vehicle
        public PenModel PenBody = new PenModel(Color.Black, 5);
        public PenModel PenWheel = new PenModel(Color.Red, 3);

        // Draw extention
        public bool FrontBackExtionsionLine_Draw = false;
        public int FrontBackExtionsionLine_Length = 10000;
        public PenModel FrontBackExtionsionLine_Pen = new PenModel(Color.Yellow, 5);

        public bool TurningRadius_Draw = false;
        public PenModel TurningRadius_Pen = new PenModel(Color.Yellow, 5);

        public bool GuideLine_Body_Draw = false;
        public PenModel GuideLine_Body_Pen = new PenModel(Color.Green, 2);

        public bool GuideLine_Wheel_Draw = false;
        public PenModel GuideLine_Wheel_Pen = new PenModel(Color.Red, 2);
    }

    class VehicleView
    {
        private VehicleModel Model { get; }
        public GeometryUtils GeometryUtils;

        public bool IsDriveMode { get; private set; } = false;

        public VehicleView(VehicleModel model, GeometryUtils geometryUtils)
        {
            Model = model;
            GeometryUtils = geometryUtils;

            TurningRadius_RelativePoint = new Point(-(Model.Dimension_L / 2 - BackOverhang), int.MaxValue / 2);
        }

        public int BackOverhang => Model.Dimension_L - Model.FrontOverhang - Model.WheelBase;

        public Point[] BodyRelativePoints => GeometryUtils.GenerateRectanglePoints(Model.Dimension_L, Model.Dimension_W);

        public Point[] WheelRelativePositions
        {
            get
            {
                return new Point[] {
                    new Point(   Model.Dimension_L / 2 - Model.FrontOverhang,   Model.WheelTrack / 2),
                    new Point(   Model.Dimension_L / 2 - Model.FrontOverhang, - Model.WheelTrack / 2),
                    new Point(- (Model.Dimension_L / 2 - BackOverhang),       - Model.WheelTrack / 2),
                    new Point(- (Model.Dimension_L / 2 - BackOverhang),         Model.WheelTrack / 2),
                };
            }
        }

        public Point[] RearviewMirrowRelativePositions
        {
            get
            {
                return new Point[] {
                    new Point(Model.RearviewMirrow_Position_X,   (Model.Dimension_W + Model.RearviewMirrow_W) / 2),
                    new Point(Model.RearviewMirrow_Position_X, - (Model.Dimension_W + Model.RearviewMirrow_W) / 2),
                };
            }
        }

        public double VehicleRadian => GeometryUtils.AngleToRadian(Model.VehicleAngle);
        public double WheelRadian => GeometryUtils.AngleToRadian(Model.WheelAngle);

        public Point TurningRadius_RelativePoint;

        public void Rotate(int angleDelta)
        {
            if (!IsDriveMode)
            {
                RotateBody(angleDelta);
            }
            else
            {
                RotateWheel(angleDelta);
            }
        }

        public void RotateBody(int angleDelta)
        {
            Model.VehicleAngle += angleDelta;
        }

        public void RotateWheel(int angleDelta)
        {
            Model.WheelAngle += angleDelta;
            if (Model.WheelAngle < -Model.MaxWheelAngle)
            {
                Model.WheelAngle = -Model.MaxWheelAngle;
            }
            if (Model.WheelAngle > Model.MaxWheelAngle)
            {
                Model.WheelAngle = Model.MaxWheelAngle;
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

            if (Math.Abs(Model.WheelAngle) <= 0.01)
            {
                TurningRadius_RelativePoint.Y = int.MaxValue / 2;
            }
            else
            {
                var TurningRadius = Model.WheelBase / Math.Tan(WheelRadian);
                TurningRadius_RelativePoint.Y = (int)(TurningRadius + Model.WheelTrack / 2 * (Model.WheelAngle > 0 ? 1 : -1));
            }
        }

        public Point RelativePointToLogic(Point pt)
        {
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, Model.VehicleAngle);
            pt = pt + new Size(Model.Position);
            return pt;
        }

        public Point RelativeWheelPointToLogic(Point wheelPosition, Point pt)
        {
            pt = GeometryUtils.RotatePoint(wheelPosition, pt, Model.WheelAngle);
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, Model.VehicleAngle);
            pt = pt + new Size(Model.Position);
            return pt;
        }

        public void Draw(Graphics g)
        {
            // Draw body
            {
                var pen = new Pen(Model.PenBody.Color, Model.PenBody.Width);
                var body = BodyRelativePoints;
                GeometryUtils.DrawLogicLines(g, pen, body.Select(pt => RelativePointToLogic(pt)).ToArray());
            }

            // Draw wheel
            {
                var pen = new Pen(Model.PenWheel.Color, Model.PenWheel.Width);

                var wheelPositions = WheelRelativePositions;
                var wheelAngles = new double[] { Model.WheelAngle, Model.WheelAngle, 0, 0 }; // Only front wheels have angle.

                for (var i = 0; i < wheelPositions.Length; ++i)
                {
                    var wheel = GeometryUtils.GenerateRectanglePoints(Model.Wheel_L, Model.Wheel_W);
                    wheel = wheel.Select(pt => GeometryUtils.RotatePoint(new Point(0, 0), pt, wheelAngles[i])).ToArray();  // Rotate from Wheel Position
                    wheel = wheel.Select(pt => pt + new Size(wheelPositions[i])).ToArray();  // Move Wheel Posistion
                    GeometryUtils.DrawLogicLines(g, pen, wheel.Select(pt => RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Rearview Mirrow
            {
                var pen = new Pen(Model.PenBody.Color, Model.PenBody.Width);

                var positions = RearviewMirrowRelativePositions;
                for (var i = 0; i < positions.Length; ++i)
                {
                    var mirrow = GeometryUtils.GenerateRectanglePoints(Model.RearviewMirrow_L, Model.RearviewMirrow_W);
                    mirrow = mirrow.Select(pt => pt + new Size(positions[i])).ToArray();  // Move Mirrow Posistion
                    GeometryUtils.DrawLogicLines(g, pen, mirrow.Select(pt => RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Driver
            {
                if (IsDriveMode)
                {
                    var pen = new Pen(Model.PenBody.Color, Model.PenBody.Width);

                    var position = RelativePointToLogic(Model.DriverPosition);
                    GeometryUtils.DrawLogicCircle(g, pen, position, Model.DriverSize / 2);
                }
            }

            // Draw Front Back Extionsion Line
            {
                if (IsDriveMode && Model.FrontBackExtionsionLine_Draw)
                {
                    var pen = new Pen(Model.FrontBackExtionsionLine_Pen.Color, Model.FrontBackExtionsionLine_Pen.Width);
                    var body = BodyRelativePoints;
                    var extendDirection = new int[] { 1, 1, -1, -1 };
                    for (var i = 0; i < body.Length; ++i)
                    {
                        var pts = new Point[]
                        {
                            body[i],
                            body[i] + new Size(Model.FrontBackExtionsionLine_Length * extendDirection[i], 0),
                        };
                        GeometryUtils.DrawLogicLines(g, pen, pts.Select(pt => RelativePointToLogic(pt)).ToArray());
                    }
                }
            }

            // Draw turning radius
            {
                if (IsDriveMode && Model.TurningRadius_Draw && Model.WheelAngle != 0)
                {
                    var pen = new Pen(Model.TurningRadius_Pen.Color, Model.TurningRadius_Pen.Width);

                    var wheelPositions = WheelRelativePositions;
                    var wheelPointsToDraw = new Point[2];
                    if (Model.WheelAngle > 0)
                    {
                        wheelPointsToDraw[0] = wheelPositions[0];
                        wheelPointsToDraw[1] = wheelPositions[3];
                    }
                    else
                    {
                        wheelPointsToDraw[0] = wheelPositions[1];
                        wheelPointsToDraw[1] = wheelPositions[2];
                    }

                    var pts = new Point[] { wheelPointsToDraw[0], TurningRadius_RelativePoint, wheelPointsToDraw[1] };
                    GeometryUtils.DrawLogicLines(g, pen, pts.Select(pt => RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Guid lines
            {
                if (IsDriveMode && Model.WheelAngle != 0)
                {
                    if (Model.GuideLine_Body_Draw)
                    {
                        var pen = new Pen(Model.GuideLine_Body_Pen.Color, Model.GuideLine_Body_Pen.Width);

                        var body = BodyRelativePoints;
                        foreach (Point pt in body)
                        {
                            GeometryUtils.DrawLogicCircle(g, pen, RelativePointToLogic(TurningRadius_RelativePoint), RelativePointToLogic(pt));
                        }
                    }

                    if (Model.GuideLine_Wheel_Draw)
                    {
                        var pen = new Pen(Model.GuideLine_Wheel_Pen.Color, Model.GuideLine_Wheel_Pen.Width);

                        var wheelPositions = WheelRelativePositions;
                        foreach (Point pt in wheelPositions)
                        {
                            GeometryUtils.DrawLogicCircle(g, pen, RelativePointToLogic(TurningRadius_RelativePoint), RelativePointToLogic(pt));
                        }
                    }
                }
            }

            // Draw Vehicle Axis
            {
                var pen = new Pen(Model.PenBody.Color, Model.PenBody.Width / 2);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                GeometryUtils.DrawLogicLines(g, pen, new[] { new Point(-Model.Dimension_L / 2, 0), new Point(Model.Dimension_L / 2, 0) }.Select(pt => RelativePointToLogic(pt)).ToArray());
                GeometryUtils.DrawLogicLines(g, pen, new[] { new Point(0, -Model.Dimension_W / 2), new Point(0, Model.Dimension_W / 2) }.Select(pt => RelativePointToLogic(pt)).ToArray());
            }
        }

        public bool IsSelected(Point logicPt)
        {
            return GeometryUtils.IsPointInPolygon(logicPt, BodyRelativePoints.Select(p => RelativePointToLogic(p)).ToArray());
        }

        public void SetSelected(bool selected, bool isDrive)
        {
            if (selected)
            {
                IsDriveMode = isDrive;
                Model.PenBody = new PenModel(Color.DarkGreen, 8);
                if (IsDriveMode)
                {
                    Model.FrontBackExtionsionLine_Draw = true;
                }
            }
            else
            {
                IsDriveMode = false;
                Model.PenBody = new PenModel(Color.Black, 5);
                Model.FrontBackExtionsionLine_Draw = false;
            }
        }

        public void Move(Size logicSize)
        {
            if (IsDriveMode)
            {
                var logicDistance = GeometryUtils.GetDistance(new Point(0, 0), new Point(logicSize.Width, logicSize.Height));
                // 判断2个向量夹角
                // cos(ang) = ((x1 * x2) + (y1 * y2))
                var moveForward = logicSize.Width * Math.Cos(VehicleRadian) + logicSize.Height * Math.Sin(VehicleRadian) > 0;
                Drive(logicDistance, moveForward);
            }
            else
            {
                Model.Position += logicSize;
            }
        }

        public void Drive(double logicDistance, bool moveForward)
        {
            if (Model.WheelAngle == 0)
            {
                var xDelta = logicDistance * Math.Cos(VehicleRadian) * (moveForward ? 1 : -1);
                var yDelta = logicDistance * Math.Sin(VehicleRadian) * (moveForward ? 1 : -1);

                Model.Position += new Size((int)(xDelta), (int)(yDelta));
            }
            else
            {
                var or = TurningRadius_RelativePoint;
                var turningRadius = GeometryUtils.GetDistance(or, new Point(0, 0));
                var angleDelta = GeometryUtils.RadianToAngle(Math.Asin(logicDistance / turningRadius));
                if ((Model.WheelAngle > 0 && !moveForward)        // Turn left and move back,
                    || (Model.WheelAngle < 0 && moveForward))     // Turn right and move forward
                {
                    angleDelta = -angleDelta;
                }

                Model.Position = GeometryUtils.RotatePoint(RelativePointToLogic(or), Model.Position, angleDelta);
                Model.VehicleAngle += angleDelta;
            }
        }

        public void SetDisplayGuideLineBody(bool display)
        {
            this.Model.GuideLine_Body_Draw = display;
        }

        public void SetDisplayGuideLineWheel(bool display)
        {
            this.Model.GuideLine_Wheel_Draw = display;
        }
    }
}
