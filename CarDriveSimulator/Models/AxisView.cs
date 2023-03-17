using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator.Models
{
    class AxisModel
    {
        public bool Display = true;
        public int Spacing = 100;
        public PenModel PenModel = new PenModel(Color.Black);
    }

    class AxisView
    {
        public AxisModel Model { get; }
        private GeometryUtils GeometryUtils;

        public AxisView(AxisModel model, GeometryUtils geometryUtils)
        {
            Model = model;
            GeometryUtils = geometryUtils;
        }

        public void Draw(Graphics g, int logicX1, int logicX2, int logicY1, int logicY2)
        {
            if (!Model.Display)
            {
                return;
            }
            var pen = new Pen(Model.PenModel.Color, Model.PenModel.Width);

            // Draw axis
            for (var x = logicX1 / Model.Spacing * Model.Spacing; x < logicX2; x += Model.Spacing)
            {
                GeometryUtils.DrawLogicLine(g, pen, new Point(x, logicY1), new Point(x, logicY2));
            }
            for (var y = logicY1 / Model.Spacing * Model.Spacing; y < logicY2; y += Model.Spacing)
            {
                GeometryUtils.DrawLogicLine(g, pen, new Point(logicX1, y), new Point(logicX2, y));
            }
        }
    }
}
