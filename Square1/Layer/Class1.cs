using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1.Layer
{
    class RearLayer : ILayer
    {
        string code;
        public bool RotatableAt(int step)
        {
            return code[step % 12] != 3 && code[(step + 6) % 12] != 3;
        }

        public void RotateAt(int step, RearLayer associate)
        {
            throw new NotImplementedException();
        }

        public void Shift(int step)
        {
            //if (step > 0) code = code.Substring(step) + code.Substring(0, code.Length - step);
            //else code = code.Substring(code.Length - step) + code.Substring(0,)
        }
    }
}
