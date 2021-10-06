using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShopBridge.Data;
using ShopBridge.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Test
{
    public class Bootstrap
    {
        #region public variable
        public readonly IServiceProvider ServiceProvider;
        #endregion

        #region Constructor
        public Bootstrap()
        {
            var services = new ServiceCollection();

            #region Dependency Injection
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            #region Mocks
            var dataRepositoryMock = new Mock<IDataRepository>();
            services.AddSingleton(x => dataRepositoryMock);
            services.AddSingleton(x => dataRepositoryMock.Object);
            #endregion

            ServiceProvider = services.BuildServiceProvider();
        }
        #endregion
    }
}
