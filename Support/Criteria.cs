namespace Square_1NN.Support
{
    abstract class Criteria
    {
        public abstract bool MeetCriteria(int x, int y);
        public static Criteria operator|(Criteria first, Criteria second)
        {
            return new OrCriteria(first, second);
        }
    }    
}
