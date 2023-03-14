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
        public GeometryUtils GeometryUtils;
        public VehicleModel ActiveVehicleModel;

        private string filePathName;

        public GlobalController()
        {
            this.ScenarioModel = new ScenarioModel();
            this.ScenarioModel.OriginalPointInDevice = new Point(300, 300);

            this.ScenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 6000, Cube_W = 2500, Cube_Number = 10, Position = new Point( 1500, -27_000) });
            this.ScenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 2500, Cube_W = 5000, Cube_Number = 10, Position = new Point(-7000, -10_000) });

            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(0, 0) });
            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(2700, 6000) });
            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(2700, -6000) });

            this.ActiveVehicleModel = this.ScenarioModel.VehicleModels[0];
            this.ActiveVehicleModel.PenBody = new PenModel(Color.DarkGreen, 8);
            this.ActiveVehicleModel.FrontBackExtionsionLine_Draw = true;
            this.ActiveVehicleModel.TurningRadius_Draw = true;
            this.ActiveVehicleModel.GuideLine_Body_Draw = true;

            this.GeometryUtils = new GeometryUtils();
            this.GeometryUtils.OriginalPointInDevice = this.ScenarioModel.OriginalPointInDevice;
            this.GeometryUtils.Scale = this.ScenarioModel.Scale;

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
            var m = ScenarioModel.DeserializeFromJson(value);
            if (m == null)
            {
                throw new Exception("Fail to load scenarios.");
            }
            this.ScenarioModel = m;
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

        public void MoveModel(Size devicePt)
        {
            // System.Diagnostics.Trace.WriteLine($"MoveOriginalPoint {CurrentMouseMode}, {devicePt}");
            if (CurrentMouseMode == MouseMode.MoveScreen)
            {
                ScenarioModel.OriginalPointInDevice = ScenarioModel.OriginalPointInDevice + devicePt;

                this.GeometryUtils.OriginalPointInDevice = this.ScenarioModel.OriginalPointInDevice;
                this.GeometryUtils.Scale = this.ScenarioModel.Scale;
            }
            else if (CurrentMouseMode == MouseMode.MoveSelectComponent)
            {
                var logicDeltaX = devicePt.Width / ScenarioModel.Scale;
                var logicDeltaY = -devicePt.Height / ScenarioModel.Scale;

                if (this.CurrentEditMode == EditMode.Edit)
                {
                    ActiveVehicleModel.Position += new Size((int)logicDeltaX, (int)logicDeltaY);
                }
                else
                {
                    //foreach (var m in ScenarioModel.VehicleModels)
                    //{
                    //    MoveVehicleModel(m, logicDeltaX, logicDeltaY);
                    //}

                    MoveVehicleModel(ActiveVehicleModel, logicDeltaX, logicDeltaY);
                }
            }
        }

        public void MoveVehicleModel(VehicleModel m, double logicDeltaX, double logicDeltaY)
        {
            var logicDistance = Math.Sqrt(Math.Pow(logicDeltaX, 2) + Math.Pow(logicDeltaY, 2));
            // 判断2个向量夹角
            // cos(ang) = ((x1 * x2) + (y1 * y2))
            var vehicleMoveForward = logicDeltaX * Math.Cos(m.VehicleRadian) + logicDeltaY * Math.Sin(m.VehicleRadian) > 0;

            MoveVehicleModel(m, logicDistance, vehicleMoveForward);
        }

        public void MoveVehicleModel(VehicleModel m, double logicDistance, bool moveForward)
        {
            if (m.WheelAngle == 0)
            {
                var xDelta = logicDistance * Math.Cos(m.VehicleRadian) * (moveForward ? 1 : -1);
                var yDelta = logicDistance * Math.Sin(m.VehicleRadian) * (moveForward ? 1 : -1);

                m.Position += new Size((int)(xDelta), (int)(yDelta));
            }
            else
            {
                var or = m.TurningRadius_RelativePoint;
                var turningRadius = Math.Sqrt(Math.Pow(or.X, 2) + Math.Pow(or.Y, 2));
                var angleDelta = GeometryUtils.RadianToAngle(Math.Asin(logicDistance / turningRadius));
                if ((m.WheelAngle > 0 && !moveForward)        // Turn left and move back,
                    || (m.WheelAngle < 0 && moveForward))     // Turn right and move forward
                {
                    angleDelta = -angleDelta;
                }

                m.Position = GeometryUtils.RotatePoint(m.RelativePointToLogic(or), m.Position, angleDelta);
                m.VehicleAngle += angleDelta;
            }
        }

        public void MoveActiveVehicleModel(double logicDistance, bool moveForward)
        {
            MoveVehicleModel(ActiveVehicleModel, logicDistance, moveForward);
        }

        public void ScaleFromPoint(Point scaleAtDevicePt, bool increase)
        {
            // scaleAtDevicePt    ==>   Original
            // Original += scaleAtDevicePt + (Original - scaleAtDevicePt) * (0.9 or 1.1)
            var deltaScale = increase ? 1.1 : 1 / 1.1;
            var sizeX = (ScenarioModel.OriginalPointInDevice.X - scaleAtDevicePt.X) * deltaScale;
            var sizeY = (ScenarioModel.OriginalPointInDevice.Y - scaleAtDevicePt.Y) * deltaScale;

            ScenarioModel.OriginalPointInDevice = scaleAtDevicePt + (new Size((int)sizeX, (int)sizeY));
            ScenarioModel.Scale *= deltaScale;

            this.GeometryUtils.OriginalPointInDevice = this.ScenarioModel.OriginalPointInDevice;
            this.GeometryUtils.Scale = this.ScenarioModel.Scale;
        }

        public Bitmap Draw(int deviceWidth, int deviceHeight)
        {
            var logicX1 = GeometryUtils.DeviceToLogical_X(0);
            var logicX2 = GeometryUtils.DeviceToLogical_X(deviceWidth);
            var logicY1 = GeometryUtils.DeviceToLogical_Y(deviceHeight);
            var logicY2 = GeometryUtils.DeviceToLogical_Y(0);

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

                foreach (var m in ScenarioModel.ParkingModels)
                {
                    DrawParking(g, m);
                }

                foreach (var m in ScenarioModel.VehicleModels)
                {
                    DrawVehicle(g, m);
                }
                DrawVehicle(g, ActiveVehicleModel);
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

        public void DrawParking(Graphics g, ParkingModel m)
        {
            var pen = new Pen(m.Pen.Color, m.Pen.Width);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            for (var i = 0; i < m.Cube_Number; ++i)
            {
                var position = new Point(i * m.Cube_L, 0);
                var pts = new Point[]
                {
                    position,
                    position + new Size(m.Cube_L, 0),
                    position + new Size(m.Cube_L, -m.Cube_W),
                    position + new Size(0, -m.Cube_W),
                };

                DrawLogicLines(g, pen, pts.Select(pt => m.RelativePointToLogic(pt)).ToArray());
            }
        }

        public void DrawVehicle(Graphics g, VehicleModel m)
        {
            // Draw body
            {
                var pen = new Pen(m.PenBody.Color, m.PenBody.Width);
                var body = m.BodyRelativePoints;
                DrawLogicLines(g, pen, body.Select(pt => m.RelativePointToLogic(pt)).ToArray());
            }

            // Draw wheel
            {
                var pen = new Pen(m.PenWheel.Color, m.PenWheel.Width);

                var wheelPositions = m.WheelRelativePositions;
                var wheelAngles = new double[] { m.WheelAngle, m.WheelAngle, 0, 0 }; // Only front wheels have angle.

                for (var i = 0; i < wheelPositions.Length; ++i)
                {
                    var wheel = GeometryUtils.GenerateRectanglePoints(m.Wheel_L, m.Wheel_W);
                    wheel = wheel.Select(pt => GeometryUtils.RotatePoint(new Point(0, 0), pt, wheelAngles[i])).ToArray();  // Rotate from Wheel Position
                    wheel = wheel.Select(pt => pt + new Size(wheelPositions[i])).ToArray();  // Move Wheel Posistion
                    DrawLogicLines(g, pen, wheel.Select(pt => m.RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Rearview Mirrow
            {
                var pen = new Pen(m.PenBody.Color, m.PenBody.Width);

                var positions = m.RearviewMirrowRelativePositions;
                for (var i = 0; i < positions.Length; ++i)
                {
                    var mirrow = GeometryUtils.GenerateRectanglePoints(m.RearviewMirrow_L, m.RearviewMirrow_W);
                    mirrow = mirrow.Select(pt => pt + new Size(positions[i])).ToArray();  // Move Mirrow Posistion
                    DrawLogicLines(g, pen, mirrow.Select(pt => m.RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Driver
            {
                var pen = new Pen(m.PenBody.Color, m.PenBody.Width);

                var position = m.RelativePointToLogic(m.DriverPosition);
                DrawLogicCircle(g, pen, position, m.DriverSize / 2);
            }

            // Draw Front Back Extionsion Line
            {
                if (m.FrontBackExtionsionLine_Draw)
                {
                    var pen = new Pen(m.FrontBackExtionsionLine_Pen.Color, m.FrontBackExtionsionLine_Pen.Width);
                    var body = m.BodyRelativePoints;
                    var extendDirection = new int[] { 1, 1, -1, -1 };
                    for (var i = 0; i < body.Length; ++i)
                    {
                        var pts = new Point[]
                        {
                            body[i],
                            body[i] + new Size(m.FrontBackExtionsionLine_Length * extendDirection[i], 0),
                        };
                        DrawLogicLines(g, pen, pts.Select(pt => m.RelativePointToLogic(pt)).ToArray());
                    }
                }
            }

            // Draw turning radius
            {
                if (m.TurningRadius_Draw && m.WheelAngle != 0)
                {
                    var pen = new Pen(m.TurningRadius_Pen.Color, m.TurningRadius_Pen.Width);

                    var wheelPositions = m.WheelRelativePositions;
                    var wheelPointsToDraw = new Point[2];
                    if (m.WheelAngle > 0)
                    {
                        wheelPointsToDraw[0] = wheelPositions[0];
                        wheelPointsToDraw[1] = wheelPositions[3];
                    }
                    else
                    {
                        wheelPointsToDraw[0] = wheelPositions[1];
                        wheelPointsToDraw[1] = wheelPositions[2];
                    }

                    var pts = new Point[] { wheelPointsToDraw[0], m.TurningRadius_RelativePoint, wheelPointsToDraw[1] };
                    DrawLogicLines(g, pen, pts.Select(pt => m.RelativePointToLogic(pt)).ToArray());
                }
            }

            // Draw Guid lines
            {
                if (m.WheelAngle != 0)
                {
                    if (m.GuideLine_Body_Draw)
                    {
                        var pen = new Pen(m.GuideLine_Body_Pen.Color, m.GuideLine_Body_Pen.Width);

                        var body = m.BodyRelativePoints;
                        foreach (Point pt in body)
                        {
                            DrawLogicCircle(g, pen, m.RelativePointToLogic(m.TurningRadius_RelativePoint), m.RelativePointToLogic(pt));
                        }
                    }

                    if (m.GuideLine_Wheel_Draw)
                    {
                        var pen = new Pen(m.GuideLine_Wheel_Pen.Color, m.GuideLine_Wheel_Pen.Width);

                        var wheelPositions = m.WheelRelativePositions;
                        foreach (Point pt in wheelPositions)
                        {
                            DrawLogicCircle(g, pen, m.RelativePointToLogic(m.TurningRadius_RelativePoint), m.RelativePointToLogic(pt));
                        }
                    }
                }
            }
        }

        public void RotateSelectedModel(int vehicleAngleDelta, int wheelAngleDelta)
        {
            //foreach (var m in ScenarioModel.VehicleModels)
            //{
            //    RotateVehicle(m, vehicleAngleDelta, wheelAngleDelta);
            //}
            RotateVehicle(ActiveVehicleModel, vehicleAngleDelta, wheelAngleDelta);
        }

        public void RotateVehicle(VehicleModel m, int vehicleAngleDelta, int wheelAngleDelta)
        {
            m.Rotate(vehicleAngleDelta, wheelAngleDelta);
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

        public void DrawLogicLine(Graphics g, Pen pen, Point pt1, Point pt2)
        {
            g.DrawLine(pen, GeometryUtils.LogicalToDevice_Point(pt1), GeometryUtils.LogicalToDevice_Point(pt2));
        }

        public void DrawLogicCircle(Graphics g, Pen pen, Point center, Point circumFerence)
        {
            var radius = Math.Sqrt(Math.Pow(center.X - circumFerence.X, 2) + Math.Pow(center.Y - circumFerence.Y, 2));
            DrawLogicCircle(g, pen, center, radius);
        }

        public void DrawLogicCircle(Graphics g, Pen pen, Point center, double radius)
        {
            var x = GeometryUtils.LogicalToDevice_X((int)(center.X - radius));
            var y = GeometryUtils.LogicalToDevice_Y((int)(center.Y + radius));
            var deviceRadius = 2 * radius * ScenarioModel.Scale;
            g.DrawEllipse(pen, new Rectangle(x, y, (int)deviceRadius, (int)deviceRadius));
        }

    }
}
