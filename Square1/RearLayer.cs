using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1
{
    class RearLayer : LayerBase
    {
        protected RearLayer() : base() { }
        public RearLayer(string mask) : base(mask) { }

        public override bool Rotatable()
        {
            return Major[0] != '3' && Major[Major.Length - 1] != '1' && Minor[0] != '3' && Minor[Minor.Length - 1] != '1';
        }
        public override void Reverse(bool for_major)
        {
            base.Reverse(for_major);
            Fix(for_major);
        }
        public virtual void Fix(bool for_major = true)
        {
            string target = for_major ? Major : Minor;
            for (int index = 0; index < target.Length; index++)
            {
                
                if (target[index] == '3' && index + 1 == target.Length)
                {
                    target = target.Substring(0, index) + "1" + target.Substring(index + 1);
                }
                else if (target[index] == '1')
                {
                    target = target.Substring(0, index) + "3" + target.Substring(index + 1);
                }
                else if (target[index] == '3' && target[index + 1] == '1')
                {
                    target = target.Substring(0, index) + "13" + target.Substring(index + 2);
                    index++;
                }
            }
            if (for_major) Major = target;
            else Minor = target;
        }
        public List<int> GetAvailableMoves()
        {
            string full = Minor + Major + Minor + Major;
            int start = Minor.Length;
            List<int> result = new List<int>();
            for (int rotate = -5; rotate <= 6; rotate++)
            {
                if (full[start + rotate] != '3' && full[start + rotate + 6] != '3') result.Add(rotate);
            }
            return result;
        }

        public override LayerBase Clone()
        {
            return new RearLayer() { major = Major, minor = Minor };
        }
    }
}
