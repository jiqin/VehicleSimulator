
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
            this.checkBoxShowCarGuidelineWheel_4 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineWheel_3 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineWheel_2 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineWheel_1 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineBody_4 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineBody_3 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineBody_2 = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineBody_1 = new System.Windows.Forms.CheckBox();
            this.checkBoxRecordWheelTrace = new System.Windows.Forms.CheckBox();
            this.checkBoxRecordBodyTrace = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineWheel = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCarGuidelineBody = new System.Windows.Forms.CheckBox();
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
            this.pictureBoxDraw.Location = new System.Drawing.Point(143, 0);
            this.pictureBoxDraw.Name = "pictureBoxDraw";
            this.pictureBoxDraw.Size = new System.Drawing.Size(1304, 866);
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
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineWheel_4);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineWheel_3);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineWheel_2);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineWheel_1);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineBody_4);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineBody_3);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineBody_2);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineBody_1);
            this.panel1.Controls.Add(this.checkBoxRecordWheelTrace);
            this.panel1.Controls.Add(this.checkBoxRecordBodyTrace);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineWheel);
            this.panel1.Controls.Add(this.checkBoxShowCarGuidelineBody);
            this.panel1.Controls.Add(this.buttonDeleteTag);
            this.panel1.Controls.Add(this.buttonAddCar);
            this.panel1.Controls.Add(this.checkBoxShowAxis2);
            this.panel1.Controls.Add(this.checkBoxShowAxis1);
            this.panel1.Location = new System.Drawing.Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 867);
            this.panel1.TabIndex = 4;
            // 
            // checkBoxShowCarGuidelineWheel_4
            // 
            this.checkBoxShowCarGuidelineWheel_4.AutoSize = true;
            this.checkBoxShowCarGuidelineWheel_4.Location = new System.Drawing.Point(200, 220);
            this.checkBoxShowCarGuidelineWheel_4.Name = "checkBoxShowCarGuidelineWheel_4";
            this.checkBoxShowCarGuidelineWheel_4.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineWheel_4.TabIndex = 17;
            this.checkBoxShowCarGuidelineWheel_4.Text = "4";
            this.checkBoxShowCarGuidelineWheel_4.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineWheel_4.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineWheel_4_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineWheel_3
            // 
            this.checkBoxShowCarGuidelineWheel_3.AutoSize = true;
            this.checkBoxShowCarGuidelineWheel_3.Location = new System.Drawing.Point(140, 220);
            this.checkBoxShowCarGuidelineWheel_3.Name = "checkBoxShowCarGuidelineWheel_3";
            this.checkBoxShowCarGuidelineWheel_3.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineWheel_3.TabIndex = 16;
            this.checkBoxShowCarGuidelineWheel_3.Text = "3";
            this.checkBoxShowCarGuidelineWheel_3.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineWheel_3.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineWheel_3_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineWheel_2
            // 
            this.checkBoxShowCarGuidelineWheel_2.AutoSize = true;
            this.checkBoxShowCarGuidelineWheel_2.Location = new System.Drawing.Point(80, 220);
            this.checkBoxShowCarGuidelineWheel_2.Name = "checkBoxShowCarGuidelineWheel_2";
            this.checkBoxShowCarGuidelineWheel_2.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineWheel_2.TabIndex = 15;
            this.checkBoxShowCarGuidelineWheel_2.Text = "2";
            this.checkBoxShowCarGuidelineWheel_2.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineWheel_2.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineWheel_2_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineWheel_1
            // 
            this.checkBoxShowCarGuidelineWheel_1.AutoSize = true;
            this.checkBoxShowCarGuidelineWheel_1.Location = new System.Drawing.Point(20, 220);
            this.checkBoxShowCarGuidelineWheel_1.Name = "checkBoxShowCarGuidelineWheel_1";
            this.checkBoxShowCarGuidelineWheel_1.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineWheel_1.TabIndex = 14;
            this.checkBoxShowCarGuidelineWheel_1.Text = "1";
            this.checkBoxShowCarGuidelineWheel_1.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineWheel_1.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineWheel_1_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineBody_4
            // 
            this.checkBoxShowCarGuidelineBody_4.AutoSize = true;
            this.checkBoxShowCarGuidelineBody_4.Location = new System.Drawing.Point(200, 140);
            this.checkBoxShowCarGuidelineBody_4.Name = "checkBoxShowCarGuidelineBody_4";
            this.checkBoxShowCarGuidelineBody_4.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineBody_4.TabIndex = 13;
            this.checkBoxShowCarGuidelineBody_4.Text = "4";
            this.checkBoxShowCarGuidelineBody_4.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineBody_4.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineBody_4_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineBody_3
            // 
            this.checkBoxShowCarGuidelineBody_3.AutoSize = true;
            this.checkBoxShowCarGuidelineBody_3.Location = new System.Drawing.Point(140, 140);
            this.checkBoxShowCarGuidelineBody_3.Name = "checkBoxShowCarGuidelineBody_3";
            this.checkBoxShowCarGuidelineBody_3.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineBody_3.TabIndex = 12;
            this.checkBoxShowCarGuidelineBody_3.Text = "3";
            this.checkBoxShowCarGuidelineBody_3.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineBody_3.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineBody_3_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineBody_2
            // 
            this.checkBoxShowCarGuidelineBody_2.AutoSize = true;
            this.checkBoxShowCarGuidelineBody_2.Location = new System.Drawing.Point(80, 140);
            this.checkBoxShowCarGuidelineBody_2.Name = "checkBoxShowCarGuidelineBody_2";
            this.checkBoxShowCarGuidelineBody_2.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineBody_2.TabIndex = 11;
            this.checkBoxShowCarGuidelineBody_2.Text = "2";
            this.checkBoxShowCarGuidelineBody_2.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineBody_2.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineBody_2_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineBody_1
            // 
            this.checkBoxShowCarGuidelineBody_1.AutoSize = true;
            this.checkBoxShowCarGuidelineBody_1.Location = new System.Drawing.Point(20, 140);
            this.checkBoxShowCarGuidelineBody_1.Name = "checkBoxShowCarGuidelineBody_1";
            this.checkBoxShowCarGuidelineBody_1.Size = new System.Drawing.Size(59, 36);
            this.checkBoxShowCarGuidelineBody_1.TabIndex = 10;
            this.checkBoxShowCarGuidelineBody_1.Text = "1";
            this.checkBoxShowCarGuidelineBody_1.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineBody_1.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineBody_1_CheckedChanged);
            // 
            // checkBoxRecordWheelTrace
            // 
            this.checkBoxRecordWheelTrace.AutoSize = true;
            this.checkBoxRecordWheelTrace.Location = new System.Drawing.Point(20, 300);
            this.checkBoxRecordWheelTrace.Name = "checkBoxRecordWheelTrace";
            this.checkBoxRecordWheelTrace.Size = new System.Drawing.Size(256, 36);
            this.checkBoxRecordWheelTrace.TabIndex = 9;
            this.checkBoxRecordWheelTrace.Text = "Record Wheel Trace";
            this.checkBoxRecordWheelTrace.UseVisualStyleBackColor = true;
            this.checkBoxRecordWheelTrace.CheckedChanged += new System.EventHandler(this.checkBoxRecordWheelTrace_CheckedChanged);
            // 
            // checkBoxRecordBodyTrace
            // 
            this.checkBoxRecordBodyTrace.AutoSize = true;
            this.checkBoxRecordBodyTrace.Location = new System.Drawing.Point(20, 260);
            this.checkBoxRecordBodyTrace.Name = "checkBoxRecordBodyTrace";
            this.checkBoxRecordBodyTrace.Size = new System.Drawing.Size(242, 36);
            this.checkBoxRecordBodyTrace.TabIndex = 8;
            this.checkBoxRecordBodyTrace.Text = "Record Body Trace";
            this.checkBoxRecordBodyTrace.UseVisualStyleBackColor = true;
            this.checkBoxRecordBodyTrace.CheckedChanged += new System.EventHandler(this.checkBoxRecordBodyTrace_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineWheel
            // 
            this.checkBoxShowCarGuidelineWheel.AutoSize = true;
            this.checkBoxShowCarGuidelineWheel.Location = new System.Drawing.Point(20, 180);
            this.checkBoxShowCarGuidelineWheel.Name = "checkBoxShowCarGuidelineWheel";
            this.checkBoxShowCarGuidelineWheel.Size = new System.Drawing.Size(291, 36);
            this.checkBoxShowCarGuidelineWheel.TabIndex = 6;
            this.checkBoxShowCarGuidelineWheel.Text = "Car LeadingLine Wheel";
            this.checkBoxShowCarGuidelineWheel.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineWheel.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineWheel_CheckedChanged);
            // 
            // checkBoxShowCarGuidelineBody
            // 
            this.checkBoxShowCarGuidelineBody.AutoSize = true;
            this.checkBoxShowCarGuidelineBody.Location = new System.Drawing.Point(20, 100);
            this.checkBoxShowCarGuidelineBody.Name = "checkBoxShowCarGuidelineBody";
            this.checkBoxShowCarGuidelineBody.Size = new System.Drawing.Size(277, 36);
            this.checkBoxShowCarGuidelineBody.TabIndex = 5;
            this.checkBoxShowCarGuidelineBody.Text = "Car LeadingLine Body";
            this.checkBoxShowCarGuidelineBody.UseVisualStyleBackColor = true;
            this.checkBoxShowCarGuidelineBody.CheckedChanged += new System.EventHandler(this.checkBoxShowCarGuidelineBody_CheckedChanged);
            // 
            // buttonDeleteTag
            // 
            this.buttonDeleteTag.Location = new System.Drawing.Point(50, 514);
            this.buttonDeleteTag.Name = "buttonDeleteTag";
            this.buttonDeleteTag.Size = new System.Drawing.Size(150, 46);
            this.buttonDeleteTag.TabIndex = 4;
            this.buttonDeleteTag.Text = "Delete Tag";
            this.buttonDeleteTag.UseVisualStyleBackColor = true;
            this.buttonDeleteTag.Click += new System.EventHandler(this.buttonDeleteTag_Click);
            // 
            // buttonAddCar
            // 
            this.buttonAddCar.Location = new System.Drawing.Point(50, 462);
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
            this.checkBoxShowAxis2.Location = new System.Drawing.Point(20, 60);
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
            this.checkBoxShowAxis1.Location = new System.Drawing.Point(20, 20);
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
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineBody;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineWheel;
        private System.Windows.Forms.CheckBox checkBoxRecordBodyTrace;
        private System.Windows.Forms.CheckBox checkBoxRecordWheelTrace;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineBody_1;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineBody_4;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineBody_3;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineBody_2;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineWheel_4;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineWheel_3;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineWheel_2;
        private System.Windows.Forms.CheckBox checkBoxShowCarGuidelineWheel_1;
    }
}

