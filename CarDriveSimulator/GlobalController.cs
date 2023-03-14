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

        public List<VehicleModelExtension> vehicleModelExtensions;
        public VehicleModelExtension activeVehicleModelExtension;

        public List<ParkingModelExtension> parkingModelExtensions;

        private string filePathName;

        public GlobalController()
        {
            this.ScenarioModel = new ScenarioModel();
            this.ScenarioModel.OriginalPointInDevice = new Point(300, 300);

            this.GeometryUtils = new GeometryUtils();
            this.GeometryUtils.OriginalPointInDevice = this.ScenarioModel.OriginalPointInDevice;
            this.GeometryUtils.Scale = this.ScenarioModel.Scale;

            this.ScenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 6000, Cube_W = 2500, Cube_Number = 10, Position = new Point(1500, -27_000) });
            this.ScenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 2500, Cube_W = 5000, Cube_Number = 10, Position = new Point(-7000, -10_000) });
            this.parkingModelExtensions = this.ScenarioModel.ParkingModels.Select(m => new ParkingModelExtension(m, GeometryUtils)).ToList();

            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(0, 0) });
            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(2700, 6000) });
            this.ScenarioModel.VehicleModels.Add(new VehicleModel() { Position = new Point(2700, -6000) });
            this.vehicleModelExtensions = this.ScenarioModel.VehicleModels.Select(m => new VehicleModelExtension(m, GeometryUtils)).ToList();

            this.activeVehicleModelExtension = vehicleModelExtensions[0];
            this.activeVehicleModelExtension.Model.PenBody = new PenModel(Color.DarkGreen, 8);
            this.activeVehicleModelExtension.Model.FrontBackExtionsionLine_Draw = true;
            this.activeVehicleModelExtension.Model.TurningRadius_Draw = true;
            this.activeVehicleModelExtension.Model.GuideLine_Body_Draw = true;

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
                    activeVehicleModelExtension.Model.Position += new Size((int)logicDeltaX, (int)logicDeltaY);
                }
                else
                {
                    //foreach (var m in ScenarioModel.VehicleModels)
                    //{
                    //    MoveVehicleModel(m, logicDeltaX, logicDeltaY);
                    //}

                    MoveVehicleModel(activeVehicleModelExtension, logicDeltaX, logicDeltaY);
                }
            }
        }

        public void MoveVehicleModel(VehicleModelExtension m, double logicDeltaX, double logicDeltaY)
        {
            var logicDistance = Math.Sqrt(Math.Pow(logicDeltaX, 2) + Math.Pow(logicDeltaY, 2));
            // 判断2个向量夹角
            // cos(ang) = ((x1 * x2) + (y1 * y2))
            var vehicleMoveForward = logicDeltaX * Math.Cos(m.VehicleRadian) + logicDeltaY * Math.Sin(m.VehicleRadian) > 0;

            MoveVehicleModel(m, logicDistance, vehicleMoveForward);
        }

        public void MoveVehicleModel(VehicleModelExtension m, double logicDistance, bool moveForward)
        {
            if (m.Model.WheelAngle == 0)
            {
                var xDelta = logicDistance * Math.Cos(m.VehicleRadian) * (moveForward ? 1 : -1);
                var yDelta = logicDistance * Math.Sin(m.VehicleRadian) * (moveForward ? 1 : -1);

                m.Model.Position += new Size((int)(xDelta), (int)(yDelta));
            }
            else
            {
                var or = m.TurningRadius_RelativePoint;
                var turningRadius = Math.Sqrt(Math.Pow(or.X, 2) + Math.Pow(or.Y, 2));
                var angleDelta = GeometryUtils.RadianToAngle(Math.Asin(logicDistance / turningRadius));
                if ((m.Model.WheelAngle > 0 && !moveForward)        // Turn left and move back,
                    || (m.Model.WheelAngle < 0 && moveForward))     // Turn right and move forward
                {
                    angleDelta = -angleDelta;
                }

                m.Model.Position = GeometryUtils.RotatePoint(m.RelativePointToLogic(or), m.Model.Position, angleDelta);
                m.Model.VehicleAngle += angleDelta;
            }
        }

        public void MoveActiveVehicleModel(double logicDistance, bool moveForward)
        {
            MoveVehicleModel(activeVehicleModelExtension, logicDistance, moveForward);
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

                foreach (var m in parkingModelExtensions)
                {
                    m.Draw(g);
                }

                foreach (var m in vehicleModelExtensions)
                {
                    m.Draw(g);
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
                GeometryUtils.DrawLogicLine(g, pen, new Point(x, logicY1), new Point(x, logicY2));
            }
            for (var y = logicY1 / axisModel.Spacing * axisModel.Spacing; y < logicY2; y += axisModel.Spacing)
            {
                GeometryUtils.DrawLogicLine(g, pen, new Point(logicX1, y), new Point(logicX2, y));
            }
        }

        public void RotateSelectedModel(int vehicleAngleDelta, int wheelAngleDelta)
        {
            activeVehicleModelExtension.Rotate(vehicleAngleDelta, wheelAngleDelta);
        }
    }
}
