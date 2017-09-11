namespace HangfireAutofacDemo.Services
{
    public class CountService
    {
        private int _counter;

        public int Next()
        {
            _counter += 1;
            return _counter;
        }
    }
}
