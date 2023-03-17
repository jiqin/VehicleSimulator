using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator.Models
{
    class TagMode
    {
        public int ID = 0;
        public string Text = "";
        public int DeviceRadius = 30;
        public Point Position = new Point(0, 0);
        public PenModel PenModel = new PenModel(Color.Brown, 3);
    }

    class TagView
    {
        public TagMode Model { get; }
        private GeometryUtils GeometryUtils;

        public TagView(TagMode model, GeometryUtils geometryUtils)
        {
            Model = model;
            GeometryUtils = geometryUtils;
        }

        public void Draw(Graphics g)
        {
            var pen = new Pen(Model.PenModel.Color, Model.PenModel.Width);

            var devicePosition = GeometryUtils.LogicalToDevice_Point(Model.Position);
            var x = (int)(devicePosition.X - Model.DeviceRadius);
            var y = (int)(devicePosition.Y - Model.DeviceRadius);
            g.DrawEllipse(pen, new Rectangle(x, y, (int)Model.DeviceRadius * 2, (int)Model.DeviceRadius * 2));
            g.DrawLine(pen, new Point(devicePosition.X - Model.DeviceRadius, devicePosition.Y), new Point(devicePosition.X + Model.DeviceRadius, devicePosition.Y));
            g.DrawLine(pen, new Point(devicePosition.X, devicePosition.Y - Model.DeviceRadius), new Point(devicePosition.X, devicePosition.Y + Model.DeviceRadius));

            var s = string.IsNullOrEmpty(Model.Text) ? $"{Model.ID}" : Model.Text;

            StringFormat sf = new StringFormat();
            //sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            var fontSize = 20;
            g.DrawString(s, new Font(FontFamily.GenericSerif, fontSize, FontStyle.Regular), Brushes.Brown, devicePosition + new Size(0, Model.DeviceRadius), sf);
        }

        public bool IsSelected(Point devicePt)
        {
            var devicePosition = GeometryUtils.LogicalToDevice_Point(Model.Position);
            return GeometryUtils.GetDistance(devicePt, devicePosition) <= Model.DeviceRadius;
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                Model.PenModel = new PenModel(Color.Brown, 8);
            }
            else
            {
                Model.PenModel = new PenModel(Color.Brown, 3);
            }
        }
    }
}
