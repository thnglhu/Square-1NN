using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1.Layer
{
    class MiddleLayer : LayerBase
    {
        protected MiddleLayer() : base() { }
        public MiddleLayer(string mask) : base(mask) { }

        public override void Shift(bool _)
        {
            string temp = Major;
            Major = Minor;
            Minor = temp;
        }

        public override bool Rotatable()
        {
            return Major[0] == '4' || Major[0] == '2' && Minor[0] == '4' || Minor[0] == '2';
        }
        public override void Shift(int step)
        {
            if (step % 2 != 0) Shift(true);
        }

        public override LayerBase Clone()
        {
            return new MiddleLayer() { major = Major, minor = Minor };
        }
    }
}
