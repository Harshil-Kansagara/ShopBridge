using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShopBridge.Test
{
    [CollectionDefinition("Register Dependency")]
    public class BootstrapFixture : ICollectionFixture<Bootstrap>
    {

    }
}
