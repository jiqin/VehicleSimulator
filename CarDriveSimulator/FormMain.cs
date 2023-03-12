using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarDriveSimulator
{
    public partial class FormMain : Form
    {
        private Model model = new Model();
        public FormMain()
        {
            InitializeComponent();

            UpdateMenuItemEditModeDisplayText();
        }

        private void ToolStripMenuItem_New_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Edit_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Save_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_SaveAs_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_EditMode_Click(object sender, EventArgs e)
        {
            model.UpdateEditMode();
            UpdateMenuItemEditModeDisplayText();
        }

        private void UpdateMenuItemEditModeDisplayText()
        {
            ToolStripMenuItem_EditMode.Text = (model.CurrentEditMode == Model.EditMode.Edit ? "Edit Mode" : "Simulator Mode");
        }
    }
}
