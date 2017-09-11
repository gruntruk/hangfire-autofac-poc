using System;
using System.Diagnostics;

namespace HangfireAutofacDemo.Services
{
    public class OtherService
    {
        private readonly CountService _countService;

        public OtherService(CountService countService)
        {
            _countService = countService;
        }

        public void DoSomething()
        {
            Debug.WriteLine($"Count from OtherService is now {_countService.Next()}");
        }
    }
}
