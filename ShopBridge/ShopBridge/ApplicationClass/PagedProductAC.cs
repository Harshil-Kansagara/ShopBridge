using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.ApplicationClass
{
    public class PagedProductAC
    {
        /// <summary>
        /// Count of total products.
        /// </summary>
        public int TotalProductsCount { get; set; }
        /// <summary>
        /// List of products (after paging/filetring/sorting).
        /// </summary>
        public List<ProductAC> Products { get; set; }
    }
}
