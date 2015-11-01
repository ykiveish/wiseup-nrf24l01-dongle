using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace read_arduino_sensor_data
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fSensorUI());
        }
    }
}
