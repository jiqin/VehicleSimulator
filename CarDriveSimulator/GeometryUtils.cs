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
    }
}
