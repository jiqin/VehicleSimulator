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
        private Bitmap cachedBitmap = new Bitmap(1, 1);
        private Graphics cachedG;

        private ScenarioModel scenarioModel;
        private GeometryUtils geometryUtils;

        private List<VehicleModelExtension> vehicleModelExtensions;
        private VehicleModelExtension selectedVehicleModelExtension;

        private List<ParkingModelExtension> parkingModelExtensions;
        private ParkingModelExtension selectedParkingModelExtension;

        private string filePathName;

        public GlobalController()
        {
            this.parkingModelExtensions = new List<ParkingModelExtension>();
            this.vehicleModelExtensions = new List<VehicleModelExtension>();

            this.scenarioModel = new ScenarioModel();
            this.scenarioModel.OriginalPointInDevice = new Point(300, 300);

            this.geometryUtils = new GeometryUtils();
            this.geometryUtils.OriginalPointInDevice = this.scenarioModel.OriginalPointInDevice;
            this.geometryUtils.Scale = this.scenarioModel.Scale;

            this.scenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 6000, Cube_W = 2500, Cube_Number = 10, Position = new Point(1500, -27_000) });
            this.scenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 2500, Cube_W = 5000, Cube_Number = 10, Position = new Point(-7000, -10_000) });
            this.parkingModelExtensions = this.scenarioModel.ParkingModels.Select(m => new ParkingModelExtension(m, geometryUtils)).ToList();

            AddNewVehicle(new Point(0, 0));
            AddNewVehicle(new Point(2700, 6000));
            AddNewVehicle(new Point(2700, -6000));

            Reset();
        }

        public void AddNewVehicle(Point position)
        {
            var m = new VehicleModel() { Position = position };
            this.scenarioModel.VehicleModels.Add(m);
            this.vehicleModelExtensions.Add(new VehicleModelExtension(m, geometryUtils));
        }

        public void LoadFromFile(string filePathName)
        {
            UpdateScenarios(File.ReadAllText(filePathName));
            Reset();
            this.filePathName = filePathName;
        }

        public string GetScenarioText()
        {
            return ScenarioModel.SerializeToJson(this.scenarioModel);
        }

        public void UpdateScenarios(string value)
        {
            var m = ScenarioModel.DeserializeFromJson(value);
            if (m == null)
            {
                throw new Exception("Fail to load scenarios.");
            }
            this.scenarioModel = m;
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
            cachedBitmap = new Bitmap(1, 1);
            cachedG = Graphics.FromImage(cachedBitmap);
        }

        public void UpdateSelectedModel(Point devicePt, bool isDrive)
        {
            var logicPt = geometryUtils.DeviceToLogical_Point(devicePt);

            UnselectAllMode();

            foreach (var m in vehicleModelExtensions)
            {
                if (m.IsSelected(logicPt))
                {
                    this.selectedVehicleModelExtension = m;
                    this.selectedVehicleModelExtension.SetSelected(true, isDrive);
                    return;
                }
            }

            foreach (var m in parkingModelExtensions)
            {
                if (m.IsSelected(logicPt))
                {
                    this.selectedParkingModelExtension = m;
                    this.selectedParkingModelExtension.SetSelected(true);
                    return;
                }
            }
        }

        public void UnselectAllMode()
        {
            if (this.selectedVehicleModelExtension != null)
            {
                this.selectedVehicleModelExtension.SetSelected(false, false);
                this.selectedVehicleModelExtension = null;
            }

            if (this.selectedParkingModelExtension != null)
            {
                this.selectedParkingModelExtension.SetSelected(false);
                this.selectedParkingModelExtension = null;
            }
        }

        public void MoveModel(Point previousDevicePt, Point currentDevicePt)
        {
            var deviceSize = new Size(currentDevicePt.X - previousDevicePt.X, currentDevicePt.Y - previousDevicePt.Y);
            var logicSize = new Size((int)(deviceSize.Width / scenarioModel.Scale), (int)(-deviceSize.Height / scenarioModel.Scale));

            if (selectedVehicleModelExtension == null && selectedParkingModelExtension == null) // Move whole screen
            {
                scenarioModel.OriginalPointInDevice = scenarioModel.OriginalPointInDevice + deviceSize;

                this.geometryUtils.OriginalPointInDevice = this.scenarioModel.OriginalPointInDevice;
                this.geometryUtils.Scale = this.scenarioModel.Scale;
            }
            else
            {
                selectedVehicleModelExtension?.Move(logicSize);
                selectedParkingModelExtension?.Move(logicSize);
            }
        }

        public void DriveActiveVehicleModel(double logicDistance, bool moveForward)
        {
            selectedVehicleModelExtension?.Drive(logicDistance, moveForward);
        }

        public void ScaleFromPoint(Point scaleAtDevicePt, bool increase)
        {
            // scaleAtDevicePt    ==>   Original
            // Original += scaleAtDevicePt + (Original - scaleAtDevicePt) * (0.9 or 1.1)
            var deltaScale = increase ? 1.1 : 1 / 1.1;
            var sizeX = (scenarioModel.OriginalPointInDevice.X - scaleAtDevicePt.X) * deltaScale;
            var sizeY = (scenarioModel.OriginalPointInDevice.Y - scaleAtDevicePt.Y) * deltaScale;

            scenarioModel.OriginalPointInDevice = scaleAtDevicePt + (new Size((int)sizeX, (int)sizeY));
            scenarioModel.Scale *= deltaScale;

            this.geometryUtils.OriginalPointInDevice = this.scenarioModel.OriginalPointInDevice;
            this.geometryUtils.Scale = this.scenarioModel.Scale;
        }

        public Bitmap Draw(int deviceWidth, int deviceHeight)
        {
            var logicX1 = geometryUtils.DeviceToLogical_X(0);
            var logicX2 = geometryUtils.DeviceToLogical_X(deviceWidth);
            var logicY1 = geometryUtils.DeviceToLogical_Y(deviceHeight);
            var logicY2 = geometryUtils.DeviceToLogical_Y(0);

            if (cachedBitmap.Width < deviceWidth || cachedBitmap.Height < deviceHeight)
            {
                cachedBitmap = new Bitmap(deviceWidth, deviceHeight);
                cachedG = Graphics.FromImage(cachedBitmap);
            }

            var g = cachedG;
            //using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, cachedBitmap.Width, cachedBitmap.Height));

                DrawAxis(g, scenarioModel.Axis1, logicX1, logicX2, logicY1, logicY2);
                DrawAxis(g, scenarioModel.Axis2, logicX1, logicX2, logicY1, logicY2);

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
                geometryUtils.DrawLogicLine(g, pen, new Point(x, logicY1), new Point(x, logicY2));
            }
            for (var y = logicY1 / axisModel.Spacing * axisModel.Spacing; y < logicY2; y += axisModel.Spacing)
            {
                geometryUtils.DrawLogicLine(g, pen, new Point(logicX1, y), new Point(logicX2, y));
            }
        }

        public void RotateSelectedModel(int angleDelta)
        {
            selectedVehicleModelExtension?.Rotate(angleDelta);
            selectedParkingModelExtension?.Rotate(angleDelta);
        }
    }
}
