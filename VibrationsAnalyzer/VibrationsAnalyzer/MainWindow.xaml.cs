using VibrationAnalyzer.Classes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Wpf;
using System.Linq;

namespace VibrationAnalyzer
{
    public partial class MainWindow : Window
    {
        /**************************************************/
        // Constants
        const int MAX_NODES = 50; // Max number of elements in the graph

        /**************************************************/
        // Properties
        static SerialPort SPort;
        static bool IsConnected = false;
        static List<SensorData> SensorDataList = new List<SensorData>();
        public static MainWindow Instance { get; private set; }


        /**************************************************/
        // Methods
        void ConnectToPort()
        {
            string[] ports = SerialPort.GetPortNames();

            if (!IsConnected && ports.Length > 0)
            {
                SPort = new SerialPort(ports[0]);

                SPort.BaudRate = 9600;
                SPort.Parity = Parity.None;
                SPort.StopBits = StopBits.One;
                SPort.DataBits = 8;
                SPort.Handshake = Handshake.None;
                SPort.RtsEnable = true;

                SPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                SPort.Open();
                IsConnected = true;
            }
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            List<byte> data = new List<byte>();
            List<float> realData = new List<float>();
            SensorData tempData = new SensorData();
            bool readData = false;

            // We create a list of bytes that we will convert and process
            for (int i = 0; i < 40; i++) data.Add((byte)sp.ReadByte());

            for (int i = 0; i < data.Count; i += readData ? 4 : 1)
            {
                float temp = SubArray2Float(data.ToArray(), i);

                if (temp == 2.020202f && !readData) // Delimiter
                {
                    int size = (int)SubArray2Float(data.ToArray(), i + 4);

                    if (SubArray2Float(data.ToArray(), i + (size - 4)) == 2.020202f)
                        readData = true;
                }

                // We add the data until we find a delimiter again
                if (readData && temp != 2.020202f) realData.Add(temp);
            }

            // All received data:
            tempData.Accelerometer.X = realData[1];
            tempData.Accelerometer.Y = realData[2];
            tempData.Accelerometer.Z = realData[3];

            tempData.Gyroscope.X = realData[4];
            tempData.Gyroscope.Y = realData[5];
            tempData.Gyroscope.Z = realData[6];

            tempData.Temperature = realData[7];

            // Pitch:
            tempData.Angles.X = (float)((Math.Atan2(realData[2], realData[3]) * 57.295779) + (realData[5] * 0.1));

            // Roll:
            tempData.Angles.Y = (float)((Math.Atan2(realData[1], Math.Sqrt(Math.Pow(realData[2], 2) + Math.Pow(realData[3], 2))) * 57.295779) + (realData[4] * 0.1));

            // Yaw:
            tempData.Angles.Z = (float)(realData[6] * 0.1);

            SensorDataList.Add(tempData);

            // We will only have MAX_NODES quantity of elements in each graph
            // This will make it faster to process and render the information
            if (SensorDataList.Count >= MAX_NODES) SensorDataList.RemoveAt(0);

            RenderReceivedData();
        }

        private static void RenderReceivedData()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                // We get the reference to the UI elements
                var chtPosition = (CartesianChart)Instance.FindName("chtPosition");
                var chtAcceleration = (CartesianChart)Instance.FindName("chtAcceleration");

                var txtPosition = (TextBlock)Instance.FindName("txtPosition");
                var txtAcceleration = (TextBlock)Instance.FindName("txtAcceleration");

                // We create the graphs data, and we set the graph style
                var positionSeries = new LineSeries()
                {
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    Values = new LiveCharts.ChartValues<float>(SensorDataList.Select(data => data.Accelerometer.Y * 10).ToList())
                };
                var accelerationSeries = new LineSeries()
                {
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    Values = new LiveCharts.ChartValues<float>(SensorDataList.Select(data => data.Accelerometer.Y).ToList())
                };

                // Assign the current accelerometer value to the UI
                txtPosition.Text = Math.Round(SensorDataList[SensorDataList.Count - 1].Accelerometer.Y * 10, 5).ToString() + " cm";
                txtAcceleration.Text = Math.Round(SensorDataList[SensorDataList.Count - 1].Accelerometer.Y, 5).ToString() + " m/s^2";

                // Remove the olda graph data
                chtPosition.Series.Clear();
                chtAcceleration.Series.Clear();

                // Sets the new graph data
                chtPosition.Series.Add(positionSeries);
                chtAcceleration.Series.Add(accelerationSeries);
            }));
        }

        // This function converts from 4 uint packs to a single float variable
        private static float SubArray2Float(byte[] array, int index)
        {
            byte[] num = new byte[4];
            if (index + 4 <= array.Length && index >= 0) Array.Copy(array, index, num, 0, 4);

            return BitConverter.ToSingle(num, 0);
        }

        /**************************************************/
        // Events
        public MainWindow()
        {
            // We initialize the timer which will search for a new serial port connection every 5 seconds
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);
            var timer = new Timer((e) =>
            {
                ConnectToPort();
            }, null, startTimeSpan, periodTimeSpan);

            InitializeComponent();

            Instance = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // We close the serial port connection
            SPort.Close();
        }
    }
}
