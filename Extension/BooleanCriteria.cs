using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Extension
{
    class BooleanCriteria : Criteria
    {
        bool value;
        public BooleanCriteria(bool value)
        {
            this.value = value;
        }

        public override bool MeetCriteria(int x, int y)
        {
            return value;
        }
    }
}
