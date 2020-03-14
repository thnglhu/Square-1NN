using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Extension
{
    abstract class Criteria
    {
        public abstract bool MeetCriteria(int x, int y);
        public static Criteria operator|(Criteria first, Criteria second)
        {
            return new OrCriteria(first, second);
        }
        //public static Criteria operator&(Criteria first, Criteria second)
        //{
        //    return new AndCriteria(first, second);
        //}
    }    
}
