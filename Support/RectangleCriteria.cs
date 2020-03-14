namespace Square_1NN.Support
{
    class RectangleCriteria : Criteria
    {
        int originX, originY, sizeX, sizeY;
        public RectangleCriteria(int originX, int originY, int sizeX, int sizeY)
        {
            this.originX = originX;
            this.originY = originY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }
        public override bool MeetCriteria(int x, int y)
        {
            return x >= originX && y >= originY && x <= originX + sizeX && y <= originY + sizeY;
        }
    }
}
