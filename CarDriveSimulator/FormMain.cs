using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace CarDriveSimulator
{
    public partial class FormMain : Form
    {
        private GlobalController controller;
        private string latestEventMessage = "";
        private MouseEventArgs mouseEventArgs;
        private Point previousMousePoint = new Point(0, 0);

        public FormMain()
        {
            CreateNewScenario();

            InitializeComponent();

            UpdateMenuItemEditModeDisplayText();
        }

        private void ToolStripMenuItem_New_Click(object sender, EventArgs e)
        {
            CreateNewScenario();
        }

        private void CreateNewScenario()
        {
            controller = new GlobalController();
        }

        private void ToolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Filter = "*.json|*.*";
            fd.ShowDialog();

            if (!string.IsNullOrEmpty(fd.FileName))
            {
                try
                {
                    controller.LoadFromFile(fd.FileName);
                }
                catch (Exception ex)
                {
                    labelDebugInfo.Text = ex.ToString();
                }
            }
        }

        private void ToolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            if (!controller.Save())
            {
                SaveToFile();
            }
        }

        private void ToolStripMenuItem_SaveAs_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void SaveToFile()
        {
            var fd = new SaveFileDialog();
            fd.Filter = "*.json|*.*";
            fd.FileName = Path.Combine(fd.InitialDirectory, $"scenarios_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.json");
            fd.ShowDialog();
            if (!string.IsNullOrEmpty(fd.FileName))
            {
                try
                {
                    var fileName = fd.FileName;
                    if (!fileName.EndsWith(".json"))
                    {
                        fileName += ".json";
                    }
                    controller.Save(fd.FileName);
                }
                catch(Exception e)
                {
                    labelDebugInfo.Text = e.ToString();
                }
            }
        }

        private void ToolStripMenuItem_Edit_Click(object sender, EventArgs e)
        {
            var textBox = new FormEditScenarios(controller.GetScenarioText());
            textBox.ShowDialog();
            if (textBox.IsOK)
            {
                try
                {
                    controller.UpdateScenarios(textBox.TextContent);
                    pictureBoxDraw.Invalidate();
                }
                catch (Exception ex)
                {
                    labelDebugInfo.Text = ex.ToString();
                }
            }
        }

        private void ToolStripMenuItem_EditMode_Click(object sender, EventArgs e)
        {
            controller.UpdateEditMode();
            UpdateMenuItemEditModeDisplayText();
        }

        private void UpdateMenuItemEditModeDisplayText()
        {
            ToolStripMenuItem_EditMode.Text = (controller.CurrentEditMode == GlobalController.EditMode.Edit ? "Edit Mode" : "Simulator Mode");
        }

        private void Update_EventMessage(string eventMessage)
        {
            latestEventMessage = eventMessage;
            //Trace.WriteLine($"Get event: {eventMessage}");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Update_EventMessage("FormMain_Load");

            Update_AllComponents();
        }

        private void Update_AllComponents()
        {
            Update_PannelSize();

            pictureBoxDraw.Invalidate();

            Update_LabelDebugInfo();
        }

        private void Update_PannelSize()
        {
            var pad = 10;
            panelDebugInfo.Width = this.Width - panelDebugInfo.Left - pad;
            labelDebugInfo.Width = panelDebugInfo.Width - labelDebugInfo.Left - pad;

            panelDraw.Width = this.Width - panelDraw .Left - pad;
            panelDraw.Height = this.Height - panelDraw.Top - pad;

            pictureBoxDraw.Width = panelDraw.Width - pictureBoxDraw.Left - pad;
            pictureBoxDraw.Height = panelDraw.Height - pictureBoxDraw.Top - pad;
        }

        private void Update_LabelDebugInfo()
        {
            string debugInfo = $"";
            debugInfo += $", Event: {latestEventMessage}";
            //debugInfo += $", FormSize: {this.Width}, {this.Height}";
            //debugInfo += $", panelDebugInfo: {panelDebugInfo.Left}, {panelDebugInfo.Top}, {panelDebugInfo.Right}, {panelDebugInfo.Bottom}";
            //debugInfo += $", panelDraw: {panelDraw.Left}, {panelDraw.Top}, {panelDraw.Right}, {panelDraw.Bottom}";
            //debugInfo += $", pictureBoxDraw: {pictureBoxDraw.Left}, {pictureBoxDraw.Top}, {pictureBoxDraw.Right}, {pictureBoxDraw.Bottom}";

            debugInfo += $", Original: {controller.ScenarioModel.OriginalPointInDevicePoint}";
            debugInfo += $", Scale: {controller.ScenarioModel.Scale:F2}";
            if (mouseEventArgs != null)
            {
                debugInfo += $", mouseEventArgs: {mouseEventArgs.Location} => logic: {controller.DeviceToLogical_Point(mouseEventArgs.Location)}, Button: {mouseEventArgs.Button}, Clicks: {mouseEventArgs.Clicks}, Delta: {mouseEventArgs.Delta}";
            }
            labelDebugInfo.Text = debugInfo;
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            Update_EventMessage("FormMain_Paint");
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            Update_EventMessage("FormMain_Resize");

            Update_AllComponents();
        }

        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_Paint");

            var bitmap = controller.Draw(pictureBoxDraw.Width, pictureBoxDraw.Height);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void pictureBoxDraw_MouseDown(object sender, MouseEventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_MouseDown");
            previousMousePoint = e.Location;
            if (e.Button == MouseButtons.Right)
            {
                controller.CurrentMouseMode = GlobalController.MouseMode.MoveScreen;
            }
            else if (e.Button == MouseButtons.Left)
            {
                controller.CurrentMouseMode = GlobalController.MouseMode.MoveSelectComponent;
            }
            
            mouseEventArgs = e;
            Update_LabelDebugInfo();
            pictureBoxDraw.Invalidate();
        }

        private void pictureBoxDraw_MouseUp(object sender, MouseEventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_MouseUp");
            previousMousePoint = e.Location;
            controller.CurrentMouseMode = GlobalController.MouseMode.None;
            mouseEventArgs = e;
            Update_LabelDebugInfo();
            pictureBoxDraw.Invalidate();
        }

        private void pictureBoxDraw_MouseWheel(object sender, MouseEventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_MouseWheel");
            controller.ScaleFromPoint(e.Location, e.Delta > 0);
            mouseEventArgs = e;
            Update_LabelDebugInfo();
            pictureBoxDraw.Invalidate();
        }

        private void pictureBoxDraw_MouseMove(object sender, MouseEventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_MouseMove");
            controller.MoveModel(new Size(e.Location.X - previousMousePoint.X, e.Location.Y - previousMousePoint.Y));
            previousMousePoint = e.Location;
            mouseEventArgs = e;
            Update_LabelDebugInfo();

            if (e.Button != MouseButtons.None)
            {
                pictureBoxDraw.Invalidate();
            }
        }

        private void pictureBoxDraw_Click(object sender, EventArgs e)
        {
            Update_EventMessage($"pictureBoxDraw_Click");
            Update_LabelDebugInfo();
            pictureBoxDraw.Invalidate();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            var redraw = false;
            switch(e.KeyCode)
            {
                case Keys.A:
                    controller.RotateActiveVehicle(0, -1);
                    redraw = true;
                    break;
                case Keys.D:
                    controller.RotateActiveVehicle(0, 1);
                    redraw = true;
                    break;
                case Keys.Q:
                    controller.RotateActiveVehicle(-1, 0);
                    redraw = true;
                    break;
                case Keys.E:
                    controller.RotateActiveVehicle(1, 0);
                    redraw = true;
                    break;
                default:
                    break;
            }

            if (redraw)
            {
                Update_EventMessage($"FormMain_KeyDown");
                Update_LabelDebugInfo();
                pictureBoxDraw.Invalidate();
            }
        }
    }
}
