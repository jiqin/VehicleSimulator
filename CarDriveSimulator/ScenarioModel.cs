using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CarDriveSimulator
{

    public class PenModel
    {
        public Color Color;
        public int Width;

        public PenModel(Color color, int width = 1)
        {
            Color = color;
            Width = width;
        }
    }

    class AxisModel
    {
        public bool Display = true;
        public int Spacing = 100;
        public PenModel PenModel = new PenModel(Color.Black);
    }

    class ScenarioModel
    {
        public AxisModel Axis1 = new AxisModel { Display = true, Spacing = 100, PenModel = new PenModel(Color.LightGreen, 1) };
        public AxisModel Axis2 = new AxisModel { Display = true, Spacing = 1000, PenModel = new PenModel(Color.LightGray, 2) };

        public Point OriginalPointInDevicePoint = new Point(0, 0);
        public double Scale = 1; // DevicePoint = Scale * LogicPoint 

        public List<VehicleModel> VehicleModels = new List<VehicleModel>();

        public static string SerializeToJson(ScenarioModel o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public static ScenarioModel DeserializeFromJson(string v)
        {
            return JsonConvert.DeserializeObject<ScenarioModel>(v);
        }
    }

    /*
     * \  |
     *  \ |
     *   \|  Angle = -30
     * 
     * |----------------------------
     * | Front Overhang
     * |
     * | |-|                 |-|
     * |       Wheel Track   Wheel Size
     * |
     * |
     * |
     * | Wheel Base
     * |
     * |
     * |
     * |
     * |
     * | |-|                 |-|
     * |
     * |----------------------------
     * 
     */
    class VehicleModel
    {
        public string Name = "My Car 1";
        public string Model = "ModelY";

        // Fixed dimensions
        public int Dimension_L = 4750;
        public int Dimension_W = 1921;
        public int Dimension_H = 1624;
        public int FrontOverhang = 800;

        public int WheelBase = 2890;
        public int WheelTrack = 1636;
        public int Wheel_L = 800;
        public int Wheel_W = 194;

        public Point[] WheelRelativePositions
        {
            get
            {
                return new Point[] {
                    new Point(-WheelTrack / 2,  Dimension_L / 2 - FrontOverhang),
                    new Point( WheelTrack / 2,  Dimension_L / 2 - FrontOverhang),
                    new Point( WheelTrack / 2, -(WheelBase - (Dimension_L / 2 - FrontOverhang))),
                    new Point(-WheelTrack / 2, -(WheelBase - (Dimension_L / 2 - FrontOverhang)))};
            }
        }

        // Positions
        public Point Position = new Point(0, 0);
        public double VehicleAngle = 0;  // Here the Angle is from Y axis to right
        public double WheelAngle = 0;

        // Draw Vehicle
        public PenModel PenBody = new PenModel(Color.Green, 5);
        public PenModel PenWheel = new PenModel(Color.Red, 3);

        // Draw extention
        public bool FrontBackExtionsionLine_Draw = true;
        public int FrontBackExtionsionLine_Length = 10000;
        public PenModel FrontBackExtionsionLine_Pen = new PenModel(Color.Yellow, 5);

        public bool TurningRadius_Draw = true;
        public PenModel TurningRadius_Pen = new PenModel(Color.Yellow, 5);
        public Point TurningRadiusPoint = new Point(0, 0);

        public bool GuideLine_Body_Draw = true;
        public PenModel GuideLine_Body_Pen = new PenModel(Color.Green, 2);

        public bool GuideLine_Wheel_Draw = false;
        public PenModel GuideLine_Wheel_Pen = new PenModel(Color.Red, 2);

        public void Rotate(int vehicleAngleDelta, int wheelAngleDelta)
        {
            VehicleAngle += vehicleAngleDelta;

            WheelAngle += wheelAngleDelta;
            if (WheelAngle < -30)
            {
                WheelAngle = -30;
            }
            if (WheelAngle > 30)
            {
                WheelAngle = 30;
            }

            /*
             *  Wheel0    Wheel1
             *  
             *  
             *           Length1          tan(wheel angle) = Length1 / Length2
             *  
             *  
             *  Wheel3    Wheel2                Length2                              Turning Radius Point
             */
            var flag = 1;
            var tmpWheelAngle = WheelAngle;
            if (tmpWheelAngle < 0)
            {
                tmpWheelAngle = -tmpWheelAngle;
                flag = -1;
            }
            if (tmpWheelAngle > 0)
            {
                var wheelPositions = WheelRelativePositions;
                var y = wheelPositions[2].Y;
                var length1 = wheelPositions[1].Y - wheelPositions[2].Y;
                var length2 = length1 / Math.Tan(tmpWheelAngle * Math.PI / 180);
                TurningRadiusPoint = new Point((int)(wheelPositions[2].X + length2) * flag, y);
            }
        }
    }
}
