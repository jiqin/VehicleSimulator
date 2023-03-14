using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator
{
    class ParkingModelExtension
    {
        public ParkingModel Model { get; }
        public GeometryUtils GeometryUtils;

        public ParkingModelExtension(ParkingModel model, GeometryUtils geometryUtils)
        {
            Model = model;
            GeometryUtils = geometryUtils;
        }

        public double Radian => GeometryUtils.AngleToRadian(Model.Angle);

        public Point[] EdgeRelativePoints
        {
            get
            {
                return new Point[] {
                    new Point(0, 0),
                    new Point(Model.Cube_Number * Model.Cube_L, 0),
                    new Point(Model.Cube_Number * Model.Cube_L, - Model.Cube_W),
                    new Point(0, - Model.Cube_W),
                };
            }
        }

        public Point RelativePointToLogic(Point pt)
        {
            pt = GeometryUtils.RotatePoint(new Point(0, 0), pt, Model.Angle);
            pt = pt + new Size(Model.Position);
            return pt;
        }

        public void Rotate(int angleDelta)
        {
            Model.Angle += angleDelta;
        }

        public void Draw(Graphics g)
        {
            var pen = new Pen(Model.Pen.Color, Model.Pen.Width);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            GeometryUtils.DrawLogicLines(g, pen, EdgeRelativePoints.Select(pt => RelativePointToLogic(pt)).ToArray());

            for (var i = 1; i < Model.Cube_Number; ++i)
            {
                var position = new Point(i * Model.Cube_L, 0);
                var pts = new Point[]
                {
                    position,
                    position + new Size(0, -Model.Cube_W),
                };

                GeometryUtils.DrawLogicLines(g, pen, pts.Select(pt => RelativePointToLogic(pt)).ToArray(), false);
            }
        }

        public bool IsSelected(Point logicPt)
        {
            return false;
        }
    }
}
