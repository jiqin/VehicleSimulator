using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator.Models
{
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

        public PenModel Pen = new PenModel(Color.Black, 3);
    }

    class ParkingView
    {
        private ParkingModel Model { get; }
        private GeometryUtils GeometryUtils;

        public ParkingView(ParkingModel model, GeometryUtils geometryUtils)
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
            return GeometryUtils.IsPointInPolygon(logicPt, EdgeRelativePoints.Select(p => RelativePointToLogic(p)).ToArray());
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                Model.Pen = new PenModel(Color.DarkGreen, 8);
            }
            else
            {
                Model.Pen = new PenModel(Color.Black, 3);
            }
        }

        public void Move(Size logicSize)
        {
            Model.Position += logicSize;
        }
    }
}
