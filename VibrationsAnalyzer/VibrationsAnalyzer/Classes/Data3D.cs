using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationAnalyzer.Classes
{
    public class Data3D
    {
        /**************************************************/
        // Properties
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /**************************************************/
        // Methods
        public Data3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Data3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
