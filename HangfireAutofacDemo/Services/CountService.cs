namespace HangfireAutofacDemo.Services
{
    public class CountService
    {
        private int _counter;

        public CountService(int seed = 0)
        {
            _counter = seed;
        }

        public int Next()
        {
            _counter += 1;
            return _counter;
        }
    }
}
