using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Test
{
    public class BaseTest : IDisposable
    {
        protected readonly IServiceScope _scope;

        public BaseTest(Bootstrap bootstrap)
        {
            _scope = bootstrap.ServiceProvider.CreateScope();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
