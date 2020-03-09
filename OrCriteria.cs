using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    class OrCriteria : ICriteria
    {
        ICriteria criteria1, criteria2;
        public OrCriteria(ICriteria criteria1, ICriteria criteria2)
        {
            this.criteria1 = criteria1;
            this.criteria2 = criteria2;
        }

        public bool MeetCriteria(int x, int y)
        {
            return criteria1.MeetCriteria(x, y) || criteria2.MeetCriteria(x, y);
        }
    }
}
