using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HangfireAutofacDemo.Services;
using Microsoft.Azure.KeyVault.Models;

namespace HangfireAutofacDemo.Jobs
{
    [MultitenantJob]
    public class CountJob
    {
        private readonly CountService _countService;

        public CountJob(CountService countService)
        {
            _countService = countService;
        }

        public void Execute(string tenantId)
        {
            Debug.WriteLine($"Executing: {_countService.Next()}");
        }
    }
}
