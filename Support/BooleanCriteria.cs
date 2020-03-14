namespace Square_1NN.Support
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
