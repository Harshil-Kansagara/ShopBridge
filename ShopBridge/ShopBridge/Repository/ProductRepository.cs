using Microsoft.EntityFrameworkCore;
using ShopBridge.ApplicationClass;
using ShopBridge.CustomException;
using ShopBridge.Data;
using ShopBridge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region Private Variables
        private readonly IDataRepository _dataRepsoitory;
        #endregion

        #region Constructor
        public ProductRepository(IDataRepository dataRepository)
        {
            _dataRepsoitory = dataRepository;
        }
        #endregion

        public async Task<Guid> SaveProductAsync(ProductAC productAC)
        {
            if (productAC.Name == null)
            {
                throw new ValidationException("Product name is required");
            }

            if (productAC.Price == null)
            {
                throw new ValidationException("Product price can't be 0");
            }

            Product product = new Product()
            {
                Description = productAC.Description,
                Name = productAC.Name,
                Price = Int32.Parse(productAC.Price)
            };
            using (await _dataRepsoitory.BeginTransactionAsync())
            {
                await _dataRepsoitory.AddAsync<Product>(product);
                await _dataRepsoitory.SaveChangesAsync();
                _dataRepsoitory.CommitTransaction();
                return product.Id;
            }
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            if (productId.Equals(Guid.Empty))
            {
                throw new ValidationException("Product Id can't be empty");
            }
            var product = await _dataRepsoitory.SingleOrDefaultAsync<Product>(x => x.Id == productId);
            _dataRepsoitory.Remove<Product>(product);
            await _dataRepsoitory.SaveChangesAsync();
        }

        public async Task<ProductAC> UpdateProductAsync(Guid productId, ProductAC productAC)
        {
            if (productId.Equals(Guid.Empty))
            {
                throw new ValidationException("Product Id can't be empty");
            }
            var product = await _dataRepsoitory.SingleOrDefaultAsync<Product>(x => x.Id == productId);
            product.Name = productAC.Name;
            product.Description = productAC.Description;
            product.Price = Int32.Parse(productAC.Price);
            using (await _dataRepsoitory.BeginTransactionAsync())
            {
                _dataRepsoitory.Update<Product>(product);
                await _dataRepsoitory.SaveChangesAsync();
                _dataRepsoitory.CommitTransaction();
            }
            productAC.Id = product.Id;
            return productAC;
        }

        public async Task<ProductAC> GetProductAsync(Guid productId)
        {
            if (productId.Equals(Guid.Empty))
            {
                throw new ValidationException("Product Id can't be empty");
            }

            var product = await _dataRepsoitory.SingleOrDefaultAsync<Product>(x => x.Id == productId);
            if (product == null)
            {
                throw new ValidationException("No product found");
            }
            return new ProductAC
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString()
            };
        }

        public async Task<PagedProductAC> GetAllProductsAsync(FilterModelAC filterModel)
        {
            PagedProductAC pagedProductAC = new PagedProductAC();
            List<Product> products = await _dataRepsoitory.GetAll<Product>().ToListAsync();
            if (!products.Any())
            {
                pagedProductAC.Products = new List<ProductAC>();
                return pagedProductAC;
            }
            pagedProductAC.TotalProductsCount = products.Count;

            if (filterModel != null)
            {
                if (filterModel.Filters != null && filterModel.Filters.Any())
                {
                    products = FilterProductList(products, filterModel.Filters);
                    pagedProductAC.TotalProductsCount = products.Count;
                }
                if (filterModel.PageNo != null && filterModel.PageRecordCount != null)
                {
                    products = PaginationProductList(products, filterModel.PageNo.Value, filterModel.PageRecordCount.Value);
                }
            }

            //If no any loan found after filtering
            if (!products.Any())
            {
                pagedProductAC.Products = new List<ProductAC>();
                return pagedProductAC;
            }

            List<ProductAC> productACs = new List<ProductAC>();
            foreach (var product in products)
            {
                productACs.Add(new ProductAC
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.ToString()
                });
            }
            pagedProductAC.Products = productACs;
            return pagedProductAC;
        }

        private List<Product> PaginationProductList(List<Product> products, int pageNo, int recordPerPageCount){
            int recordToSkip = (Math.Abs(pageNo) - 1) * Math.Abs(recordPerPageCount);
            return products.Skip(recordToSkip).Take(Math.Abs(recordPerPageCount)).ToList();
        }


        private List<Product> FilterProductList(List<Product> products, List<FilterAC> filters)
        {
            foreach (var filter in filters)
            {
                if (filter.Field.Trim().ToLowerInvariant().Equals("name") && filter.Operator.Trim().ToLowerInvariant().Equals("="))
                {
                    products = products.Where(x => x.Name == filter.Value).ToList();
                }
                if (filter.Field.Trim().ToLowerInvariant().Equals("price") && filter.Operator.Trim().ToLowerInvariant().Equals("="))
                {
                    products = products.Where(x => x.Price == Int32.Parse(filter.Value)).ToList();
                }
            }
            return products;
        }
    }
}
