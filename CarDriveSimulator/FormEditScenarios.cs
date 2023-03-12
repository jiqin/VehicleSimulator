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
    public partial class FormEditScenarios : Form
    {
        public string TextContent;
        public bool IsOK = false;
        public FormEditScenarios(string text)
        {
            InitializeComponent();

            this.TextContent = text;
            this.richTextBox1.Text = text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            TextContent = this.richTextBox1.Text;
            IsOK = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }

        private void FormEditScenarios_Resize(object sender, EventArgs e)
        {
            richTextBox1.Width = this.Width - richTextBox1.Left - 10;
            richTextBox1.Height = this.Height - richTextBox1.Top - 10;
        }
    }
}
