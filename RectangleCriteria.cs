using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN
{
    class RectangleCriteria : ICriteria
    {
        int originX, originY, sizeX, sizeY;
        public RectangleCriteria(int originX, int originY, int sizeX, int sizeY)
        {
            this.originX = originX;
            this.originY = originY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }
        public bool MeetCriteria(int x, int y)
        {
            return x >= originX && y >= originY && x <= originX + sizeX && y <= originY + sizeY;
        }
    }
}
