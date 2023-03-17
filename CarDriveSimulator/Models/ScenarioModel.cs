using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CarDriveSimulator.Models
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

    class ScenarioModel
    {
        public AxisModel Axis1 = new AxisModel { Display = false, Spacing = 100, PenModel = new PenModel(Color.LightGreen, 1) };
        public AxisModel Axis2 = new AxisModel { Display = true, Spacing = 1000, PenModel = new PenModel(Color.LightGray, 2) };

        public Point OriginalPointInDevice = new Point(0, 0);
        public double Scale = 0.1; // DevicePoint = Scale * LogicPoint 

        public List<VehicleModel> VehicleModels = new List<VehicleModel>();

        public List<ParkingModel> ParkingModels = new List<ParkingModel>();

        public List<TagMode> TagModes = new List<TagMode>();

        public static string SerializeToJson(ScenarioModel o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public static ScenarioModel DeserializeFromJson(string v)
        {
            return JsonConvert.DeserializeObject<ScenarioModel>(v);
        }
    }
}
