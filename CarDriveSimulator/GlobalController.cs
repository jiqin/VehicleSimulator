using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using CarDriveSimulator.Models;

namespace CarDriveSimulator
{
    class GlobalController
    {
        private Bitmap cachedBitmap = new Bitmap(1, 1);
        private Graphics cachedG;

        private ScenarioModel scenarioModel;
        private GeometryUtils geometryUtils;

        private AxisView axisView1;
        private AxisView axisView2;

        private List<VehicleView> vehicleViews;
        private VehicleView selectedVehicleView;

        private List<ParkingView> parkingViews;
        private ParkingView selectedParkingView;

        private List<TagView> tagViews;
        private TagView selectedTagView;

        private bool inRecordingBodyTrace = false;
        private List<List<Point>> bodyTraceLines = new List<List<Point>>();
        private PenModel penBodyTraceLine = new PenModel(Color.YellowGreen, 3);

        private bool inRecordingWheelTrace = false;
        private List<List<Point>> wheelTraceLines = new List<List<Point>>();
        private PenModel penWheelTraceLine = new PenModel(Color.OrangeRed, 3);

        private string filePathName;

        public GlobalController()
        {
            this.parkingViews = new List<ParkingView>();
            this.vehicleViews = new List<VehicleView>();

            this.scenarioModel = new ScenarioModel();
            this.scenarioModel.OriginalPointInDevice = new Point(1000, 300);

            this.geometryUtils = new GeometryUtils();
            this.geometryUtils.OriginalPointInDevice = this.scenarioModel.OriginalPointInDevice;
            this.geometryUtils.Scale = this.scenarioModel.Scale;

            this.scenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 6000, Cube_W = 2500, Cube_Number = 10, Position = new Point(1500, -27_000) });
            this.scenarioModel.ParkingModels.Add(new ParkingModel { Cube_L = 2500, Cube_W = 5000, Cube_Number = 10, Position = new Point(-7000, -10_000) });

            UpdateViewsFromCurrentScenarios();

            AddNewVehicle(new Point(0, 0));
            AddNewVehicle(new Point(2700, 6000));
            AddNewVehicle(new Point(2700, -6000));

