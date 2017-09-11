using System.Threading;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HangfireAutofacDemo
{
    public class QueryStringTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public static AsyncLocal<string> CurrentTenantId { get; } = new AsyncLocal<string>();

        public QueryStringTenantIdentificationStrategy(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool TryIdentifyTenant(out object tenantId)
        {
            if (_contextAccessor.HttpContext == null)
            {
                tenantId = CurrentTenantId.Value;
                return tenantId != null;
            }

            if (_contextAccessor.HttpContext.Request.Query.TryGetValue("tenant", out var values))
            {
                tenantId = values[0];
                return true;
            }

            tenantId = null;
            return false;
        }
    }
}