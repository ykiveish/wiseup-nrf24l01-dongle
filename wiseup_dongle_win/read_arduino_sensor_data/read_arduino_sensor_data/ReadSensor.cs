using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using sensor_hub_api;

namespace read_arduino_sensor_data
{
    public partial class fSensorUI : Form
    {
        SensorHub ArduinoAPI;
        public fSensorUI()
        {
            InitializeComponent();
            ArduinoAPI = new SensorHub();
            ArduinoAPI.Start();
        }

        private void btnGetSensorData_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ArduinoAPI.GetSensorData((byte)(int.Parse(txtPinNumber.Text)), _updateUI);
        }

        private void btnSetSensorData_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ArduinoAPI.SetSensorData((byte)(int.Parse(txtPinNumber.Text)), (byte)(int.Parse(txtData.Text)));
        }

        delegate void SetDataCallback(string data);
        private void _updateUI(string data)
        {
            if (lblSensorData.InvokeRequired)
            {
                SetDataCallback access = new SetDataCallback(_updateUI);

                if (this.IsHandleCreated == false)
                    return;

                this.Invoke(access, new object[] { data });
            }
            else
            {
                lblSensorData.Text = data;
            }
        }

        private void fSensorUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            ArduinoAPI.Exit();
        }

        
    }
}
