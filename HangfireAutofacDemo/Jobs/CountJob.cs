using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HangfireAutofacDemo.Services;
using Microsoft.Azure.KeyVault.Models;

namespace HangfireAutofacDemo.Jobs
{
    [TenantJob]
    public class CountJob
    {
        private readonly CountService _countService;
        private readonly OtherService _otherService;

        public CountJob(CountService countService, OtherService otherService)
        {
            _countService = countService;
            _otherService = otherService;
        }

        public void Execute(string tenantId)
        {
            Debug.WriteLine($"Executing: {_countService.Next()}");
            _otherService.DoSomething();
        }
    }
}
