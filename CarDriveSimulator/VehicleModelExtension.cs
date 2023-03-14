﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator
{
    class VehicleModelExtension
    {
        public VehicleModel Model { get; }
        public GeometryUtils GeometryUtils;

        public VehicleModelExtension(VehicleModel model, GeometryUtils geometryUtils)
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

        public void Rotate(int vehicleAngleDelta, int wheelAngleDelta)
        {
            Model.VehicleAngle += vehicleAngleDelta;

            Model.WheelAngle += wheelAngleDelta;
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
                var pen = new Pen(Model.PenBody.Color, Model.PenBody.Width);

                var position = RelativePointToLogic(Model.DriverPosition);
                GeometryUtils.DrawLogicCircle(g, pen, position, Model.DriverSize / 2);
            }

            // Draw Front Back Extionsion Line
            {
                if (Model.FrontBackExtionsionLine_Draw)
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
                if (Model.TurningRadius_Draw && Model.WheelAngle != 0)
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
                if (Model.WheelAngle != 0)
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
        }

        public bool IsSelected(Point logicPt)
        {
            return false;
        }
    }
}
