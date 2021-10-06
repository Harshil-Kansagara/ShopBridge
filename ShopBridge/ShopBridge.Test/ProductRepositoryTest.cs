using Moq;
using ShopBridge.Data;
using ShopBridge.Repository;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ShopBridge.ApplicationClass;
using ShopBridge.CustomException;
using ShopBridge.Model;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;

namespace ShopBridge.Test
{
    [Collection("Register Dependency")]
    public class ProductRepositoryTest : BaseTest
    {
        #region Private Variables
        private readonly Mock<IDataRepository> _dataRepositoryMock;
        private readonly IProductRepository _productRepository;
        #endregion

        #region Constructor
        public ProductRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _dataRepositoryMock = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _productRepository = bootstrap.ServiceProvider.GetService<IProductRepository>();
            _dataRepositoryMock.Reset();
        }
        #endregion

        [Fact]
        public async Task SaveProductAsync_ProductNameIsNull_VerifyThrowsValidationException()
        {
            // Arrange
            var productAC = new ProductAC
            {
                Name = null,
                Description = "Xyz Description",
                Price = "300"
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.SaveProductAsync(productAC));

        }

        [Fact]
        public async Task SaveProductAsync_ProductPriceIsNull_VerifyThrowsValidationException()
        {
            // Arrange
            var productAC = new ProductAC
            {
                Name = "Xyz",
                Description = "Xyz Description",
                Price = null
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.SaveProductAsync(productAC));

        }

        [Fact]
        public async Task SaveProductAsync_SaveProduct_VerifyCount()
        {
            // Arrange
            var productAC = new ProductAC
            {
                Name = "Xyz",
                Description = "Xyz Description",
                Price = "300"
            };
            _dataRepositoryMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.FromResult(It.IsAny<IDbContextTransaction>()));

            // Act
            var productId = await _productRepository.SaveProductAsync(productAC);

            // Assert
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            _dataRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _dataRepositoryMock.Verify(x => x.CommitTransaction(), Times.Once);

        }

        [Fact]
        public async Task DeleteProductAsync_ProductIdIsEmpty_VerifyThrowsValidationException()
        {
            // Arrange
            var productId = Guid.Empty;

            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.DeleteProductAsync(productId));

        }

        [Fact]
        public async Task DeleteProductAsync_ProductIdIsNotEmpty_VerifyCount()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            await _productRepository.DeleteProductAsync(productId);

            // Assert
            _dataRepositoryMock.Verify(x => x.Remove(It.IsAny<Product>()),Times.Once);
            _dataRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task UpdateProductAsync_ProductIdIsEmpty_VerifyThrowsValidationException()
        {
            // Arrange
            var productId = Guid.Empty;
            var productAC = new ProductAC
            {
                Name = "XYZ",
                Description = "XYZ",
                Id = productId,
                Price = "300"
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.UpdateProductAsync(productId,productAC));

        }

        [Fact]
        public async Task UpdateProductAsync_VerifyCount()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productAC = new ProductAC
            {
                Name = "XYZ",
                Description = "XYZ 123",
                Id = productId,
                Price = "300"
            };
            var product = new Product
            {
                Name = "XYZ",
                Description = "XYZ",
                Id = productId,
                Price = 300
            };
            _dataRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.FromResult(product));

            // Act
            var result = await _productRepository.UpdateProductAsync(productId, productAC);

            // Assert
            _dataRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            _dataRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.Equal(productAC.Description, result.Description);
        }

        [Fact]
        public async Task GetProductAsync_ProductIdIsEmpty_VerifyThrowsValidationException()
        {
            // Arrange
            var productId = Guid.Empty;
            
            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.GetProductAsync(productId));
        }

        [Fact]
        public async Task GetProductAsync_NoProductFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            Product product = null;
            _dataRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.FromResult(product));

            // Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _productRepository.GetProductAsync(productId));
        }

        [Fact]
        public async Task GetProductAsync_ProductFound_AssertEqual()
        {
            // Arrange
            var productId = Guid.NewGuid();
            Product product = new Product()
            {
                Id = productId,
                Name = "XYZ",
                Description = "XYZ Description",
                Price = 300
            };
            _dataRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.FromResult(product));

            // Act
            var result = await _productRepository.GetProductAsync(productId);

            // Assert
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Description, result.Description);
        }

        [Fact]
        public async Task GetAllProductsAsync_NoProductsFound()
        {
            // Arrange
            List<Product> products = new List<Product>();
            _dataRepositoryMock.Setup(x => x.GetAll<Product>()).Returns(products.AsQueryable().BuildMock().Object);
            var filterModel = new FilterModelAC();

            // Act
            var result = await _productRepository.GetAllProductsAsync(filterModel);

            // Assert
            Assert.Empty(result.Products);
        }

        [Fact]
        public async Task GetAllProductsAsync_ProductsFound_Pagination_AssertCount()
        {
            // Arrange
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 1 Description",
                    Name = "Product 1",
                    Price = 200
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 2 Description",
                    Name = "Product 2",
                    Price = 100
                }
            };
            var filterModel = new FilterModelAC()
            {
                PageNo = 1,
                PageRecordCount = 1
            };
            _dataRepositoryMock.Setup(x => x.GetAll<Product>()).Returns(products.AsQueryable().BuildMock().Object);

            // Act
            var result = await _productRepository.GetAllProductsAsync(filterModel);

            // Assert
            Assert.Single(result.Products);
        }

        [Fact]
        public async Task GetAllProductsAsync_ProductsFound_FilterByName_AssertCount()
        {
            // Arrange
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 1 Description",
                    Name = "Product 1",
                    Price = 200
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 2 Description",
                    Name = "Product 2",
                    Price = 100
                }
            };
            var filterModel = new FilterModelAC()
            {
                PageNo = 1,
                PageRecordCount = 1,
                Filter = "[{'field':'name','operator':' = ','value':'Product 1'}]"
            };
            _dataRepositoryMock.Setup(x => x.GetAll<Product>()).Returns(products.AsQueryable().BuildMock().Object);

            // Act
            var result = await _productRepository.GetAllProductsAsync(filterModel);

            // Assert
            Assert.Single(result.Products);
        }

        [Fact]
        public async Task GetAllProductsAsync_ProductsNotFound_FilterByName_AssertCount()
        {
            // Arrange
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 1 Description",
                    Name = "Product 1",
                    Price = 200
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Description = "Product 2 Description",
                    Name = "Product 2",
                    Price = 100
                }
            };
            var filterModel = new FilterModelAC()
            {
                PageNo = 1,
                PageRecordCount = 1,
                Filter = "[{'field':'name','operator':' = ','value':'Product 3'}]"
            };
            _dataRepositoryMock.Setup(x => x.GetAll<Product>()).Returns(products.AsQueryable().BuildMock().Object);

            // Act
            var result = await _productRepository.GetAllProductsAsync(filterModel);

            // Assert
            Assert.Empty(result.Products);
        }
    }
}
