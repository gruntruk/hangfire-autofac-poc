using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Common;
using Hangfire.Server;

namespace HangfireAutofacDemo.Jobs
{
    public class TenantJobAttribute : JobFilterAttribute, IServerFilter
    {
        public void OnPerforming(PerformingContext filterContext)
        {
            if (filterContext.BackgroundJob.Job.Type != typeof(CountJob))
            {
                return;
            }
            QueryStringTenantIdentificationStrategy.CurrentTenantId.Value =
                (string) filterContext.BackgroundJob.Job.Args.First();
        }

        public void OnPerformed(PerformedContext filterContext)
        {
        }
    }
}