            Reset();
        }

        public void UpdateViewsFromCurrentScenarios()
        {
            axisView1 = new AxisView(this.scenarioModel.Axis1, geometryUtils);
            axisView2 = new AxisView(this.scenarioModel.Axis2, geometryUtils);

            this.parkingViews = this.scenarioModel.ParkingModels.Select(m => new ParkingView(m, geometryUtils)).ToList();
            this.vehicleViews = this.scenarioModel.VehicleModels.Select(m => new VehicleView(m, geometryUtils)).ToList();
            this.tagViews = this.scenarioModel.TagModes.Select(m => new TagView(m, geometryUtils)).ToList();
        }

        public void AddNewVehicle(Point logicPosition)
        {
            var m = new VehicleModel() { Position = logicPosition };
            this.scenarioModel.VehicleModels.Add(m);
            this.vehicleViews.Add(new VehicleView(m, geometryUtils));
        }

        public void AddNewTag(Point devicePt)
        {
            var m = new TagMode() { Position = geometryUtils.DeviceToLogical_Point(devicePt), ID = this.scenarioModel.TagModes.Count };
            this.scenarioModel.TagModes.Add(m);
            this.tagViews.Add(new TagView(m, geometryUtils));
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

            UpdateViewsFromCurrentScenarios();
        }

        public void RemoveSelectedTagModel()
        {
            if (this.selectedTagView != null)
            {
                this.scenarioModel.TagModes.Remove(this.selectedTagView.Model);
                this.selectedTagView = null;

                UpdateViewsFromCurrentScenarios();
            }
        }

        public void UpdateSelectedModel(Point devicePt, bool isDrive)
        {
            var logicPt = geometryUtils.DeviceToLogical_Point(devicePt);

            UnselectAllMode();

            foreach (var m in tagViews)
            {
                if (m.IsSelected(devicePt))
                {
                    this.selectedTagView = m;
                    this.selectedTagView.SetSelected(true);
                    return;
                }
            }

            foreach (var m in vehicleViews)
            {
                if (m.IsSelected(logicPt))
                {
                    if (this.selectedVehicleView != m)
                    {
                        this.selectedVehicleView = m;
                        this.selectedVehicleView.SetSelected(true, isDrive);

                        AddWheelToTraceLines(selectedVehicleView, true);
                    }

                    return;
                }
            }

            foreach (var m in parkingViews)
            {
                if (m.IsSelected(logicPt))
                {
                    this.selectedParkingView = m;
                    this.selectedParkingView.SetSelected(true);
                    return;
                }
            }
        }

        public void UnselectAllMode()
        {
            if (this.selectedTagView != null)
            {
                this.selectedTagView.SetSelected(false);
                this.selectedTagView = null;
            }

            if (this.selectedVehicleView != null)
            {
                this.selectedVehicleView.SetSelected(false, false);
                this.selectedVehicleView = null;
            }

            if (this.selectedParkingView != null)
            {
                this.selectedParkingView.SetSelected(false);
                this.selectedParkingView = null;
            }
        }

        public void MoveModel(Point previousDevicePt, Point currentDevicePt)
        {
            var deviceSize = new Size(currentDevicePt.X - previousDevicePt.X, currentDevicePt.Y - previousDevicePt.Y);
            var logicSize = new Size((int)(deviceSize.Width / scenarioModel.Scale), (int)(-deviceSize.Height / scenarioModel.Scale));

            if (selectedVehicleView == null && selectedParkingView == null) // Move whole screen
            {
                scenarioModel.OriginalPointInDevice = scenarioModel.OriginalPointInDevice + deviceSize;

                this.geometryUtils.OriginalPointInDevice = this.scenarioModel.OriginalPointInDevice;
                this.geometryUtils.Scale = this.scenarioModel.Scale;
            }
            else if (selectedVehicleView != null)
            {
                selectedVehicleView.Move(logicSize);
                AddWheelToTraceLines(selectedVehicleView, false);
            }
            else
            {
                selectedParkingView?.Move(logicSize);
            }
        }

        public void DriveSelectedVehicleModel(double logicDistance, bool moveForward)
        {
            if (selectedVehicleView != null)
            {
                selectedVehicleView.Drive(logicDistance, moveForward);
                AddWheelToTraceLines(selectedVehicleView, false);
            }
        }

        private void AddWheelToTraceLines(VehicleView m, bool isNewSelectedVehicle)
        {
            if (m == null || !m.IsDriveMode)
            {
                return;
            }

            Action f1 = () =>
            {
                if (!inRecordingBodyTrace)
                {
                    return;
                }

                var pos = m.BodyRelativePoints.Select(p => m.RelativePointToLogic(p)).ToList();
                if (bodyTraceLines.Count > 0)
                {
                    // In order not to show trace line too density.
                    if (geometryUtils.GetDistance(bodyTraceLines.LastOrDefault()[0], pos[0]) <= 300)
                    {
                        return;
                    }
                }
                bodyTraceLines.Add(pos);
            };
            f1();

            Action f2 = () => {
                if (!inRecordingWheelTrace)
                {
                    return;
                }

                var pos = m.WheelRelativePositions.Select(p => m.RelativePointToLogic(p)).ToList();
                if (wheelTraceLines.Count > 0)
                {
                    // In order not to show trace line too density.
                    if (geometryUtils.GetDistance(wheelTraceLines.LastOrDefault()[0], pos[0]) <= 300)
                    {
                        return;
                    }
                }

                if (isNewSelectedVehicle)
                {
                    for (var i = 0; i < pos.Count; ++i)
                    {
                        wheelTraceLines.Add(new List<Point>());
                    }
                    
                }
                for (var i = 0; i < pos.Count; ++i)
                {
                    wheelTraceLines[wheelTraceLines.Count - pos.Count + i].Add(pos[i]);
                }
            };
            f2();
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

        public bool DisplayAxis1 { get { return this.scenarioModel.Axis1.Display; } set { this.scenarioModel.Axis1.Display = value; } }
        public bool DisplayAxis2 { get { return this.scenarioModel.Axis2.Display; } set { this.scenarioModel.Axis2.Display = value; } }

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

                axisView1.Draw(g, logicX1, logicX2, logicY1, logicY2);
                axisView2.Draw(g, logicX1, logicX2, logicY1, logicY2);

                foreach (var m in parkingViews)
                {
                    m.Draw(g);
                }

                foreach (var m in vehicleViews)
                {
                    m.Draw(g);
                }

                foreach (var m in tagViews)
                {
                    m.Draw(g);
                }

                DrawTraceline(g);
            }

            return cachedBitmap;
        }

        private void DrawTraceline(Graphics g)
        {
            Action<PenModel, List<List<Point>>, bool> DrawTraceLine = (penModel, tracelines, enclose) =>
            {
                var pen = new Pen(penModel.Color, penModel.Width);
                foreach (var line in tracelines)
                {
                    geometryUtils.DrawLogicLines(g, pen, line.ToArray(), enclose);
                }
            };

            DrawTraceLine(penBodyTraceLine, bodyTraceLines, true);
            DrawTraceLine(penWheelTraceLine, wheelTraceLines, false);
        }

        public void RotateSelectedModel(int angleDelta)
        {
            selectedVehicleView?.Rotate(angleDelta);
            selectedParkingView?.Rotate(angleDelta);
        }

        public void SetDisplayVehicleGuideLineBody(bool[] display)
        {
            foreach (var m in vehicleViews)
            {
                m.SetDisplayGuideLineBody(display);
            }
        }

        public void SetDisplayVehicleGuideLineWheel(bool[] display)
        {
            foreach (var m in vehicleViews)
            {
                m.SetDisplayGuideLineWheel(display);
            }
        }

        public void RecordBodyTrace(bool record)
        {
            inRecordingBodyTrace = record;
            if (!inRecordingBodyTrace)
            {
                bodyTraceLines.Clear();
            }
        }

        public void RecordWheelTrace(bool record)
        {
            inRecordingWheelTrace = record;
            if (!inRecordingWheelTrace)
            {
                wheelTraceLines.Clear();
            }
        }
    }
}
