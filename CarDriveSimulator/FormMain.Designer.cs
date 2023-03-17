
namespace CarDriveSimulator
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_New = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.panelDraw = new System.Windows.Forms.Panel();
            this.pictureBoxDraw = new System.Windows.Forms.PictureBox();
            this.panelDebugInfo = new System.Windows.Forms.Panel();
            this.labelDebugInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonDeleteTag = new System.Windows.Forms.Button();
            this.buttonAddCar = new System.Windows.Forms.Button();
            this.checkBoxShowAxis2 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowAxis1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.panelDraw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).BeginInit();
            this.panelDebugInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_New,
            this.ToolStripMenuItem_Open,
            this.ToolStripMenuItem_Save,
            this.ToolStripMenuItem_SaveAs,
            this.ToolStripMenuItem_Edit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1656, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_New
            // 
            this.ToolStripMenuItem_New.Name = "ToolStripMenuItem_New";
            this.ToolStripMenuItem_New.Size = new System.Drawing.Size(82, 36);
            this.ToolStripMenuItem_New.Text = "New";
            this.ToolStripMenuItem_New.Click += new System.EventHandler(this.ToolStripMenuItem_New_Click);
            // 
            // ToolStripMenuItem_Open
            // 
            this.ToolStripMenuItem_Open.Name = "ToolStripMenuItem_Open";
            this.ToolStripMenuItem_Open.Size = new System.Drawing.Size(93, 36);
            this.ToolStripMenuItem_Open.Text = "Open";
            this.ToolStripMenuItem_Open.Click += new System.EventHandler(this.ToolStripMenuItem_Open_Click);
            // 
            // ToolStripMenuItem_Save
            // 
            this.ToolStripMenuItem_Save.Name = "ToolStripMenuItem_Save";
            this.ToolStripMenuItem_Save.Size = new System.Drawing.Size(84, 36);
            this.ToolStripMenuItem_Save.Text = "Save";
            this.ToolStripMenuItem_Save.Click += new System.EventHandler(this.ToolStripMenuItem_Save_Click);
            // 
            // ToolStripMenuItem_SaveAs
            // 
            this.ToolStripMenuItem_SaveAs.Name = "ToolStripMenuItem_SaveAs";
            this.ToolStripMenuItem_SaveAs.Size = new System.Drawing.Size(116, 36);
            this.ToolStripMenuItem_SaveAs.Text = "Save As";
            this.ToolStripMenuItem_SaveAs.Click += new System.EventHandler(this.ToolStripMenuItem_SaveAs_Click);
            // 
            // ToolStripMenuItem_Edit
            // 
            this.ToolStripMenuItem_Edit.Name = "ToolStripMenuItem_Edit";
            this.ToolStripMenuItem_Edit.Size = new System.Drawing.Size(74, 36);
            this.ToolStripMenuItem_Edit.Text = "Edit";
            this.ToolStripMenuItem_Edit.Click += new System.EventHandler(this.ToolStripMenuItem_Edit_Click);
            // 
            // panelDraw
            // 
            this.panelDraw.Controls.Add(this.pictureBoxDraw);
            this.panelDraw.Location = new System.Drawing.Point(206, 98);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(1450, 873);
            this.panelDraw.TabIndex = 1;
            // 
            // pictureBoxDraw
            // 
            this.pictureBoxDraw.Location = new System.Drawing.Point(3, 0);
            this.pictureBoxDraw.Name = "pictureBoxDraw";
            this.pictureBoxDraw.Size = new System.Drawing.Size(1444, 866);
            this.pictureBoxDraw.TabIndex = 0;
            this.pictureBoxDraw.TabStop = false;
            this.pictureBoxDraw.Click += new System.EventHandler(this.pictureBoxDraw_Click);
            this.pictureBoxDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxDraw_Paint);
            this.pictureBoxDraw.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseDoubleClick);
            this.pictureBoxDraw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseDown);
            this.pictureBoxDraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseMove);
            this.pictureBoxDraw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseUp);
            this.pictureBoxDraw.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseWheel);
            // 
            // panelDebugInfo
            // 
            this.panelDebugInfo.Controls.Add(this.labelDebugInfo);
            this.panelDebugInfo.Location = new System.Drawing.Point(0, 44);
            this.panelDebugInfo.Name = "panelDebugInfo";
            this.panelDebugInfo.Size = new System.Drawing.Size(1653, 51);
            this.panelDebugInfo.TabIndex = 2;
            // 
            // labelDebugInfo
            // 
            this.labelDebugInfo.AutoSize = true;
            this.labelDebugInfo.Location = new System.Drawing.Point(3, 10);
            this.labelDebugInfo.Name = "labelDebugInfo";
            this.labelDebugInfo.Size = new System.Drawing.Size(86, 32);
            this.labelDebugInfo.TabIndex = 0;
            this.labelDebugInfo.Text = "Debug";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonDeleteTag);
            this.panel1.Controls.Add(this.buttonAddCar);
            this.panel1.Controls.Add(this.checkBoxShowAxis2);
            this.panel1.Controls.Add(this.checkBoxShowAxis1);
            this.panel1.Location = new System.Drawing.Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 867);
            this.panel1.TabIndex = 4;
            // 
            // buttonDeleteTag
            // 
            this.buttonDeleteTag.Location = new System.Drawing.Point(22, 183);
            this.buttonDeleteTag.Name = "buttonDeleteTag";
            this.buttonDeleteTag.Size = new System.Drawing.Size(150, 46);
            this.buttonDeleteTag.TabIndex = 4;
            this.buttonDeleteTag.Text = "Delete Tag";
            this.buttonDeleteTag.UseVisualStyleBackColor = true;
            this.buttonDeleteTag.Click += new System.EventHandler(this.buttonDeleteTag_Click);
            // 
            // buttonAddCar
            // 
            this.buttonAddCar.Location = new System.Drawing.Point(22, 118);
            this.buttonAddCar.Name = "buttonAddCar";
            this.buttonAddCar.Size = new System.Drawing.Size(150, 46);
            this.buttonAddCar.TabIndex = 3;
            this.buttonAddCar.Text = "Add Car";
            this.buttonAddCar.UseVisualStyleBackColor = true;
            this.buttonAddCar.Click += new System.EventHandler(this.buttonAddCar_Click);
            // 
            // checkBoxShowAxis2
            // 
            this.checkBoxShowAxis2.AutoSize = true;
            this.checkBoxShowAxis2.Location = new System.Drawing.Point(22, 62);
            this.checkBoxShowAxis2.Name = "checkBoxShowAxis2";
            this.checkBoxShowAxis2.Size = new System.Drawing.Size(173, 36);
            this.checkBoxShowAxis2.TabIndex = 2;
            this.checkBoxShowAxis2.Text = "Show Axis 2";
            this.checkBoxShowAxis2.UseVisualStyleBackColor = true;
            this.checkBoxShowAxis2.CheckedChanged += new System.EventHandler(this.checkBoxShowAxis2_CheckedChanged);
            // 
            // checkBoxShowAxis1
            // 
            this.checkBoxShowAxis1.AutoSize = true;
            this.checkBoxShowAxis1.Location = new System.Drawing.Point(22, 20);
            this.checkBoxShowAxis1.Name = "checkBoxShowAxis1";
            this.checkBoxShowAxis1.Size = new System.Drawing.Size(173, 36);
            this.checkBoxShowAxis1.TabIndex = 1;
            this.checkBoxShowAxis1.Text = "Show Axis 1";
            this.checkBoxShowAxis1.UseVisualStyleBackColor = true;
            this.checkBoxShowAxis1.CheckedChanged += new System.EventHandler(this.checkBoxShowAxis1_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1656, 970);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelDebugInfo);
            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Car Drive Simulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelDraw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).EndInit();
            this.panelDebugInfo.ResumeLayout(false);
            this.panelDebugInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_New;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Save;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SaveAs;
        private System.Windows.Forms.Panel panelDraw;
        private System.Windows.Forms.PictureBox pictureBoxDraw;
        private System.Windows.Forms.Panel panelDebugInfo;
        private System.Windows.Forms.Label labelDebugInfo;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Edit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxShowAxis2;
        private System.Windows.Forms.CheckBox checkBoxShowAxis1;
        private System.Windows.Forms.Button buttonAddCar;
        private System.Windows.Forms.Button buttonDeleteTag;
    }
}

