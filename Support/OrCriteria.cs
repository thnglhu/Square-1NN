namespace Square_1NN.Support
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
