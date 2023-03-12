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
        public AxisModel Axis2 = new AxisModel { Display = true, Spacing = 1000, PenModel = new PenModel(Color.Black, 2) };

        public Point OriginalPointInDevicePoint = new Point(0, 0);
        public double Scale = 1; // DevicePoint = Scale * LogicPoint 

        public List<VehicleModel> VehicleModels = new List<VehicleModel> { new VehicleModel() };

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

        // Positions
        public Point Position = new Point(0, 0);
        public int VehicleAngle = 0;
        public int WheelAngle = 0;

        // Draw Vehicle
        public PenModel PenBody = new PenModel(Color.Green, 5);
        public PenModel PenWheel = new PenModel(Color.Red, 3);

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
        }
    }
}
