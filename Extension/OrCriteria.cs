﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Extension
{
    class OrCriteria : Criteria
    {
        Criteria first, second;
        public OrCriteria(Criteria first, Criteria second)
        {
            this.first = first;
            this.second = second;
        }

        public override bool MeetCriteria(int x, int y)
        {
            return first.MeetCriteria(x, y) || second.MeetCriteria(x, y);
        }
    }
}
