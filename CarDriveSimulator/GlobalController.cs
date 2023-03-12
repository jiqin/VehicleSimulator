using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace CarDriveSimulator
{
    class GlobalController
    {
        public enum EditMode
        {
            Edit = 0,
            Simulate = 1,
        }
        public EditMode CurrentEditMode { get; private set; }

        private Bitmap cachedBitmap = new Bitmap(1, 1);
        private Graphics cachedG;

        public enum MouseMode
        {
            None = 0,
            MoveScreen,
            MoveSelectComponent,
        }

        public MouseMode CurrentMouseMode { get; set; }

        public ScenarioModel ScenarioModel;

        private string filePathName;

        public GlobalController()
        {
            this.ScenarioModel = new ScenarioModel();

            Reset();
        }

        public void LoadFromFile(string filePathName)
        {
            UpdateScenarios(File.ReadAllText(filePathName));
            Reset();
            this.filePathName = filePathName;
        }

        public string GetScenarioText()
        {
            return ScenarioModel.SerializeToJson(this.ScenarioModel);
        }

        public void UpdateScenarios(string value)
        {
            this.ScenarioModel = ScenarioModel.DeserializeFromJson(value);
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(filePathName))
            {
                return false;
            }
            File.WriteAllText(filePathName, GetScenarioText());
            return true;
        }

        public void Save(string filePathName)
        {
            File.WriteAllText(filePathName, GetScenarioText());
            this.filePathName = filePathName;
        }

        public void Reset()
        {
            CurrentEditMode = EditMode.Edit;
            CurrentMouseMode = MouseMode.None;

            cachedBitmap = new Bitmap(1, 1);
            cachedG = Graphics.FromImage(cachedBitmap);
        }

        public EditMode UpdateEditMode()
        {
            if (CurrentEditMode == EditMode.Edit)
            {
                CurrentEditMode = EditMode.Simulate;
            }
            else
            {
                CurrentEditMode = EditMode.Edit;
            }
            return CurrentEditMode;
        }

        public void MoveOriginalPoint(Size devicePt)
        {
            System.Diagnostics.Trace.WriteLine($"MoveOriginalPoint {CurrentMouseMode}, {devicePt}");
            if (CurrentMouseMode == MouseMode.MoveScreen)
            {
                ScenarioModel.OriginalPointInDevicePoint = ScenarioModel.OriginalPointInDevicePoint + devicePt;
            }
        }

        public void ScaleFromPoint(Point scaleAtDevicePt, bool increase)
        {
            // scaleAtDevicePt    ==>   Original
            // Original += scaleAtDevicePt + (Original - scaleAtDevicePt) * (0.9 or 1.1)
            var deltaScale = increase ? 1.1 : 1 / 1.1;
            var sizeX = (ScenarioModel.OriginalPointInDevicePoint.X - scaleAtDevicePt.X) * deltaScale;
            var sizeY = (ScenarioModel.OriginalPointInDevicePoint.Y - scaleAtDevicePt.Y) * deltaScale;
            ScenarioModel.OriginalPointInDevicePoint = scaleAtDevicePt + (new Size((int)sizeX, (int)sizeY));
            ScenarioModel.Scale *= deltaScale;
        }

        public Bitmap Draw(int deviceWidth, int deviceHeight)
        {
            var logicX1 = DeviceToLogical_X(0);
            var logicX2 = DeviceToLogical_X(deviceWidth);
            var logicY1 = DeviceToLogical_Y(deviceHeight);
            var logicY2 = DeviceToLogical_Y(0);

            if (cachedBitmap.Width < deviceWidth || cachedBitmap.Height < deviceHeight)
            {
                cachedBitmap = new Bitmap(deviceWidth, deviceHeight);
                cachedG = Graphics.FromImage(cachedBitmap);
            }

            var g = cachedG;
            //using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, cachedBitmap.Width, cachedBitmap.Height));

                DrawAxis(g, ScenarioModel.Axis1, logicX1, logicX2, logicY1, logicY2);
                DrawAxis(g, ScenarioModel.Axis2, logicX1, logicX2, logicY1, logicY2);

                foreach (var m in ScenarioModel.VehicleModels)
                {
                    DrawVehicle(g, m);
                }
            }

            return cachedBitmap;
        }

        private void DrawAxis(Graphics g, AxisModel axisModel, int logicX1, int logicX2, int logicY1, int logicY2)
        {
            if (!axisModel.Display)
            {
                return;
            }
            var pen = new Pen(axisModel.PenModel.Color, axisModel.PenModel.Width);

            // Draw axis
            for (var x = logicX1 / axisModel.Spacing * axisModel.Spacing; x < logicX2; x += axisModel.Spacing)
            {
                DrawLogicLine(g, pen, new Point(x, logicY1), new Point(x, logicY2));
            }
            for (var y = logicY1 / axisModel.Spacing * axisModel.Spacing; y < logicY2; y += axisModel.Spacing)
            {
                DrawLogicLine(g, pen, new Point(logicX1, y), new Point(logicX2, y));
            }
        }

        public void DrawVehicle(Graphics g, VehicleModel m)
        {
            // Draw body
            {
                var pen = new Pen(m.PenBody.Color, m.PenBody.Width);
                var body = GenerateRectanglePoints(m.Dimension_W, m.Dimension_L);
                body = RotateRectangle(body, m.VehicleAngle);
                body = MoveRotate(body, m.Position);
                DrawLogicLines(g, pen, body);
            }

            // Draw wheel
            {
                var pen = new Pen(m.PenWheel.Color, m.PenWheel.Width);
                var wheelPositions = new Point[] {
                    new Point(-m.WheelTrack / 2,  m.Dimension_L / 2 - m.FrontOverhang),
                    new Point( m.WheelTrack / 2,  m.Dimension_L / 2 - m.FrontOverhang),
                    new Point( m.WheelTrack / 2, -(m.WheelBase - (m.Dimension_L / 2 - m.FrontOverhang))),
                    new Point(-m.WheelTrack / 2, -(m.WheelBase - (m.Dimension_L / 2 - m.FrontOverhang)))};

                var wheelAngles = new int[] { m.WheelAngle, m.WheelAngle, 0, 0 }; // Only front wheels have angle.

                for (var i = 0; i < wheelPositions.Length; ++i)
                {
                    var pos = wheelPositions[i];
                    var angle = wheelAngles[i];
                    var wheel = GenerateRectanglePoints(m.Wheel_W, m.Wheel_L);
                    wheel = RotateRectangle(wheel, angle);
                    wheel = MoveRotate(wheel, pos);
                    wheel = RotateRectangle(wheel, m.VehicleAngle);
                    wheel = MoveRotate(wheel, m.Position);
                    DrawLogicLines(g, pen, wheel);
                }
            }
        }

        public void RotateActiveVehicle(int vehicleAngleDelta, int wheelAngleDelta)
        {
            foreach (var m in ScenarioModel.VehicleModels)
            {
                RotateVehicle(m, vehicleAngleDelta, wheelAngleDelta);
            }
        }

        public void RotateVehicle(VehicleModel m, int vehicleAngleDelta, int wheelAngleDelta)
        {
            m.Rotate(vehicleAngleDelta, wheelAngleDelta);
        }

        public Point[] GenerateRectanglePoints(int xLength, int yLength)
        {
            return new Point[] {
                new Point(-xLength / 2,  yLength / 2),
                new Point( xLength / 2,  yLength / 2),
                new Point( xLength / 2, -yLength / 2),
                new Point(-xLength / 2, -yLength / 2),
                new Point(-xLength / 2,  yLength / 2),};
        }

        /*
         * rect => rotate angle
         * 
         *      . (new top-left)
         * top-left  
         * |---------
         * |
         * |
         * |    . (center point)
         * |
         * |
         * |---------
         * 
         */
        public Point[] RotateRectangle(Point[] points, int angle)
        {
            return points.Select(p => RotatePoint(new Point(0, 0), p, angle)).ToArray();
        }

        public Point[] MoveRotate(Point[] points, int moveX, int moveY)
        {
            return points.Select(p => p + new Size(moveX, moveY)).ToArray();
        }

        public Point[] MoveRotate(Point[] points, Point move)
        {
            return points.Select(p => p + new Size(move)).ToArray();
        }

        /*
         *     .(pt0)
         * 
         *         . (pt)   => rotate angle (-30) => pt0
         * 
         * . (r)
         * 
         */
        public Point RotatePoint(Point r, Point pt, int angle)
        {
            var ang = -angle * Math.PI / 180;
            var x0 = (pt.X - r.X) * Math.Cos(ang) - (pt.Y - r.Y) * Math.Sin(ang) + r.X;
            var y0 = (pt.X - r.X) * Math.Sin(ang) + (pt.Y - r.Y) * Math.Cos(ang) + r.Y;
            return new Point((int)x0, (int)y0);
        }

        public void DrawLogicLines(Graphics g, Pen pen, Point[] points)
        {
            for (var i = 0; i < points.Length - 1; ++i)
            {
                DrawLogicLine(g, pen, points[i], points[i + 1]);
            }
        }

        public void DrawLogicLine(Graphics g, Pen pen, Point pt1, Point pt2)
        {
            g.DrawLine(pen, LogicalToDevice_Point(pt1), LogicalToDevice_Point(pt2));
        }


        /*
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
         */

        public int LogicalToDevice_X(int x)
        {
            return (int)(ScenarioModel.OriginalPointInDevicePoint.X + x * ScenarioModel.Scale);
        }

        public int LogicalToDevice_Y(int y)
        {
            return (int)(ScenarioModel.OriginalPointInDevicePoint.Y - y * ScenarioModel.Scale);
        }

        public Point LogicalToDevice_Point(Point pt)
        {
            return new Point(LogicalToDevice_X(pt.X), LogicalToDevice_Y(pt.Y));
        }

        public int DeviceToLogical_X(int x)
        {
            return (int)((x - ScenarioModel.OriginalPointInDevicePoint.X) / ScenarioModel.Scale);
        }

        public int DeviceToLogical_Y(int y)
        {
            return (int)((-y + ScenarioModel.OriginalPointInDevicePoint.Y) / ScenarioModel.Scale);
        }

        public Point DeviceToLogical_Point(Point pt)
        {
            return new Point(DeviceToLogical_X(pt.X), DeviceToLogical_Y(pt.Y));
        }
    }
}
