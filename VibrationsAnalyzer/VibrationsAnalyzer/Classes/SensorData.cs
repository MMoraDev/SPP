using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationAnalyzer.Classes
{
    public class SensorData
    {
        /**************************************************/
        // Properties
        public Data3D Accelerometer { get; set; }
        public Data3D Gyroscope { get; set; }
        public Data3D Angles { get; set; }
        public Data3D AngleCompensations { get; set; }
        public float Temperature { get; set; }


        /**************************************************/
        // Methods
        public SensorData()
        {
            Accelerometer = new Data3D();
            Gyroscope = new Data3D();
            Angles = new Data3D();
            AngleCompensations = new Data3D();
            Temperature = 0;
        }
    }
}
