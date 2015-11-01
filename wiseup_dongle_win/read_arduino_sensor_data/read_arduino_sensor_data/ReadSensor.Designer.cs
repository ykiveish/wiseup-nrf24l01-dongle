namespace read_arduino_sensor_data
{
    partial class fSensorUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetSensorData = new System.Windows.Forms.LinkLabel();
            this.lblSensorData = new System.Windows.Forms.Label();
            this.txtPinNumber = new System.Windows.Forms.TextBox();
            this.btnSetSensorData = new System.Windows.Forms.LinkLabel();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGetSensorData
            // 
            this.btnGetSensorData.AutoSize = true;
            this.btnGetSensorData.Location = new System.Drawing.Point(12, 111);
            this.btnGetSensorData.Name = "btnGetSensorData";
            this.btnGetSensorData.Size = new System.Drawing.Size(86, 13);
            this.btnGetSensorData.TabIndex = 0;
            this.btnGetSensorData.TabStop = true;
            this.btnGetSensorData.Text = "Get Sensor Data";
            this.btnGetSensorData.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnGetSensorData_LinkClicked);
            // 
            // lblSensorData
            // 
            this.lblSensorData.AutoSize = true;
            this.lblSensorData.Location = new System.Drawing.Point(70, 86);
            this.lblSensorData.Name = "lblSensorData";
            this.lblSensorData.Size = new System.Drawing.Size(27, 13);
            this.lblSensorData.TabIndex = 1;
            this.lblSensorData.Text = "N/A";
            // 
            // txtPinNumber
            // 
            this.txtPinNumber.Location = new System.Drawing.Point(80, 6);
            this.txtPinNumber.Name = "txtPinNumber";
            this.txtPinNumber.Size = new System.Drawing.Size(35, 20);
            this.txtPinNumber.TabIndex = 2;
            this.txtPinNumber.Text = "14";
            // 
            // btnSetSensorData
            // 
            this.btnSetSensorData.AutoSize = true;
            this.btnSetSensorData.Location = new System.Drawing.Point(12, 60);
            this.btnSetSensorData.Name = "btnSetSensorData";
            this.btnSetSensorData.Size = new System.Drawing.Size(79, 13);
            this.btnSetSensorData.TabIndex = 3;
            this.btnSetSensorData.TabStop = true;
            this.btnSetSensorData.Text = "SetSensorData";
            this.btnSetSensorData.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSetSensorData_LinkClicked);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(80, 32);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(35, 20);
            this.txtData.TabIndex = 2;
            this.txtData.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pin Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Write Data:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Read Data:";
            // 
            // fSensorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 203);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetSensorData);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.txtPinNumber);
            this.Controls.Add(this.lblSensorData);
            this.Controls.Add(this.btnGetSensorData);
            this.Name = "fSensorUI";
            this.Text = "ReadSensor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fSensorUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel btnGetSensorData;
        private System.Windows.Forms.Label lblSensorData;
        private System.Windows.Forms.TextBox txtPinNumber;
        private System.Windows.Forms.LinkLabel btnSetSensorData;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

