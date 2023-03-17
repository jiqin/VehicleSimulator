using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator
{
    class GeometryUtils
    {
        public Point OriginalPointInDevice { get; set; }
        public double Scale { get; set; }  // Scale = DevicePoint / LogicPoint

        /***********************************************************************
         * 
         * Logic Point and Device Point convert
         * 
         * 
         * (0, 0) (Device Original) => x
         * ||
         * y
         * 
         * 
         *                                (lgX, lgY) Logical Point
         * 
         *                    y
         *                    ||
         *                    (0, 0) (Logic Original)  => x
         * 
         * 
         * 
         ***********************************************************************/

        public int LogicalToDevice_X(int x)
        {
            return (int)(OriginalPointInDevice.X + x * Scale);
        }

        public int LogicalToDevice_Y(int y)
        {
            return (int)(OriginalPointInDevice.Y - y * Scale);
        }

        public Point LogicalToDevice_Point(Point pt)
        {
            return new Point(LogicalToDevice_X(pt.X), LogicalToDevice_Y(pt.Y));
        }

        public int DeviceToLogical_X(int x)
        {
            return (int)((x - OriginalPointInDevice.X) / Scale);
        }

        public int DeviceToLogical_Y(int y)
        {
            return (int)((-y + OriginalPointInDevice.Y) / Scale);
        }

        public Point DeviceToLogical_Point(Point pt)
        {
            return new Point(DeviceToLogical_X(pt.X), DeviceToLogical_Y(pt.Y));
        }

        /***********************************************************************
         * 
         * Point rotate and move
         * 
         *     .(toPt)
         * 
         *         . (FromPt)   => rotate angle (30) => toPt
         * 
         * . (originPt)
         * 
         ***********************************************************************/

        public static double AngleToRadian(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double RadianToAngle(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public static Point RotatePoint(Point originPt, Point fromPt, double angle)
        {
            var radian = AngleToRadian(angle);
            var x0 = (fromPt.X - originPt.X) * Math.Cos(radian) - (fromPt.Y - originPt.Y) * Math.Sin(radian) + originPt.X;
            var y0 = (fromPt.X - originPt.X) * Math.Sin(radian) + (fromPt.Y - originPt.Y) * Math.Cos(radian) + originPt.Y;
            return new Point((int)x0, (int)y0);
        }

        public double GetDistance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));
        }


        /*
         *   3       0
         *   2       1
         */
        public static Point[] GenerateRectanglePoints(int xLength, int yLength)
        {
            return new Point[] {
                new Point( xLength / 2,  yLength / 2),
                new Point( xLength / 2, -yLength / 2),
                new Point(-xLength / 2, -yLength / 2),
                new Point(-xLength / 2,  yLength / 2),
            };
        }


        public void DrawLogicLine(Graphics g, Pen pen, Point pt1, Point pt2)
        {
            g.DrawLine(pen, LogicalToDevice_Point(pt1), LogicalToDevice_Point(pt2));
        }

        public void DrawLogicCircle(Graphics g, Pen pen, Point center, Point circumFerence)
        {
            var radius = GetDistance(center, circumFerence);
            DrawLogicCircle(g, pen, center, radius);
        }

        public void DrawLogicCircle(Graphics g, Pen pen, Point center, double radius)
        {
            var x = LogicalToDevice_X((int)(center.X - radius));
            var y = LogicalToDevice_Y((int)(center.Y + radius));
            var deviceRadius = 2 * radius * Scale;
            g.DrawEllipse(pen, new Rectangle(x, y, (int)deviceRadius, (int)deviceRadius));
        }

        public void DrawLogicLines(Graphics g, Pen pen, Point[] points, bool enclose = true)
        {
            for (var i = 0; i < points.Length - 1; ++i)
            {
                DrawLogicLine(g, pen, points[i], points[i + 1]);
            }

            if (enclose)
            {
                DrawLogicLine(g, pen, points[points.Length - 1], points[0]);
            }
        }


        public bool IsPointsInTheSameSideOfLine(Point pt1, Point pt2, Point[] line)
        {
            // 要判断两个点是否在某条线段的同一侧可以通过叉积来实现
            Func<Point, Point[], int> crossProduct = (pt, line) =>
            {
                return (pt.X - line[0].X) * (line[1].Y - line[0].Y) - (pt.Y - line[0].Y) * (line[1].X - line[0].X);
            };

            var v1 = crossProduct(pt1, line);
            var v2 = crossProduct(pt2, line);
            return (v1 > 0 && v2 > 0) || (v1 < 0 && v2 < 0);
        }

        public bool IsPointInTriangle(Point pt, Point[] triangle)
        {
            // 同侧法
            // 如果pt在三角形ABC内部，则满足以下三个条件：P,A在BC的同侧、P,B在AC的同侧、PC在AB的同侧
            return IsPointsInTheSameSideOfLine(pt, triangle[0], new Point[] { triangle[1], triangle[2] })
                && IsPointsInTheSameSideOfLine(pt, triangle[1], new Point[] { triangle[2], triangle[0] })
                && IsPointsInTheSameSideOfLine(pt, triangle[2], new Point[] { triangle[0], triangle[1] });
        }

        public bool IsPointInPolygon(Point pt, Point[] polygon)
        {
            for (var i = 1; i < polygon.Length - 1; ++i)
            {
                if (IsPointInTriangle(pt, new Point[] {polygon[0], polygon[i], polygon[i+1]}))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
