using ShopBridge.ApplicationClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Repository
{
    public interface IProductRepository
    {
        Task<ProductAC> GetProductAsync(Guid productId);

        Task<PagedProductAC> GetAllProductsAsync(FilterModelAC filterModel);

        Task<Guid> SaveProductAsync(ProductAC product);

        Task<ProductAC> UpdateProductAsync(Guid productId, ProductAC product);

        Task DeleteProductAsync(Guid productId);
    }
}
