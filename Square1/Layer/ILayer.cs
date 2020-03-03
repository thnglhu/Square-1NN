using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1.Layer
{
    interface ILayer
    {
        void Shift(int step);
        bool RotatableAt(int step);
    }
}
