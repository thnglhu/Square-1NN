using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Extension
{
    class OrCriteria : ICriteria
    {
        ICriteria first, second;
        public OrCriteria(ICriteria first, ICriteria second)
        {
            this.first = first;
            this.second = second;
        }

        public bool MeetCriteria(int x, int y)
        {
            return first.MeetCriteria(x, y) || second.MeetCriteria(x, y);
        }
    }
}
