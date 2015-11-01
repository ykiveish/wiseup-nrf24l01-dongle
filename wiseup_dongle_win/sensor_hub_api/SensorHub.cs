using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace sensor_hub_api
{
    public class ArduinoAPICommand
    {
        public byte Command { get; set; }
        public byte[] Data { get; set; }

        public ArduinoAPICommand(byte command, byte[] data)
        {
            this.Command = command;
            this.Data = data;
        }

        public byte[] GenerateCommand()
        {
            byte[] packet = new byte[17];
            packet[0] = this.Command;
            Buffer.BlockCopy(this.Data, 0, packet, 1, 16);
            return packet;
        }
    }

    public class SerialState
    {
        public enum SerialReadStates
        {
            MagicNumber,
            Command,
            Length,
            Data
        }

        public SerialReadStates state;

        public SerialState ()
        {
            state = SerialReadStates.MagicNumber;
        }
    }

    public interface IWUSerialPacket
    {
        byte Command { get; set; }
        char[] Data { get; set; }
        int DataIndex { get; set; }
    }

    public class WUSerialPacketRequest : IWUSerialPacket
    {
        public byte Command { get; set; }
        public char[] Data { get; set; }
        public int DataIndex { get; set; }
        public byte Length { get; set; }
    }

    public class WUSerialPacketResponse : IWUSerialPacket
    {
        public byte Command { get; set; }
        public char[] Data { get; set; }
        public int DataIndex { get; set; }
        public byte Length { get; set; }
    }

    public class Sensor
    {
        public byte Port { get; set; }
        public byte Type { get; set; }
        public byte Data { get; set; }

        public Sensor(byte port, byte type, byte data)
        {

        }
    }

    public class Device
    {
        public List<Sensor> _sensors = new List<Sensor>();
        public string Id { get; set; }
        public int SensorCount { get; set; }

        public void AddSensor(Sensor sensor)
        {
            this._sensors.Add(sensor);
        }
    }

    public class UART
    {
        public delegate void UARTDataHandler(WUSerialPacketResponse data);

        private SerialPort _serialCommPort = null;
        private bool _isWorking = true;
        private UARTDataHandler _dataArrivedHandler = null;

        public UART()
        {

        }

        public UART(ref SerialPort serial, UARTDataHandler callback)
        {
            this._serialCommPort = serial;
            this._dataArrivedHandler = callback;
        }

        public void UartHandler()
        {
            SerialState             checkState  = new SerialState();
            WUSerialPacketResponse  response    = new WUSerialPacketResponse();
            int                     data        = 0;

            response.DataIndex = 0;
            while (this._isWorking) 
            {
                if (this._serialCommPort.IsOpen == true) 
                {
                    if (_serialCommPort.BytesToRead > 0)
                    {
                        data = this._serialCommPort.ReadByte();
                        switch (checkState.state)
                        {
                            case SerialState.SerialReadStates.MagicNumber:
                                if ((byte)data == 0xAD)
                                {
                                    checkState.state = SerialState.SerialReadStates.Command;
                                }
                                break;
                            case SerialState.SerialReadStates.Command:
                                response.Command = (byte)data;
                                checkState.state = SerialState.SerialReadStates.Length;
                                break;
                            case SerialState.SerialReadStates.Length:
                                response.Length = (byte)data;
                                response.Data = new char[response.Length];
                                checkState.state = SerialState.SerialReadStates.Data;
                                break;
                            case SerialState.SerialReadStates.Data:
                                response.Data[response.DataIndex] = (char)data;
                                if (response.Length == response.DataIndex + 1)
                                {
                                    response.DataIndex = 0;
                                    Debug.Print("# [UART] { command: " + response.Command + ", data: " + response.Data.ToString());
                                    this._dataArrivedHandler(response);
                                }
                                else
                                {
                                    response.DataIndex++;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                Thread.Sleep(10);
            }
        }
    }

    public class SensorHub : IDisposable
    {
        public delegate void GetSensorDataEventHandler(string data);

        private static ManualResetEvent _sensorDataArrivedSignal = new ManualResetEvent(false);
        private SerialPort _serialCommPort = null;
        private string _sensorData = "";
        private bool _isExit = false;
        private Thread _uartWorker = null;
        private Thread _cloudRequestHandler = null;

        public bool IsDeviceConnected = false;

        private UART _uart = null;
        private Cloud _cloud = null;
        
        private void UARTDataArrived(WUSerialPacketResponse data)
        {
            switch (data.Command)
            {
                case 0x9: // UART_REQUEST_CHECK_DEVICE_ID
                    CloudRequest req = new CloudRequest();
                    req.UARTRequest = data;
                    this._cloud.AddRequest(req);
                    // TODO
                    // 1. HTTP request to server with device ID.
                    // 2. Add request to list of requests, delete it when response will be sent to the device.
                    break;
            }
        }

        private void _getSensorData(byte pin, GetSensorDataEventHandler callback)
        {
            byte[] data = new byte[16];
            ArduinoAPICommand cmd = new ArduinoAPICommand(0x1, data);
            byte[] msg = cmd.GenerateCommand();
            _serialCommPort.Write(msg, 0, msg.Length);
            _sensorDataArrivedSignal.WaitOne();

            if (_isExit == false)
                callback(_sensorData);
        }

        private void _setSensorData(byte pin, byte[] data)
        {
            ArduinoAPICommand cmd = new ArduinoAPICommand(0x2, data);
            byte[] msg = cmd.GenerateCommand();
            _serialCommPort.Write(msg, 0, msg.Length);
        }

        private bool _checkDevice()
        {
            SerialState checkState = new SerialState();
            byte[] msg = { 0xAD, 0x05, 0x0 };
            _serialCommPort.Write(msg, 0, msg.Length);
            Thread.Sleep(500);

            WUSerialPacketResponse response = new WUSerialPacketResponse();
            response.DataIndex = 0;
            int data = 0;
            while (_serialCommPort.BytesToRead > 0)
            {
                data = _serialCommPort.ReadByte();

                switch (checkState.state)
                {
                    case SerialState.SerialReadStates.MagicNumber:
                        if ((byte)data == 0xAD)
                        {
                            checkState.state = SerialState.SerialReadStates.Command;
                        }
                        break;
                    case SerialState.SerialReadStates.Command:
                        response.Command = (byte)data;
                        checkState.state = SerialState.SerialReadStates.Length;
                        break;
                    case SerialState.SerialReadStates.Length:
                        response.Length = (byte)data;
                        response.Data = new char[response.Length];
                        checkState.state = SerialState.SerialReadStates.Data;
                        break;
                    case SerialState.SerialReadStates.Data:
                        response.Data[response.DataIndex] = (char)data;
                        if (response.Length == response.DataIndex + 1)
                        {
                            if (response.Data[0] == 0xFA && response.Data[1] == 0xBA)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            response.DataIndex++;
                        }
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        public void Start ()
        {
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                _serialCommPort = new SerialPort(ports[i], 9600);
                try
                {
                    _serialCommPort.Open();
                }
                catch (Exception e)
                {

                }

                if (_serialCommPort.IsOpen == true) 
                {
                    if (this._checkDevice())
                    {
                        this.IsDeviceConnected = true;
                        this._uart = new UART(ref this._serialCommPort, this.UARTDataArrived);
                        this._uartWorker = new Thread(new ThreadStart(this._uart.UartHandler));
                        this._uartWorker.Start();
                        break;
                    }
                    else
                    {
                        _serialCommPort.Close();
                    }
                }
            }
        }

        public void Stop()
        {
            this._uartWorker.Abort();
            this._cloudRequestHandler.Abort();
            if (_serialCommPort.IsOpen == true)
            {
                _serialCommPort.Close();
            }
        }

        void SerialCommPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);
            SerialPort sp = (SerialPort)sender;
            _sensorData = sp.ReadExisting();
            _sensorDataArrivedSignal.Set();
        }

        public SensorHub()
        {
            this._cloud = new Cloud();
            this._cloudRequestHandler = new Thread(this._cloud.CloudHandler);
            this._cloudRequestHandler.Start();
        }

        public void GetSensorData(byte pinNumber, GetSensorDataEventHandler callback) 
        {
            Thread worker = new Thread(delegate()
            {
                _getSensorData(pinNumber, callback);
            });

            _sensorData = "";
            _sensorDataArrivedSignal.Reset();
            worker.Start();
        }

        public void SetSensorData(byte pinNumber, byte[] data)
        {
            Thread worker = new Thread(delegate()
            {
                _setSensorData(pinNumber, data);
            });

            worker.Start();
        }

        public void Exit()
        {
            _isExit = true;
            _sensorDataArrivedSignal.Set();
        }

        public void Dispose()
        {
            _serialCommPort.Close();
        }
    }
}
